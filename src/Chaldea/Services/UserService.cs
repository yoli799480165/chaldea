using System;
using System.Threading.Tasks;
using Chaldea.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chaldea.Services
{
    [Route("api/user")]
    public class UserService : ServiceBase
    {
        private readonly IRepository<string, User> _userRepository;

        public UserService(IRepository<string, User> userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize]
        [Route("getUser")]
        [HttpGet]
        public async Task<User> GetUser()
        {
            foreach (var claim in HttpContext.User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
            return new User();
        }
    }
}