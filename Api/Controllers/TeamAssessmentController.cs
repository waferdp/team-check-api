using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace Api.Controllers
{
    [Route("api/team-assessment")]
    [ApiController]
    public class TeamAssessmentController : ControllerBase
    {
        private ITeamCheckAnswerRepository _teamCheckAnswerRepository;

        public TeamAssessmentController(ITeamCheckAnswerRepository teamCheckAnswerRepository)
        {
            _teamCheckAnswerRepository = teamCheckAnswerRepository;
        }

        [HttpGet]
        public TeamAssessment CalculateTeam(DateTime? from, DateTime? to)
        {
            var answers = _teamCheckAnswerRepository.GetAll();
            var assessment = new TeamAssessment(answers);
            return assessment;
        }
    }
}