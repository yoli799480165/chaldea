﻿namespace Chaldea.IdentityServer.Controllers.Accounts
{
    public class RegisterDto
    {
        public string PhoneNumber { get; set; }

        public string Code { get; set; }

        public string Password { get; set; }
    }
}