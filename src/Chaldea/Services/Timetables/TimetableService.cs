using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Chaldea.Core.Repositories;
using Chaldea.Exceptions;
using Chaldea.Services.Dtos;
using Chaldea.Services.Timetables.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Chaldea.Services.Timetables
{
    [Authorize(Roles = nameof(UserRoles.Admin))]
    [Route("api/timetable")]
    public class TimetableService : ServiceBase
    {
        private readonly ILogger<TimetableService> _logger;
        private readonly IRepository<string, Timetable> _timetableRepository;

        public TimetableService(
            ILogger<TimetableService> logger,
            IRepository<string, Timetable> timetableRepository)
        {
            _logger = logger;
            _timetableRepository = timetableRepository;
        }
        
        [Route("createTimetable")]
        [HttpPut]
        public async Task CreateTimetable([FromBody] Timetable input)
        {
            if (input == null)
                throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            try
            {
                input.Id = Guid.NewGuid().ToString("N");
                await _timetableRepository.AddAsync(input);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Create timetable failed.");
            }
        }

        [Route("getList")]
        [HttpGet]
        public async Task<ICollection<TimetableDto>> GetList(int skip, int take)
        {
            var stages = new List<string>
            {
                "{$lookup: {from:'animes', localField:'animeId',foreignField:'_id',as:'anime'}}",
                "{$project:{'_id':1,'sourceUrl':1,'sourcePwd':1,'updateTime':1,'updateWeek':1,'anime._id':1,'anime.title':1,'anime.cover':1}}",
                "{$unwind: '$anime'}"
            };

            if (skip >= 0 && take > 0)
            {
                stages.Add("{$skip:" + skip + "}");
                stages.Add("{$limit:" + take + "}");
            }

            var pipeline = PipelineDefinition<Timetable, TimetableDto>.Create(stages);
            var timetables = await _timetableRepository.Aggregate(pipeline).ToListAsync();
            var culture = new CultureInfo("zh-cn");
            foreach (var timetable in timetables)
                timetable.WeekName = culture.DateTimeFormat.GetDayName(timetable.UpdateWeek);
            return timetables;
        }

        [Route("getWeeks")]
        [HttpGet]
        public List<DropdownItem> GetWeeks()
        {
            var weeks = new List<DropdownItem>();
            var culture = new CultureInfo("zh-cn");
            var items = typeof(DayOfWeek).GetEnumValues();
            foreach (var item in items)
                weeks.Add(new DropdownItem
                {
                    Text = culture.DateTimeFormat.GetDayName((DayOfWeek) item),
                    Value = ((int) item).ToString()
                });
            return weeks;
        }
    }
}