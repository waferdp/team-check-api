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
    [Route("api/performance-checklists")]
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
            _logger.LogInformation($"Get team answer using id");
            return _teamAnswerRepository.Get(id);
        }

        [HttpGet]
        public IEnumerable<TeamAnswer> ListAnswers([FromQuery]TeamQuery query)
        {
            _logger.LogInformation($"Get team answers");
            var answers = _teamAnswerRepository.GetAll();
            return query.Match(answers);

        }

        [HttpPost]
        public async Task<TeamAnswer> NewSubmission(TeamCheckItem[] checklist)
        {
            _logger.LogInformation($"Create new team answer");
            var answer = new TeamAnswer
            {
                Items = checklist
            };
            return await _teamAnswerRepository.SaveAsync(answer);
        }
    }
}

