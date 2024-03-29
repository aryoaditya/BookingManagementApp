﻿using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly ITokenHandler _tokenHandler;

        public AccountController(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository, IEmailHandler emailHandler, ITokenHandler tokenHandler, IAccountRoleRepository accountRoleRepository, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;
            _educationRepository = educationRepository;
            _universityRepository = universityRepository;
            _emailHandler = emailHandler;
            _tokenHandler = tokenHandler;
            _accountRoleRepository = accountRoleRepository;
            _roleRepository = roleRepository;
        }

        // Endpoint untuk Login
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginDto loginDto)
        {
            try
            {
                // Cari Employee berdasarkan alamat email
                var employee = _employeeRepository.GetByEmail(loginDto.Email);

                if (employee == null)
                {
                    // Tidak ada Employee dengan email yang cocok
                    return Unauthorized(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status401Unauthorized,
                        Status = HttpStatusCode.Unauthorized.ToString(),
                        Message = "Account or Password is invalid"
                    });
                }

                // Verifikasi kata sandi
                var account = _accountRepository.GetByEmployeeGuid(employee.Guid);
                var loginPass = HashingHandler.VerifyPassword(loginDto.Password, account.Password);

                if (account == null || !loginPass)
                {
                    // Password tidak valid
                    return Unauthorized(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status401Unauthorized,
                        Status = HttpStatusCode.Unauthorized.ToString(),
                        Message = "Account or Password is invalid"
                    });
                }

                // Menambahkan daftar claim (informasi data client)
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Email, employee.Email));
                claims.Add(new Claim(ClaimTypes.Name, string.Concat(employee.FirstName + " " + employee.LastName)));

                // Mendapatkan role ketika login
                var getRoleName = from ar in _accountRoleRepository.GetAll()
                                  join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                                  where ar.AccountGuid == account.Guid
                                  select r.Name;

                foreach (var roleName in getRoleName)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleName));
                }

                // Generate token
                var generateToken = _tokenHandler.Generate(claims);

                // Jika login berhasil,
                return Ok(new ResponseOkHandler<object>("Login success", new { Token = generateToken }));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to login",
                    Error = ex.Message
                });
            }
        }

        // Endpoint untuk Forgot Password
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                // Get employee by email
                Employee? employee = _employeeRepository.GetByEmail(email);

                if (employee == null)
                {
                    // Employee dengan email yang diberikan tidak ditemukan
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Account Not Found"
                    });
                }

                // Get account dari Guid employee
                var account = _accountRepository.GetByGuid(employee.Guid);

                // Buat OTP secara acak
                var otp = _accountRepository.GenerateRandomOtp();

                Account toUpdate = account;
                toUpdate.Otp = otp;
                toUpdate.ExpiredDate = DateTime.Now.AddMinutes(5); // Token kedaluwarsa dalam 5 menit
                toUpdate.IsUsed = false;

                _accountRepository.Update(toUpdate);

                _emailHandler.Send("Forgot Password", $"Your OTP is {otp}", email);

                return Ok(new ResponseOkHandler<object>("OTP has been sent to your email or phone."));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to send OTP",
                    Error = ex.Message
                });
            }
        }

        // Endpoint untuk register
        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register(RegisterDto registerDto)
        {
            using var transaction = _employeeRepository.BeginTransaction(); // Mulai transaksi

            try
            {
                // Buat objek Employee
                Employee newEmployee = registerDto;
                newEmployee.Nik = GenerateHandler.GenerateNIK(_employeeRepository.GetLastNik());
                _employeeRepository.Create(newEmployee);

                // Cek apakah universitas sudah ada berdasarkan kode dan nama
                var existingUniversity = _universityRepository.GetByCodeAndName(registerDto.UniversityCode, registerDto.UniversityName);

                if (existingUniversity == null)
                {
                    // Jika universitas belum ada, buat universitas baru
                    University newUniversity = registerDto;
                    _universityRepository.Create(newUniversity);

                    // Set universitas untuk penggunaan selanjutnya
                    existingUniversity = newUniversity;
                }

                // Buat objek Education jika diperlukan
                Education newEducation = registerDto;
                newEducation.Guid = newEmployee.Guid;
                newEducation.UniversityGuid = existingUniversity.Guid;
                _educationRepository.Create(newEducation);

                // Buat objek Account
                Account newAccount = registerDto;
                newAccount.Guid = newEmployee.Guid;
                newAccount.Password = HashingHandler.HashPassword(newAccount.Password);
                _accountRepository.Create(newAccount);

                // Buat accountRole "user"
                var accountRole = _accountRoleRepository.Create(new AccountRole
                {
                    AccountGuid = newAccount.Guid,
                    RoleGuid = _roleRepository.GetDefaultRoleGuid() ?? throw new Exception("Default Role Not Found")
                });

                // Commit transaksi jika semuanya berhasil
                transaction.Commit();

                return Ok(new ResponseOkHandler<EmployeeDto>((EmployeeDto)newEmployee));
            }
            catch (ExceptionHandler ex)
            {
                // Rollback transaksi jika terjadi kesalahan
                transaction.Rollback();

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to register",
                    Error = ex.Message
                });
            }
        }

        // Endpoint untuk Change Password
        [HttpPost("change-password")]
        [AllowAnonymous]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                // Mendapatkan Employee berdasarkan email
                var employee = _employeeRepository.GetByEmail(changePasswordDto.Email);

                // Dapatkan Account berdasarkan email
                var account = _accountRepository.GetByGuid(employee.Guid);

                if (account == null)
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Account not found"
                    });
                }

                // Periksa apakah OTP benar
                if (account.Otp != changePasswordDto.Otp)
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Invalid OTP"
                    });
                }

                // Periksa apakah OTP sudah digunakan
                if (account.IsUsed)
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "OTP already used"
                    });
                }

                // Periksa apakah OTP sudah kadaluarsa
                if (account.ExpiredDate < DateTime.Now)
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "OTP has expired"
                    });
                }

                // Periksa apakah NewPassword dan ConfirmPassword sama
                if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "New password and confirm password do not match"
                    });
                }

                // Update password dan tandai OTP sebagai digunakan
                account.Password = HashingHandler.HashPassword(changePasswordDto.NewPassword);
                _accountRepository.Update(account);
                account.IsUsed = true;

                return Ok(new ResponseOkHandler<string>("Password changed successfully"));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to change password",
                    Error = ex.Message
                });
            }
        }

        // HTTP GET untuk mengambil semua data Account
        [HttpGet]
        [Authorize(Roles = "manager, admin")]
        public IActionResult GetAll()
        {
            var result = _accountRepository.GetAll();
            if (!result.Any())
            {
                // Mengembalikan pesan jika tidak ada data yang ditemukan
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            var data = result.Select(x => (AccountDto)x);

            // Mengembalikan data Employee jika ada
            return Ok(new ResponseOkHandler<IEnumerable<AccountDto>>(data));
        }

        // HTTP GET untuk mengambil data Account berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _accountRepository.GetByGuid(guid);
            if (result is null)
            {
                // Mengembalikan pesan jika ID tidak ditemukan
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "ID Not Found"
                });
            }
            // Mengembalikan data Employee jika ditemukan
            return Ok(new ResponseOkHandler<AccountDto>((AccountDto)result));
        }

        // HTTP POST untuk membuat data Account baru
        [HttpPost]
        public IActionResult Create(CreateAccountDto accountDto)
        {
            try
            {
                Account toCreate = accountDto;
                toCreate.Password = HashingHandler.HashPassword(accountDto.Password);
                var result = _accountRepository.Create(toCreate);

                // Mengembalikan data Employee yang baru saja dibuat
                return Ok(new ResponseOkHandler<AccountDto>((AccountDto)result));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to create data",
                    Error = ex.Message
                });
            }
        }

        // HTTP PUT untuk memperbarui data Account berdasarkan GUID
        [HttpPut]
        public IActionResult Update(AccountDto accountDto)
        {
            try
            {
                var entity = _accountRepository.GetByGuid(accountDto.Guid);
                if (entity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                Account toUpdate = accountDto;
                toUpdate.CreatedDate = entity.CreatedDate;
                toUpdate.Password = HashingHandler.HashPassword(accountDto.Password);

                _accountRepository.Update(toUpdate);

                return Ok(new ResponseOkHandler<AccountDto>("Data updated successfully")); // Mengembalikan pesan sukses jika pembaruan berhasil
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to update data",
                    Error = ex.Message
                });
            }
        }

        // HTTP DELETE untuk menghapus data Account berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                var existingAccount = _accountRepository.GetByGuid(guid);
                if (existingAccount is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                _accountRepository.Delete(existingAccount);

                return Ok(new ResponseOkHandler<AccountDto>("Data deleted successfully"));  // Mengembalikan pesan sukses jika penghapusan berhasil
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to delete data",
                    Error = ex.Message
                });
            }
        }

        // Endpoint Mendapatkan claims payload
        [Authorize]
        [HttpGet("GetClaims/{token}")]
        public IActionResult GetClaims(string token)
        {
            var claims = _tokenHandler.ExtractClaimsFromJwt(token);
            return Ok(new ResponseOkHandler<ClaimsDTO>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Claims has been retrieved",
                Data = claims
            });
        }
    }
}