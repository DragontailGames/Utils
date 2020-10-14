using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Dragontailgames.Utils
{

    public class DatabaseAccess<T>
    {
        MongoClient client = null;
        IMongoDatabase database;
        IMongoCollection<T> collection;

        public DatabaseAccess(string clientName, string databaseName, string collectionName)
        {
            client = new MongoClient(clientName);

            database = client.GetDatabase(databaseName);
            collection = database.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Save document in Database
        /// </summary>
        /// <param name="content">document</param>
        public async Task SaveDocumentToDatabaseAsync(T content)
        {
            await collection.InsertOneAsync(content);

            Debug.Log("Save user");

            return;
        }

        /// <summary>
        /// Get document from database
        /// </summary>
        /// <returns>List of documents</returns>
        public async Task<List<T>> GetAsyncDocumentsFromDatabase()
        {
            var allDocTask = collection.FindAsync(new BsonDocument());

            var docAwaited = await allDocTask;

            List<T> docs = new List<T>();

            foreach (var p in docAwaited.ToList())
            {
                docs.Add(p);
            }

            Debug.Log("Get all");

            return docs;
        }

        /// <summary>
        /// Get document from database
        /// </summary>
        /// <returns>List of documents</returns>
        public async Task<IAsyncCursor<T>> GetAsyncDocumentFromDatabase(string varName, string id)
        {
            var filterBuilder = Builders<T>.Filter.Eq(varName, id);

            var x = await collection.FindAsync(filterBuilder);

            return x;
        }

        /// <summary>
        /// Delete a document in Database
        /// </summary>
        /// <param name="varName">Variable name</param>
        /// <param name="id">Id for document</param>
        /// <returns></returns>
        public async Task DeleteDocumentAsync(string varName, string id)
        {
            var deleteFilter = Builders<T>.Filter.Eq(varName, id);

            await collection.DeleteOneAsync(deleteFilter);

            Debug.Log("Delete as Id: " + id);
        }

        /// <summary>
        /// Replace a document in Database
        /// </summary>
        /// <param name="varName">Variable name</param>
        /// <param name="id">Id for document</param>
        /// <param name="content">Content of class</param>
        /// <returns></returns>
        public async Task ReplaceDocumentAsync(string varName, string id, T content)
        {
            var filterBuilder = Builders<T>.Filter.Eq(varName, id);

            await collection.ReplaceOneAsync(
                filterBuilder,
                content
            );

            Debug.Log("Replace as Id: " + id);
        }

        /// <summary>
        /// Update document valuer
        /// </summary>
        /// <param name="varName">Variable name</param>
        /// <param name="id">Id for document</param>
        /// <param name="replaceVarName">Variable name as replaced</param>
        /// <param name="replaceValue">Value as replaced</param>
        /// <returns></returns>
        public async Task UpdateDocumentAsync(string varName, string id, string replaceVarName, string replaceValue)
        {
            var filterBuilder = Builders<T>.Filter.Eq(varName, id);
            var update = Builders<T>.Update.Set(replaceVarName, replaceValue);

            await collection.UpdateOneAsync(filterBuilder, update);

            Debug.Log("Update as Id: " + id);
        }
    }
}
