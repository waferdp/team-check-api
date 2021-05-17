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

        public async Task<Team> AddMemberToTeam(Team team, Member member)
        {
            var collection = GetCollection();
            var idFilter = CreateIdFilter(team.Id);
            var addMemberDefinition = Builders<Team>.Update.Push<Member>(t => t.Members, member);
            await collection.UpdateOneAsync(idFilter, addMemberDefinition);
            return team;
        }

        public async Task<Team> RemoveMemberFromTeam(Team team, Member member)
        {
            var collection = GetCollection();
            var idFilter = CreateIdFilter(team.Id);
            var removeMemberDefinition = Builders<Team>.Update.Pull<Member>(t => t.Members, member);
            await collection.UpdateOneAsync(idFilter, removeMemberDefinition);
            return team;
        }
    }
}