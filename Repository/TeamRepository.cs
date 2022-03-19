using System;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Repository.Interface;

namespace Repository
{
    public class TeamRepository : SimpleRepository<Team>
    {
        private ILogger<TeamRepository> _logger;
        private FilterDefinition<Team> _getFilter;
        private bool _softDelete;

        public TeamRepository(IConfiguration configuration, ILogger<TeamRepository> logger)
        : base(configuration, logger)
        {
            _logger = logger;
            _softDelete = bool.Parse(configuration["Feature:SoftDelete"]);
            _getFilter = CreateNotDeletedFilter(_softDelete);
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