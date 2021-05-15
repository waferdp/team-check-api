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
        private IRepository<TeamAnswer> _teamAnswerRepository;
        private ILogger<PerformanceChecklistController> _logger;

        public PerformanceChecklistController(IRepository<TeamAnswer> teamAnswerRepository, ILogger<PerformanceChecklistController> logger)
        {
            _teamAnswerRepository = teamAnswerRepository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public TeamAnswer GetAnswer(Guid id)
        {
            _logger.LogInformation($"Get team check answer using id");
            return _teamAnswerRepository.Get(id);
        }

        [HttpGet]
        public IEnumerable<TeamAnswer> ListAnswers(DateTime? from, DateTime? to)
        {
            _logger.LogInformation($"Get team check answers");
            var answers = _teamAnswerRepository.GetAll();
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
        public async Task<TeamAnswer> NewSubmission(TeamCheckItem[] checklist)
        {
            _logger.LogInformation($"Create new team check answer");
            var answer = new TeamAnswer
            {
                Items = checklist
            };
            return await _teamAnswerRepository.SaveAsync(answer);
        }
    }
}

