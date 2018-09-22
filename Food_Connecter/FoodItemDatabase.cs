using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace Food_Connecter
{
    public class FoodItemDatabase
    {
        readonly SQLiteAsyncConnection database;

        public FoodItemDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<FoodItem>().Wait();
        }

        public Task<List<FoodItem>> GetItemsAsync()
        {
            return database.Table<FoodItem>().ToListAsync();
        }

        public Task<List<FoodItem>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<FoodItem>("SELECT * FROM [FoodItem] WHERE [Done] = 0");
        }

        public Task<FoodItem> GetItemAsync(int id)
        {
            return database.Table<FoodItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(FoodItem item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(FoodItem item)
        {
            return database.DeleteAsync(item);
        }
    }
}
