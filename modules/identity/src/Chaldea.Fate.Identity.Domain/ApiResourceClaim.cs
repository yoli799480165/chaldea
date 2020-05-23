using System;

namespace Chaldea.Fate.Identity.Domain
{
    public class ApiResourceClaim : Claim
    {
        public Guid ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }
}