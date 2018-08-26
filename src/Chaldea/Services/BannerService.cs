using System.Collections.Generic;
using System.Threading.Tasks;
using Chaldea.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chaldea.Services
{
    [Route("api/banner")]
    public class BannerService : ServiceBase
    {
        private readonly IRepository<ObjectId, Banner> _bannerRepository;

        public BannerService(IRepository<ObjectId, Banner> bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        [Route("add")]
        [HttpPut]
        public async Task Add([FromBody] Banner banner)
        {
            banner.Id = ObjectId.GenerateNewId();
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