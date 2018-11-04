using System.Collections.Generic;
using System.Threading.Tasks;
using Chaldea.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chaldea.Services.Banners
{
    [Route("api/banner")]
    public class BannerService : ServiceBase
    {
        private readonly IRepository<string, Banner> _bannerRepository;

        public BannerService(IRepository<string, Banner> bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [Route("add")]
        [HttpPut]
        public async Task Add([FromBody] Banner banner)
        {
            banner.Id = ObjectId.GenerateNewId().ToString();
            await _bannerRepository.AddAsync(banner);
        }

        [Route("getList")]
        [HttpGet]
        public async Task<ICollection<Banner>> GetList(int skip, int take)
        {
            var list = await _bannerRepository
                .GetAll()
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
            return list;
        }
    }
}