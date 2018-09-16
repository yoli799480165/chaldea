﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Chaldea.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chaldea.Services
{
    [Route("api/bangumi")]
    public class BangumiService : ServiceBase
    {
        private readonly IRepository<string, Bangumi> _bangumiRepository;

        public BangumiService(IRepository<string, Bangumi> bangumiRepository)
        {
            _bangumiRepository = bangumiRepository;
        }

        [Route("getlist")]
        [HttpGet]
        public async Task<ICollection<Bangumi>> GetList(int skip, int take)
        {
            if (skip == 0 && take == 0)
                return await _bangumiRepository
                    .GetAll()
                    .SortByDescending(x => x.Name)
                    .ToListAsync();

            var query = Builders<Bangumi>.Projection.Slice(x => x.Animes, 0, 6);
            var list = await _bangumiRepository
                .GetAll()
                .SortByDescending(x => x.Name)
                .Project<Bangumi>(query)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
            return list;
        }
    }
}