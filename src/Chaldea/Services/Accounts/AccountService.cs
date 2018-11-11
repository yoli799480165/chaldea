using System;
using System.Threading.Tasks;
using Chaldea.Core.Repositories;
using Chaldea.Core.Sms;
using Chaldea.Core.Utilities;
using Chaldea.Services.Accounts.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Chaldea.Services.Accounts
{
    [Route("api/account")]
    public class AccountService : ServiceBase
    {
        private readonly ILogger<AccountService> _logger;
        private readonly SmsManager _smsManager;
        private readonly IRepository<string, User> _userRepository;

        public AccountService(
            ILogger<AccountService> logger,
            SmsManager smsManager,
            IRepository<string, User> userRepository)
        {
            _logger = logger;
            _smsManager = smsManager;
            _userRepository = userRepository;
        }

        [Route("getCode")]
        [HttpGet]
        public void GetCode(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                throw new Exception($"Invalid parameter {nameof(phoneNumber)}");

            if (!RegexHelper.IsTelephone(phoneNumber))
                throw new Exception("Phone number format is incorrect.");

            var code = new Random().Next(1000, 9999).ToString();
            HttpContext.Session.SetString("code", code);
            _logger.LogInformation($"PhoneNumber: {phoneNumber}, Verification code: {code}");
        }

        [Route("register")]
        [HttpPost]
        public async Task Register([FromBody] RegisterDto input)
        {
            if (input == null)
                throw new Exception($"Invalid parameter {nameof(input)}");

            if (string.IsNullOrEmpty(input.PhoneNumber))
                throw new Exception($"Invalid parameter {nameof(input.PhoneNumber)}");

            if (string.IsNullOrEmpty(input.Password))
                throw new Exception($"Invalid parameter {nameof(input.Password)}");

            if (string.IsNullOrEmpty(input.Code))
                throw new Exception($"Invalid parameter {nameof(input.Code)}");

            var code = HttpContext.Session.GetString("code");

            if (input.Code != code)
                throw new Exception($"Invalid code: {input.Code}");

            var count = await _userRepository.CountAsync(x => x.PhoneNumber == input.PhoneNumber);
            if (count > 0)
                throw new Exception("The phone number already exists.");

            var user = new User
            {
                Id = Guid.NewGuid().ToString("N"),
                PhoneNumber = input.PhoneNumber,
                Email = string.Empty,
                Name = string.Empty,
                Role = nameof(UserRoles.Human),
                Password = Md5Helper.Md5(input.Password),
                IsActive = true
            };

            await _userRepository.AddAsync(user);
        }
    }
}