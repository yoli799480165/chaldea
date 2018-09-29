using System.Security.Claims;
using System.Threading.Tasks;
using Chaldea.IdentityServer.Repositories;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace Chaldea.IdentityServer.Configuration
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IRepository<string, User> _userRepository;

        public ResourceOwnerPasswordValidator(IRepository<string, User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            var user = await _userRepository.GetAsync(x =>
                x.Name == context.UserName && x.Password == context.Password);
            if (user != null)
                context.Result = new GrantValidationResult(
                    user.Id,
                    "custom",
                    GetUserClaims(user));
            else
                context.Result =
                    new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
        }

        //可以根据需要设置相应的Claim
        private Claim[] GetUserClaims(User user)
        {
            return new[]
            {
                new Claim("userId", user.Id),
                new Claim(JwtClaimTypes.Name, user.Name),
                new Claim(JwtClaimTypes.GivenName, user.GivenName),
                new Claim(JwtClaimTypes.FamilyName, user.FamilyName),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.EmailVerified, user.EmailVerified.ToString(), ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.WebSite, user.WebSite),
                new Claim(JwtClaimTypes.Address, user.Address)
            };
        }
    }
}