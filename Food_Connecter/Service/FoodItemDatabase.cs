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
            database.CreateTableAsync<ClassData>().Wait();
        }

        public Task<List<ClassData>> GetItemsAsync()
        {
            return database.Table<ClassData>().ToListAsync();
        }

        //public Task<List<ClassData.classes>> GetItemsNotDoneAsync()
        //{
        //    return database.QueryAsync<ClassData.classes>("SELECT * FROM [ClassData] WHERE [Done] = 0");
        //}

        public Task<ClassData> GetItemAsync(int id)
        {
            return database.Table<ClassData>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(ClassData item)
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

        public Task<int> DeleteItemAsync(ClassData item)
        {
            return database.DeleteAsync(item);
        }
    }
}
