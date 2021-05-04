using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repository.Interface;

namespace Api.Controllers
{
    [Route("api/performance-checklist")]
    [ApiController]
    public class PerformanceChecklistController : ControllerBase
    {
        private ITeamCheckAnswerRepository _teamCheckAnswerRepository;
        public PerformanceChecklistController(ITeamCheckAnswerRepository teamCheckAnswerRepository)
        {
            _teamCheckAnswerRepository = teamCheckAnswerRepository;
        }

        [HttpGet]
        public IEnumerable<TeamCheckAnswer> ListAnswers(DateTime? from, DateTime? to)
        {
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
        public async Task NewSubmission(TeamCheckItem[] checklist)
        {
            var answer = new TeamCheckAnswer
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                items = checklist
            };
            await _teamCheckAnswerRepository.SaveAnswer(answer);
            foreach(var item in checklist)
            {
                Console.WriteLine(item.Key + ": " + item.Value);
            }
        }
    }
}

