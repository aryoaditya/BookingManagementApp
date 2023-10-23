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
using System.Security.Claims;

namespace Client.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFound()
        {
            var fullNameClaim = User.FindFirst(ClaimTypes.Name);
            if (fullNameClaim != null)
            {
                var fullName = fullNameClaim.Value;
                ViewData["FullName"] = fullName;
            }

            return View();
        }

        public IActionResult Unauthorized()
        {
            return View();
        }

        public IActionResult Forbidden()
        {
            var fullNameClaim = User.FindFirst(ClaimTypes.Name);
            if (fullNameClaim != null)
            {
                var fullName = fullNameClaim.Value;
                var firstName = fullNameClaim.Value.Split(' ')[0];
                ViewData["FullName"] = fullName;
                ViewData["FirstName"] = firstName;
            }
            return View();
        }
    }
}