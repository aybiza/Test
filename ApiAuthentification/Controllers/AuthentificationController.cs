using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using BLL.Models;
using BLL.Services.Users;

namespace ApiAuthentification.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AuthentificationController : Controller
    {
        IUserService userService = new UserService();

        private IConfigurationRoot _config;

        public AuthentificationController()
        {
        }

        public AuthentificationController(IConfigurationRoot config)
        {
            _config = config;
        }

            

        // POST api/values
        [HttpPost]
        public bool Authenticate([FromBody]string login, string password)
        {
            User user = userService.GetUser(login, "Login");
            if(user != null && user.Password == password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string BuildToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Tokens:Issuer"], _config["Tokens:Issuer"], expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        public IActionResult Confidentials([FromBody]string email)
        {
            IActionResult response = Unauthorized();
            User user = userService.GetUser(email, "Email");

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

    }
}
