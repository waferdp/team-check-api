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
    public class SimpleRepository<T> : IRepository<T> where T : Entity
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        private ILogger<SimpleRepository<T>> _logger;

        public SimpleRepository(IConfiguration configuration, ILogger<SimpleRepository<T>> logger) 
        {
            _logger = logger;
            var dbConnectionString = configuration["ConnectionStrings:server"];
            var databaseName = configuration["ConnectionStrings:database"];
            _client = new MongoClient(dbConnectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public T Get(Guid id)
        {
            _logger.LogInformation($"Retrieving specific { GetTypeName() } from MongoDB ({_database.DatabaseNamespace})");
            try
            {
                var documents = _database.GetCollection<T>(GetCollectionName()).AsQueryable();
                var result = documents.FirstOrDefault(entity => entity.Id == id);
                return result;
            }
            catch(MongoException ex)
            {
                _logger.LogError($"Error retrieving entity with Id {id}: {ex.Message}", ex);
                throw;
            }
        }

        public IQueryable<T> GetAll()
        {
            _logger.LogInformation($"Retrieving all { GetTypeName() } from MongoDB ({_database.DatabaseNamespace})");
            try
            {
                var documents = _database.GetCollection<T>(GetCollectionName()).AsQueryable();
                return documents;
            } 
            catch(MongoException ex)
            {
                _logger.LogError($"Error retrieving all {GetTypeName()}:s {ex.Message}", ex);
                throw;
            }
        }

        public async Task<T> SaveAsync(T entity)
        {
            _logger.LogInformation($"Saving new { GetTypeName() } in MongoDB ({_database.DatabaseNamespace})");
            try
            {
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                    entity.Created = DateTime.Now;
                }
                var collection = _database.GetCollection<T>(GetCollectionName());
                await collection.InsertOneAsync(entity);
                return entity;
            }
            catch(MongoException ex)
            {
                _logger.LogError($"Error saving {GetTypeName()} {ex.Message}", ex);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation($"Deleting {GetTypeName()} in MongoDB ({_database.DatabaseNamespace})");
            try
            {
                var collection = _database.GetCollection<T>(GetCollectionName());
                var idFilter = Builders<T>.Filter.Eq("_id", id);
                await collection.DeleteOneAsync(idFilter);
            } catch (MongoException ex)
            {
                _logger.LogError($"Error deleting entity with Id {id}: {ex.Message}", ex);
                throw;
            }
        }

        private string GetCollectionName()
        {
            return GetTypeName() + "s";
        }

        private string GetTypeName()
        {
            return typeof(T).GetType().Name;     
        }
    }
}
