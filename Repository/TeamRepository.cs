using System;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Repository
{
    public class TeamRepository : SimpleRepository<Team>
    {
        private ILogger<TeamRepository> _logger;

        public TeamRepository(IConfiguration configuration, ILogger<TeamRepository> logger) : base (configuration, logger)
        {
            _logger = logger;
        }

        public async Task<Team> AddMemberToTeam(Guid teamId, Member member)
        {
            var collection = GetCollection();
            var idFilter = CreateIdFilter(teamId);
            var addMemberDefinition = Builders<Team>.Update.Push<Member>(t => t.Members, member);
            await collection.UpdateOneAsync(idFilter, addMemberDefinition);
            return base.Get(teamId);
        }

        public async Task<Team> RemoveMemberFromTeam(Guid teamId, Guid memberId)
        {
            var collection = GetCollection();
            var idFilter = CreateIdFilter(teamId);
            var memberPullFilter = Builders<Team>.Update.PullFilter<Member>(t => t.Members, member => member.Id == memberId);
            await collection.UpdateOneAsync(idFilter, memberPullFilter);
            return base.Get(teamId);
        }
    }
}