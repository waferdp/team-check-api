using System;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Repository.Interface;

namespace Repository
{
    public class TeamRepository : SimpleRepository<Team>, ITeamRepository
    {
        private ILogger<TeamRepository> _logger;

        public TeamRepository(IConfiguration configuration, ILogger<TeamRepository> logger) : base (configuration, logger)
        {
            _logger = logger;
        }

        public async Task<Team> AddMember(Guid teamId, Member member)
        {
            _logger.LogInformation("Updating team, adding team member");
            var collection = GetCollection();
            var idFilter = CreateIdFilter(teamId);
            var addMemberDefinition = Builders<Team>.Update.Push<Member>(t => t.Members, member);
            await collection.UpdateOneAsync(idFilter, addMemberDefinition);
            return base.Get(teamId);
        }

        public async Task<Team> RemoveMember(Guid teamId, Guid memberId)
        {
            _logger.LogInformation("Updating team, removing team member");
            var collection = GetCollection();
            var idFilter = CreateIdFilter(teamId);
            var memberPullFilter = Builders<Team>.Update.PullFilter<Member>(t => t.Members, member => member.Id == memberId);
            await collection.UpdateOneAsync(idFilter, memberPullFilter);
            return base.Get(teamId);
        }
    }
}