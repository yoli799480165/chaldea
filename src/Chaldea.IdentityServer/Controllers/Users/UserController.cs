using System.Collections.Generic;
using System.Threading.Tasks;
using Chaldea.IdentityServer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chaldea.IdentityServer.Controllers.Users
{
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IRepository<string, User> _userRepository;

        public UserController(IRepository<string, User> userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize]
        [Route("getUsers")]
        [HttpGet]
        public async Task<ICollection<User>> GetUsers()
        {
            return await _userRepository.GetAllListAsync();
        }

        [Authorize]
        [Route("addUser")]
        [HttpPut]
        public async Task AddUser([FromBody] User user)
        {
            await _userRepository.AddAsync(user);
        }

        [Authorize]
        [Route("removeUser")]
        [HttpDelete]
        public async Task RemoveUser(string id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}