using System;

namespace Chaldea.Fate.Identity.Domain
{
    public class ApiResourceSecret : Secret
    {
        public Guid ApiResourceId { get; set; }

        public ApiResource ApiResource { get; set; }
    }
}
