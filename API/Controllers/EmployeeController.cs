using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IAccountRepository _accountRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository, IAccountRepository accountRepository)
        {
            _employeeRepository = employeeRepository;
            _educationRepository = educationRepository;
            _universityRepository = universityRepository;
            _accountRepository = accountRepository;
        }

        // Endpoint untuk Login
        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
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

            // Jika login berhasil,
            return Ok(new ResponseOkHandler<LoginDto>("Login success"));
        }

        // Endpoint untuk Forgot Password
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                // Get employee by email
                var employee = _employeeRepository.GetByEmail(forgotPasswordDto.Email);

                // Get account dari Guid employee
                var account = _accountRepository.GetByGuid(employee.Guid);

                if (employee == null)
                {
                    // Employee dengan email yang diberikan tidak ditemukan
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Employee Not Found"
                    });
                }

                // Buat OTP secara acak
                var otp = _accountRepository.GenerateRandomOtp();

                Account toUpdate = account;
                toUpdate.Otp = otp;
                toUpdate.ExpiredDate = DateTime.Now.AddMinutes(5); // Token kedaluwarsa dalam 5 menit

                _accountRepository.Update(toUpdate);

                return Ok(new ResponseOkHandler<AccountDto>((AccountDto)toUpdate));

                //return Ok(new ResponseOkHandler<EmployeeDto>("OTP has been sent to your email or phone."));
            }
            catch (Exception ex)
            {
                // Handle kesalahan jika terjadi
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
        public IActionResult Register(RegisterEmployeeDto registerDto)
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

                // Commit transaksi jika semuanya berhasil
                transaction.Commit();

                return Ok(new ResponseOkHandler<EmployeeDto>((EmployeeDto)newEmployee));
            }
            catch (Exception ex)
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

        // Endpoint untuk menampilkan detail Employee dengan join
        [HttpGet("details")]
        public IActionResult GetDetails()
        {
            var employees = _employeeRepository.GetAll();
            var educations = _educationRepository.GetAll();
            var universities = _universityRepository.GetAll();

            if (!(employees.Any() && educations.Any() && universities.Any()))
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            var employeeDetails = from emp in employees
                                  join edu in educations on emp.Guid equals edu.Guid
                                  join unv in universities on edu.UniversityGuid equals unv.Guid
                                  select new EmployeeDetailDto
                                  {
                                      Guid = emp.Guid,
                                      Nik = emp.Nik,
                                      FullName = string.Concat(emp.FirstName, " ", emp.LastName),
                                      BirthDate = emp.BirthDate,
                                      Gender = emp.Gender.ToString(),
                                      HiringDate = emp.HiringDate,
                                      Email = emp.Email,
                                      PhoneNumber = emp.PhoneNumber,
                                      Major = edu.Major,
                                      Degree = edu.Degree,
                                      Gpa = edu.Gpa,
                                      University = unv.Name
                                  };

            return Ok(new ResponseOkHandler<IEnumerable<EmployeeDetailDto>>(employeeDetails));
        }

        // HTTP GET untuk mengambil semua data Employee
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _employeeRepository.GetAll();
            if (!result.Any())
            {
                // Mengembalikan pesan jika tidak ada data yang ditemukan menggunakan error handler
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                }); 
            }

            var data = result.Select(x => (EmployeeDto)x);

            // Mengembalikan data Employee jika ada menggunakan handler
            return Ok(new ResponseOkHandler<IEnumerable<EmployeeDto>>(data));  
        }

        // HTTP GET untuk mengambil data Employee berdasarkan GUID
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _employeeRepository.GetByGuid(guid);
            if (result is null)
            {
                // Mengembalikan pesan jika ID tidak ditemukan menggunakan error handler
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "ID Not Found"
                }); 
            }
            
            // Mengembalikan data Employee jika ditemukan
            return Ok(new ResponseOkHandler<EmployeeDto>((EmployeeDto)result));
        }

        // HTTP POST untuk membuat data Employee baru
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto employeeDto)
        {
            try
            {
                Employee toCreate = employeeDto;
                toCreate.Nik = GenerateHandler.GenerateNIK(_employeeRepository.GetLastNik());
                var result = _employeeRepository.Create(toCreate);

                // Mengembalikan data Employee yang baru saja dibuat
                return Ok(new ResponseOkHandler<EmployeeDto>((EmployeeDto)result));
            }
            catch (ExceptionHandler ex)
            {
                // Memunculkan pesan error jika ada kesalahan pada server/database
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to create data",
                    Error = ex.Message
                });
            }             
        }

        // HTTP PUT untuk memperbarui data Employee berdasarkan GUID
        [HttpPut]
        public IActionResult Update(EmployeeDto employeeDto)
        {
            try
            {
                var entity = _employeeRepository.GetByGuid(employeeDto.Guid);
                if (entity is null)
                {
                    // Mengembalikan pesan jika ID tidak ditemukan menggunakan error handler
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                Employee toUpdate = employeeDto;
                toUpdate.CreatedDate = entity.CreatedDate;
                //toUpdate.Nik = entity.Nik;

                _employeeRepository.Update(toUpdate);

                return Ok(new ResponseOkHandler<EmployeeDto>("Data updated successfully")); // Mengembalikan pesan sukses jika pembaruan berhasil
            }
            catch (ExceptionHandler ex)
            {
                // Memunculkan pesan error jika ada kesalahan pada server/database
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to update data",
                    Error = ex.Message
                });
            }
        }

        // HTTP DELETE untuk menghapus data Employee berdasarkan GUID
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                var existingEmployee = _employeeRepository.GetByGuid(guid);
                if (existingEmployee is null)
                {
                    // Mengembalikan pesan jika ID tidak ditemukan menggunakan error handler
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                _employeeRepository.Delete(existingEmployee);

                return Ok(new ResponseOkHandler<EmployeeDto>("Data deleted successfully"));  // Mengembalikan pesan sukses jika penghapusan berhasil
            }
            catch (ExceptionHandler ex)
            {
                // Memunculkan pesan error jika ada kesalahan pada server/database
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to delete data",
                    Error = ex.Message
                });
            }
        }
    }
}
