using System;
using System.Linq;
using System.Threading.Tasks;
using Chaldea.IdentityServer.Repositories;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Chaldea.IdentityServer.Configuration
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository<string, User> _userRepository;

        public ProfileService(IRepository<string, User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                var claims = context.Subject.Claims.ToList();

                //set issued claims to return
                context.IssuedClaims = claims.ToList();
            }
            catch (Exception)
            {
                //log your error
            }

            await Task.CompletedTask;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            try
            {
                if (!string.IsNullOrEmpty(subjectId))
                {
                    var user = await _userRepository.GetAsync(subjectId);

                    if (user != null)
                        context.IsActive = user.IsActive;
                }
            }
            catch (Exception)
            {
                //handle error logging
            }
        }
    }
}