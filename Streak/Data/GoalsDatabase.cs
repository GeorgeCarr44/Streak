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
            await Database.CreateTableAsync<Goal>();
            await Database.CreateTableAsync<Completion>();
        }

        #region Completion
        /// <summary>
        /// Gets all of the completions
        /// this might be overkill and could be slimmed down to just a count
        /// </summary>
        /// <returns></returns>
        public async Task<List<Completion>> GetCompletionsAsync()
        {
            //Make sure datatable is set up
            await Init();
            //Get all 
            return await Database.Table<Completion>().ToListAsync();
        }

        /// <summary>
        /// Gets all of the completions for a goal
        /// </summary>
        /// <param name="goal">Goal to get completions for</param>
        /// <returns></returns>
        public async Task<List<Completion>> GetCompletionsAsync(Goal goal)
        {
            //Make sure datatable is set up
            await Init();
            //Get all that relate to the passed in goal
            return await Database.Table<Completion>().Where(x => x.GoalID == goal.ID).ToListAsync();
            // SQL queries are also possible
            //return await Database.QueryAsync<Goal>($"SELECT * FROM [Goal] WHERE [GoalID] = {goal.ID}");
        }

        public async Task<int> CreateCompletionAsync(Goal goal)
        {
            await Init();
            var completion = new Completion(goal);
            return await Database.InsertAsync(completion);
        }

        private async Task<int> GetTodaysCompletionAsync(Goal goal)
        {
            return await Database.Table<Completion>().CountAsync();
        }

        public async Task<int> SaveCompletionAsync(Completion completion)
        {
            await Init();
            if (completion.ID != 0)
            {
                return await Database.UpdateAsync(completion);
            }
            else
            {
                return await Database.InsertAsync(completion);
            }
        }

        public async Task<int> DeleteCompetionAsync(Completion completion)
        {
            await Init();
            return await Database.DeleteAsync(completion);
        }

        #endregion
        #region Goals
        public async Task<List<Goal>> GetGoalsAsync()
        {
            await Init();


            var goalscount = await Database.Table<Goal>().CountAsync();
            var completionscount = await Database.Table<Completion>().CountAsync();
            //Update Goals Current Checked
            var goals = await Database.Table<Goal>().ToListAsync();
            var completions = await Database.Table<Completion>().ToListAsync();

            foreach (var goal in goals) {
                var tets = await Database.Table<Completion>().Where(x => x.GoalID == goal.ID).ToListAsync();
                if(tets.Any())
                {
                    bool b = tets.First().CreationDate == DateTime.Now.Date;
                }
                
                var thisC = await Database.Table<Completion>().CountAsync(x => x.GoalID == goal.ID && x.CreationDate == DateTime.Now.AddDays(-1));


                //goal.Checked = GetTodaysCompletionAsync(goal).Result > 0;
            }

            return goals;
        }



        public async Task<int> SaveGoalAsync(Goal goal)
        {
            await Init();
            if (goal.ID != 0)
            {
                return await Database.UpdateAsync(goal);
            }
            else
            {
                goal.CreationDate = DateTime.Now;
                return await Database.InsertAsync(goal);
            }
        }

        public async Task<int> DeleteGoalAsync(Goal goal)
        {
            await Init();
            return await Database.DeleteAsync(goal);
        }
        #endregion
    }
}
