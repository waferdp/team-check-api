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
        public TeamAssessment CalculateTeam(TeamQuery query)
        {
            var answers = _teamAnswerRepository.GetAll();
            var matched = query.Match(answers);
            var assessment = new TeamAssessment(matched);
            return assessment;
        }
    }
}