using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Interface;

namespace Api.Controllers
{
    [Route("api/performance-checklist")]
    [ApiController]
    public class PerformanceChecklistController : ControllerBase
    {
        private IRepository<TeamCheckAnswer> _teamCheckAnswerRepository;
        private ILogger<PerformanceChecklistController> _logger;

        public PerformanceChecklistController(IRepository<TeamCheckAnswer> teamCheckAnswerRepository, ILogger<PerformanceChecklistController> logger)
        {
            _teamCheckAnswerRepository = teamCheckAnswerRepository;
            _logger = logger;
        }

        [HttpGet]
        public TeamCheckAnswer GetAnswer(Guid id)
        {
            _logger.LogInformation($"Get team check answer using id");
            return _teamCheckAnswerRepository.Get(id);
        }

        [HttpGet]
        public IEnumerable<TeamCheckAnswer> ListAnswers(DateTime? from, DateTime? to)
        {
            _logger.LogInformation($"Get team check answers");
            var answers = _teamCheckAnswerRepository.GetAll();
            if(from.HasValue)
            {
                answers = answers.Where(answer => answer.Created >= from);
            }
            if(to.HasValue)
            {
                answers = answers.Where(answer => answer.Created < to);
            }
            return answers.ToList();
        }

        [HttpPost]
        public async Task<TeamCheckAnswer> NewSubmission(TeamCheckItem[] checklist)
        {
            _logger.LogInformation($"Create new team check answer");
            var answer = new TeamCheckAnswer
            {
                Items = checklist
            };
            return await _teamCheckAnswerRepository.SaveAsync(answer);
        }
    }
}

