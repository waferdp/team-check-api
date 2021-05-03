using System;
using System.Linq;
using System.Collections.Generic;
using DomainModel;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Repository
{
    public class TeamCheckAnswerRepository
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        private string databaseName;

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
