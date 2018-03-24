﻿using AuthenticationApi.Domain;
using AuthenticationApi.Dtos;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace AuthenticationApi.Controllers
{
    [Route("api/v1/auth/")]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // POST api/values
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginData login)
        {
            if (login?.Email == null || login?.Password == null) return BadRequest("Password and/or email is missing.");

            var authConfiguration =  configuration.GetSection("AuthDbConnection");

            // TODO DI
            var repo = new AuthRepo(
                authConfiguration.GetValue<string>("ConnectionString"),
                authConfiguration.GetValue<string>("DbName"),
                authConfiguration.GetValue<string>("CollectionName"));

            var auth = repo.GetAuthentication(login.Email);
            var passwordHash = login.Password.ToSHA256Hash();

            if (auth == null ||
                auth.Password?.Equals(passwordHash, StringComparison.InvariantCulture) == false ||
                auth.Email?.Equals(login.Email, StringComparison.InvariantCultureIgnoreCase) == false) return Unauthorized();

            var keyparts = configuration.GetSection("Secrets:RSA-PrivateKey").GetChildren().Select(c => c.Value);

            var privateKey = string.Join(Environment.NewLine, keyparts);
            var jwtHelpert = new JwtHelper(privateKey);
            var jwtBearerToken =  jwtHelpert.CreatePayload(auth).CreateToken();

            HttpContext.Response.Cookies.Append("SESSIONID", jwtBearerToken, new CookieOptions() { HttpOnly = true, Secure = true });

            return Created("", "Well done! You are now logged in.");
        }

        [HttpPost("logout")]
        public void Logout()
        {
            // TODO authorize
            HttpContext.Response.Cookies.Delete("SESSIONID");
        }
    }
}