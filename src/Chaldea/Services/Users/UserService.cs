using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chaldea.Services
{
    [Route("api/user")]
    public class UserService : ServiceBase
    {
        [Authorize]
        [Route("getUsers")]
        [HttpGet]
        public async Task GetUsers()
        {
            foreach (var claim in HttpContext.User.Claims)
                Console.WriteLine($"{claim.Type}: {claim.Value}");
        }
    }
}