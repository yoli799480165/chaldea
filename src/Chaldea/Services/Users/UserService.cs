using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Core.Repositories;
using Chaldea.Core.Utilities;
using Chaldea.Exceptions;
using Chaldea.Services.Dtos;
using Chaldea.Services.Users.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Chaldea.Services.Users
{
    [Authorize]
    [Route("api/user")]
    public class UserService : ServiceBase
    {
        private readonly IRepository<string, User> _userRepository;

        public UserService(
            IRepository<string, User> userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [Route("getUsers")]
        [HttpGet]
        public async Task<ICollection<UserDto>> GetUsers(int skip, int take)
        {
            ICollection<User> users;
            if (skip >= 0 && take > 0)
                users = await _userRepository.GetAll(x => x.Role != nameof(UserRoles.Admin)).Skip(skip).Limit(take)
                    .ToListAsync();
            else
                users = await _userRepository.GetAllListAsync(x => x.Role != nameof(UserRoles.Admin));

            return Mapper.Map<ICollection<UserDto>>(users);
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [Route("addUser")]
        [HttpPut]
        public async Task AddUser([FromBody] UserDto input)
        {
            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");
            var user = Mapper.Map<User>(input);
            user.Id = Guid.NewGuid().ToString("N");
            user.Password = Md5Helper.Md5("123qwe");
            await _userRepository.AddAsync(user);
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [Route("{id}/removeUser")]
        [HttpDelete]
        public async Task RemoveUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new UserFriendlyException($"Invalid parameter {nameof(id)}");
            await _userRepository.DeleteAsync(id);
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [Route("getRoles")]
        [HttpGet]
        public ICollection<DropdownItem> GetRoles()
        {
            var weeks = new List<DropdownItem>();
            var items = typeof(UserRoles).GetEnumValues();
            foreach (var item in items)
                weeks.Add(new DropdownItem
                {
                    Text = item.ToString(),
                    Value = item.ToString()
                });
            return weeks;
        }
    }
}