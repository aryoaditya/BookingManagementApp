using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.Employees;
using Client.Contracts;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public IActionResult Register()
        {
            ViewData["PageTitle"] = "Register";
            return View();
        }

        // Login View Page
        public IActionResult Login()
        {
            ViewData["PageTitle"] = "Login";
            return View();
        }

        // Login Post to API
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authRepository.Login(loginDto);
                if (result.Code == 200)
                {
                    HttpContext.Session.SetString("JWToken", result.Data.Token);
                    return Redirect("../");
                }
                else if (result.Code == 409)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View();
                }
            }
            return View();
        }

        // Logout
        [HttpGet("Logout/")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}