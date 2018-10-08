using System;
using System.Threading.Tasks;
using Chaldea.IdentityServer.Core;
using Chaldea.IdentityServer.Repositories;
using Chaldea.IdentityServer.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Chaldea.IdentityServer.Controllers.Accounts
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SmsService _smsService;
        private readonly IRepository<string, User> _userRepository;

        public AccountController(
            ILogger<AccountController> logger,
            SmsService smsService,
            IRepository<string, User> userRepository)
        {
            _logger = logger;
            _smsService = smsService;
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
#if !DEBUG
            _smsService.SendCode(phoneNumber, code);
#endif
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
                Id = ObjectId.GenerateNewId().ToString(),
                PhoneNumber = input.PhoneNumber,
                Email = string.Empty,
                Name = string.Empty,
                Role = Roles.User,
                Password = Md5Helper.Md5(input.Password),
                IsActive = true
            };

            await _userRepository.AddAsync(user);
        }
    }
}