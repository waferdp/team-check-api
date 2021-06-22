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
        protected IMongoDatabase _database;
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
                var documents = GetCollection();
                var result = documents.Find(CreateIdFilter(id)).FirstOrDefault();
                return result;
            }
            catch(MongoException ex)
            {
                _logger.LogError($"Error retrieving entity with Id {id}: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<T> GetAsync(Guid id)
        {
            _logger.LogInformation($"Retrieving specific { GetTypeName() } from MongoDB ({_database.DatabaseNamespace})");
            try
            {
                var documents = GetCollection();
                var result = await (await documents.FindAsync<T>(CreateIdFilter(id))).FirstOrDefaultAsync();
                return result;
            }
            catch(MongoException ex)
            {
                _logger.LogError($"Error retrieving entity with Id {id}: {ex.Message}", ex);
                throw;
            }
        }

        public virtual IQueryable<T> GetAll()
        {
            _logger.LogInformation($"Retrieving all { GetTypeName() } from MongoDB ({_database.DatabaseNamespace})");
            try
            {
                var documents = GetCollection().AsQueryable();
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
                var idFilter = CreateIdFilter(entity.Id);
                var collection = GetCollection();
                if(collection.Find(idFilter).Any())
                {
                    await collection.ReplaceOneAsync(CreateIdFilter(entity.Id), entity);
                }
                else
                {
                    await collection.InsertOneAsync(entity);
                }
                return entity;
            }
            catch(MongoException ex)
            {
                _logger.LogError($"Error saving {GetTypeName()} {ex.Message}", ex);
                throw;
            }
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation($"Deleting {GetTypeName()} in MongoDB ({_database.DatabaseNamespace})");
            try
            {
                var collection = GetCollection();
                await collection.DeleteOneAsync(CreateIdFilter(id));
            } catch (MongoException ex)
            {
                _logger.LogError($"Error deleting entity with Id {id}: {ex.Message}", ex);
                throw;
            }
        }

        protected IMongoCollection<T> GetCollection()
        {
            return _database.GetCollection<T>(GetCollectionName());
        }

        protected FilterDefinition<T> CreateIdFilter(Guid id)
        {
            return Builders<T>.Filter.Eq("_id", id);
        }

        private string GetCollectionName()
        {
            return GetTypeName() + "s";
        }

        private string GetTypeName()
        {
            return typeof(T).Name;     
        }
    }
}
