using System;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Interface;

namespace Api.Controllers 
{
    [Route("api/teams/{teamId}")]
    public class MemberController : ControllerBase
    {
        private ITeamRepository _teamRepository;
        private ILogger<MemberController> _logger;

        public MemberController(ITeamRepository teamRepository, ILogger<MemberController> logger)
        {
            _teamRepository = teamRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<Team> AddTeamMember(Guid teamId, [FromBody] Member member)
        {
            _logger.LogInformation("Adding team member to team");
            return await _teamRepository.AddMember(teamId, member);
        }

        [HttpDelete("{memberId}")]
        public async Task<Team> RemoveTeamMember(Guid teamId, Guid memberId)
        {
            _logger.LogInformation("Removing team member from team");
            return await _teamRepository.RemoveMember(teamId, memberId);
        }
    }
}