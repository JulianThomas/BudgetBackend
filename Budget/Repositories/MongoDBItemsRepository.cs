using Budget.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Budget.Repositories
{
    public class MongoDBItemsRepository : IUsersRepository
    {
        private const string databaseName = "BudgetDB";
        private const string collectionName = "BudgetUsers";
        private readonly IMongoCollection<User> userCollection;
        private readonly FilterDefinitionBuilder<User> filterBuilder = Builders<User>.Filter;

        public MongoDBItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            userCollection = database.GetCollection<User>(collectionName);
        }

        public async Task CreateUserAsync(User user)
        {
            await userCollection.InsertOneAsync(user);
        }
        public async Task<User> GetUserAsync(Guid id)
        {
            var filter = filterBuilder.Eq(user => user.Id, id);
            return await userCollection.Find(filter).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await userCollection.Find(new BsonDocument()).ToListAsync();
        }


        public async Task CreateBudgetItemAsync(Guid userId, BudgetItem item)
        {
            var filter = filterBuilder.Where(user => user.Id == userId);
            var insert = Builders<User>.Update.Push(user => user.BudgetItems, item);
            await userCollection.UpdateOneAsync(filter, insert);
            //filter = filterBuilder.Eq(user => user.Id, userId)
            //    & filterBuilder.ElemMatch(user => user.BudgetItems,
            //    Builders<BudgetItem>.Filter.Eq(item => item.ItemId, item.ItemId));
            //return await GetBudgetItemAsync(userId, item.ItemId);
        }
        public async Task<BudgetItem> GetBudgetItemAsync(Guid userId, Guid itemId)
        {
            var filterUser = filterBuilder.Eq(user => user.Id, userId);
            var user = await userCollection.Find(filterUser).SingleOrDefaultAsync();
            if (user == null)
                return null;
            return user.BudgetItems.Find(item => item.ItemId == itemId);
        }
        public async Task UpdateBudgetItemAsync(Guid userId, BudgetItem item)
        {

            var filter = filterBuilder.Eq(user => user.Id, userId) 
                & filterBuilder.ElemMatch(user => user.BudgetItems, 
                Builders<BudgetItem>.Filter.Eq(item=> item.ItemId, item.ItemId));
            var update = Builders<User>.Update.Set(user => user.BudgetItems[-1], item);
            await userCollection.UpdateOneAsync(filter, update);
        }
        public async Task DeleteBudgetItemAsync(Guid userId, Guid itemId)
        {
            var filter = filterBuilder.Eq(user => user.Id, userId);
            var delete = Builders<User>.Update.PullFilter(user => user.BudgetItems, 
                Builders<BudgetItem>.Filter.Eq(item => item.ItemId,itemId) );
            await userCollection.UpdateOneAsync(filter, delete);
        }

    }
}
