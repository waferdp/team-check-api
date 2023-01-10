using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Interface;

namespace Api.Controllers 
{
    [Route("api/teams")]
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
        public async Task<IEnumerable<Team>> GetTeams()
        {
            _logger.LogInformation("Get all teams");
            var allTeams = await _teamRepository.GetAllAsync();
            return allTeams; 
        }

        [HttpGet("{id}")] 
        public Team GetTeam(Guid id)
        {
            _logger.LogInformation("Get team by Id");
            var team = _teamRepository.Get(id);
            return team;
        }

        [HttpPost]
        public async Task<Team> SaveTeam([FromBody] Team team)
        {
            _logger.LogInformation("Create new team");
            return await _teamRepository.SaveAsync(team);
        }

        [HttpDelete("{id}")]
        public async Task DeleteTeam(Guid id)
        {
            _logger.LogInformation("Deleting team");
            await _teamRepository.DeleteAsync(id);
        }

    }
}