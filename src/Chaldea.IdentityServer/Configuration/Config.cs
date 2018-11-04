using System;
using System.Collections.Generic;
using Chaldea.Core.Repositories;
using Chaldea.Core.Utilities;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Chaldea.IdentityServer.Configuration
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResourceResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API", new List<string> {JwtClaimTypes.Role})
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "chaldea-mobile",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    ClientSecrets =
                    {
                        new Secret("613b807c57a641e881be80c6333de409".Sha256())
                    },
                    AllowedScopes =
                    {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                new Client
                {
                    ClientId = "chaldea-client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowOfflineAccess = true,
                    ClientSecrets =
                    {
                        new Secret("4c173206125741f49a20ff29a04896a7".Sha256())
                    },
                    AllowedScopes =
                    {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }

//                new Client
//                {
//                    ClientId = "client2",
//                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
//                    AllowOfflineAccess = true,
//                    ClientSecrets =
//                    {
//                        new Secret("secret".Sha256())
//                    },
//                    AllowedScopes =
//                    {
//                        "api1",
//                        IdentityServerConstants.StandardScopes.OpenId,
//                        IdentityServerConstants.StandardScopes.Profile
//                    }
//                }
            };
        }

        public static IEnumerable<User> GetTestUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = "admin",
                    PhoneNumber = "admin",
                    Address = "Address",
                    Email = "admin@chaldea.cn",
                    EmailVerified = true,
                    FamilyName = "admin",
                    GivenName = "admin",
                    IsActive = true,
                    Password = Md5Helper.Md5("123qwe"),
                    WebSite = "WebSite",
                    Role = nameof(UserRoles.Admin)
                }
            };
        }
    }
}