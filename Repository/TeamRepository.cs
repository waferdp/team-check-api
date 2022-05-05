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
    public class TeamRepository : SimpleRepository<Team>
    {
        private ILogger<TeamRepository> _logger;
        private IFeatureManager _featureManager; 

        public TeamRepository(IConfiguration configuration, ILogger<TeamRepository> logger)
        : base(configuration, logger)
        {
            _logger = logger;
        }

        public override async Task<IQueryable<Team>> GetAllAsync()
        {
            _logger.LogInformation($"Retrieving all Team:s from MongoDB ({_database.DatabaseNamespace})");
            try
            {
                var getFilter = CreateNotDeletedFilter();
                var documents = GetCollection().AsQueryable().Where(team => getFilter.Inject());
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
            await this.SoftDeleteAsync(id);
        }

        private async Task SoftDeleteAsync(Guid id)
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

        private FilterDefinition<Team> CreateNotDeletedFilter()
        {
            return Builders<Team>.Filter.Eq(team => team.IsDeleted, false);
        }
    }
}