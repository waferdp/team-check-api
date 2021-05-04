using System.Linq;
using DomainModel;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Threading.Tasks;
using Repository.Interface;

namespace Repository.Repository
{
    public class TeamCheckAnswerRepository : ITeamCheckAnswerRepository
    {
        private MongoClient _client;
        private IMongoDatabase _database;

        public TeamCheckAnswerRepository(IConfiguration configuration) 
        {
            var dbConnectionString = configuration["ConnectionStrings:server"];
            var databaseName = configuration["ConnectionStrings:database"];
            _client = new MongoClient(dbConnectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public IQueryable<TeamCheckAnswer> GetAll()
        {
            var documents = _database.GetCollection<TeamCheckAnswer>("TeamAnswers").AsQueryable();
            return documents;
        }

        public async Task SaveAnswer(TeamCheckAnswer answer)
        {
            var collection = _database.GetCollection<TeamCheckAnswer>("TeamAnswers");
            await collection.InsertOneAsync(answer);
        }
    }
}
