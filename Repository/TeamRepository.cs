using System;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Repository.Interface;

namespace Repository
{
    public class TeamRepository : SimpleRepository<Team>, ITeamRepository
    {
        private ILogger<TeamRepository> _logger;
        private IFeatureManager _featureManager; 
        private FilterDefinition<Team> _getFilter;
        private bool _softDelete;

        public TeamRepository(IConfiguration configuration, ILogger<TeamRepository> logger, IFeatureManager featureManager)
        : base(configuration, logger)
        {
            _logger = logger;
            _featureManager = featureManager;
            var task = featureManager.IsEnabledAsync(FeatureFlags.SoftDelete);
            task.Wait();
            _softDelete = task.Result; 
            _getFilter = CreateNotDeletedFilter(_softDelete);
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

        public override IQueryable<Team> GetAll()
        {
            _logger.LogInformation($"Retrieving all Team:s from MongoDB ({_database.DatabaseNamespace})");
            try
            {
                var documents = GetCollection().AsQueryable().Where(team => _getFilter.Inject());
                return documents;
            }
            catch (MongoException ex)
            {
                _logger.LogError($"Error retrieving all Team:s {ex.Message}", ex);
                throw;
            }
        }

        public override async Task DeleteAsync(Guid id)
        {
            if (_softDelete)
            {
                _logger.LogInformation($"Soft deleting team in MongoDB ({_database.DatabaseNamespace})");
                try
                {
                    var team = await GetAsync(id);
                    team.IsDeleted = true;
                    await SaveAsync(team);
                }
                catch (MongoException ex)
                {
                    _logger.LogError($"Error (soft) deleting team with Id {id}: {ex.Message}", ex);
                    throw;
                }
            }
            else
            {
                await base.DeleteAsync(id);
            }

        }

        private async Task SoftDelete(Guid id)
        {
            var team = await GetAsync(id);
            team.IsDeleted = true;
            await SaveAsync(team);
        }

        private FilterDefinition<Team> CreateNotDeletedFilter(bool softDelete)
        {
            if (softDelete)
            {
                return Builders<Team>.Filter.Eq(team => team.IsDeleted, false);
            }
            return Builders<Team>.Filter.Empty;
        }
    }
}