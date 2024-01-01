using SQLite;
using Streak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streak.Data
{
    public class GoalsDatabase
    {
        SQLiteAsyncConnection Database;

        public GoalsDatabase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<Goal>();
        }

        public async Task<List<Goal>> GetGoalsAsync()
        {
            await Init();
            return await Database.Table<Goal>().ToListAsync();
        }

        ///// <summary>
        ///// This is from the example
        ///// This is not needed but keeping it in for future example
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<Goal>> GetItemsNotDoneAsync()
        //{
        //    await Init();
        //    return await Database.Table<Goal>().Where(t => t.Done).ToListAsync();

        //    // SQL queries are also possible
        //    //return await Database.QueryAsync<Goal>("SELECT * FROM [Goal] WHERE [Done] = 0");
        //}

        public async Task<int> SaveItemAsync(Goal item)
        {
            await Init();
            if (item.ID != 0)
            {
                return await Database.UpdateAsync(item);
            }
            else
            {
                return await Database.InsertAsync(item);
            }
        }

        public async Task<int> DeleteItemAsync(Goal item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
