using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Core.Repositories;
using Chaldea.Core.Utilities;
using Chaldea.Exceptions;
using Chaldea.Services.Users.Dto;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Chaldea.Services.Users
{
    [Route("api/user")]
    public class UserService : ServiceBase
    {
        private readonly IRepository<string, User> _userRepository;

        public UserService(
            IRepository<string, User> userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("getUsers")]
        [HttpGet]
        public async Task<ICollection<UserDto>> GetUsers(int skip, int take)
        {
            ICollection<User> users;
            if (skip >= 0 && take > 0)
                users = await _userRepository.GetAll().Skip(skip).Limit(take).ToListAsync();
            else
                users = await _userRepository.GetAllListAsync();

            return Mapper.Map<ICollection<UserDto>>(users);
        }

        [Route("addUser")]
        [HttpPut]
        public async Task AddUser([FromBody] UserDto input)
        {
            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");
            var user = Mapper.Map<User>(input);
            user.Password = Md5Helper.Md5("123qwe");
            await _userRepository.AddAsync(user);
        }

        [Route("{id}/removeUser")]
        [HttpDelete]
        public async Task RemoveUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new UserFriendlyException($"Invalid parameter {nameof(id)}");
            await _userRepository.DeleteAsync(id);
        }
    }
}