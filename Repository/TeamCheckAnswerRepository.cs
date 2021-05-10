using System.Linq;
using DomainModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Threading.Tasks;
using Repository.Interface;
using System;

namespace Repository
{
    public class TeamCheckAnswerRepository : ITeamCheckAnswerRepository
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        private ILogger<TeamCheckAnswerRepository> _logger;

        public TeamCheckAnswerRepository(IConfiguration configuration, ILogger<TeamCheckAnswerRepository> logger) 
        {
            _logger = logger;
            var dbConnectionString = configuration["ConnectionStrings:server"];
            var databaseName = configuration["ConnectionStrings:database"];
            _client = new MongoClient(dbConnectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public IQueryable<TeamCheckAnswer> GetAll()
        {
            _logger.LogInformation($"Retrieving all TeamCheckAnswers from MongoDB ({_database.DatabaseNamespace})");
            try
            {
                var documents = _database.GetCollection<TeamCheckAnswer>("TeamAnswers").AsQueryable();
                return documents;
            } catch(MongoException ex)
            {
                _logger.LogError($"Error retrieving TeamCheckAnswers {ex.Message}", ex);
                throw;
            }
        }

        public async Task<TeamCheckAnswer> SaveAnswer(TeamCheckAnswer answer)
        {
            try
            {
                if (answer.Id == Guid.Empty)
                {
                    answer.Id = Guid.NewGuid();
                    answer.Created = DateTime.Now;
                }
                var collection = _database.GetCollection<TeamCheckAnswer>("TeamAnswers");
                await collection.InsertOneAsync(answer);
                return answer;
            } catch(MongoException ex)
            {
                _logger.LogError($"Error saving TeamCheckAnswer {ex.Message}", ex);
                throw;
            }
        }
    }
}
