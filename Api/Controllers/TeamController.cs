using System.Collections.Generic;
using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Interface;

namespace Api.Controllers 
{
    public class TeamController : ControllerBase
    {
        private IRepository<Team> _teamRepository;
        private ILogger<TeamController> _logger;

        public TeamController(IRepository<Team> teamRepository, ILogger<TeamController> logger)
        {
            _teamRepository = teamRepository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Team> GetTeams()
        {
            _logger.LogInformation($"Get teams");
            var allTeams = _teamRepository.GetAll();
            return allTeams; 
        }
    }
}