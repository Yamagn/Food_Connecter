using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
namespace Food_Connecter
{
    public class UketoriItemDateBase
    {
        readonly SQLiteAsyncConnection database;

        public UketoriItemDateBase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<osusowakeFood>().Wait();
        }

        public Task<List<osusowakeFood>> GetItemsAsync()
        {
            return database.Table<osusowakeFood>().ToListAsync();
        }

        //public Task<List<ClassData.classes>> GetItemsNotDoneAsync()
        //{
        //    return database.QueryAsync<ClassData.classes>("SELECT * FROM [ClassData] WHERE [Done] = 0");
        //}

        public Task<osusowakeFood> GetItemAsync(int id)
        {
            return database.Table<osusowakeFood>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(osusowakeFood item)
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

        public Task<int> DeleteItemAsync(osusowakeFood item)
        {
            return database.DeleteAsync(item);
        }
    }
}
