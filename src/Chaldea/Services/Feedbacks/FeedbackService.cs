using System;
using System.Threading.Tasks;
using AutoMapper;
using Chaldea.Core.Repositories;
using Chaldea.Exceptions;
using Chaldea.Services.Feedbacks.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Chaldea.Services.Feedbacks
{
    [Route("api/feedback")]
    public class FeedbackService : ServiceBase
    {
        private readonly IRepository<string, Feedback> _feedbackRepository;
        private readonly ILogger<FeedbackService> _logger;

        public FeedbackService(
            ILogger<FeedbackService> logger,
            IRepository<string, Feedback> feedbackRepository)
        {
            _logger = logger;
            _feedbackRepository = feedbackRepository;
        }

        [Route("addFeedback")]
        [HttpPut]
        public async Task AddFeedback([FromBody] FeedbackDto input)
        {
            if (input == null) throw new UserFriendlyException($"Invalid parameter {nameof(input)}");

            try
            {
                var feedback = Mapper.Map<Feedback>(input);
                feedback.CreationTime = DateTime.UtcNow;
                feedback.Id = Guid.NewGuid().ToString("N");

                await _feedbackRepository.AddAsync(feedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new UserFriendlyException("Add feedback failed.");
            }
        }
    }
}