using System;
using Volo.Abp.Domain.Entities;

namespace Chaldea.Fate.Identity.Domain
{
    public class ApiResourceScope : Entity<Guid>
    {
        public string Scope { get; set; }

        public Guid ApiResourceId { get; set; }

        public ApiResource ApiResource { get; set; }
    }
}