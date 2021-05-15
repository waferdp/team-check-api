using System;
using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;

namespace Api.Controllers
{
    [Route("api/team-assessments")]
    [ApiController]
    public class TeamAssessmentController : ControllerBase
    {
        private IRepository<TeamAnswer> _teamAnswerRepository;

        public TeamAssessmentController(IRepository<TeamAnswer> teamAnswerRepository)
        {
            _teamAnswerRepository = teamAnswerRepository;
        }

        [HttpGet]
        public TeamAssessment CalculateTeam(DateTime? from, DateTime? to)
        {
            var answers = _teamAnswerRepository.GetAll();
            var assessment = new TeamAssessment(answers);
            return assessment;
        }
    }
}