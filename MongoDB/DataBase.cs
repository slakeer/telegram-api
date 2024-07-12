using System.Security.Cryptography.X509Certificates;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApplication1.MongoDB;

public class DataBase
{
    public static IMongoCollection<BsonDocument> Collection { get; private set; }

    public DataBase()
    {
        const string connectionUri = "mongodb+srv://mail:pass@domen.id.mongodb.net/?retryWrites=true&w=majority";
        const string databaseName = "test";
        const string collectionName = "messages";
        var settings = MongoClientSettings.FromConnectionString(connectionUri);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        var client = new MongoClient(settings);
        try
        {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }   
        IMongoDatabase database = client.GetDatabase(databaseName);
        Collection = database.GetCollection<BsonDocument>(collectionName);
    }
    public void Main(double chatId, out string apiKey, out string secretKey)
    {
        apiKey = string.Empty;
        secretKey = string.Empty;

        var filter = Builders<BsonDocument>.Filter.Eq("chatId", chatId);
        var documents = Collection.Find(filter).FirstOrDefault();
        if (documents != null && documents.Contains("apiKey") && documents.Contains("secretKey"))
        {
            apiKey = documents["apiKey"].AsString;
            secretKey = documents["secretKey"].AsString;
        }
    }

    public async Task<string> DeleteUserAsync(double chatId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("chatId", chatId);
        var documents = await Collection.Find(filter).ToListAsync();

        if (documents.Count > 0)
        {
            var result = await Collection.DeleteOneAsync(filter);
            return "User data deleted successfully.";
        }
        else
        {
            return "User not found in the database.";
        }
    }

}   
