using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess
{
    public class AuthRepo
    {
        private readonly string connectionString;
        private readonly string databaseName;
        private readonly string collectionName;

        public AuthRepo(string ConnectionString, string DatabaseName, string CollectionName) {
            connectionString = ConnectionString;
            databaseName = DatabaseName;
            collectionName = CollectionName;
        }

        public void CreateAuthentication(AuthentincationInfo authInfo)
        {
            var authCollection = GetCollection();

            // add or update
            authCollection.ReplaceOne(
                filter: new BsonDocument("UserId", authInfo.UserId),
                options: new UpdateOptions { IsUpsert = true },
                replacement: authInfo);
        }

        public AuthentincationInfo GetAuthentication(string email)
        {
            var authCollection = GetCollection();

            // add or update
            return authCollection.Find(d => d.Email == email).SingleOrDefault();
        }

        private IMongoCollection<AuthentincationInfo> GetCollection()
        {
            MongoClient client = new MongoClient(connectionString);

            var database = client.GetDatabase(databaseName);
            var authCollection = database.GetCollection<AuthentincationInfo>(collectionName);
            return authCollection;
        }
    }
}
