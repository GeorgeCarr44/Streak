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
            var goals = await Database.Table<Goal>().ToListAsync();
            foreach (var goal in goals) 
                UpdateGoalsCheckedValue(goal);
            return goals;
        }


        /// <summary>
        /// This method refreshed the checked value on the goals
        /// it does this by walking all goals
        /// checking the completion table for each goal
        /// evealuating if a completion has been entered for that goal today
        /// it then saves the new value on the checked value on the goal in the database
        /// this allows it to be data bound to the buttons in the ui
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private async void UpdateGoalsCheckedValue(Goal goal)
        {

            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);
            var test = await Database.Table<Completion>().ToListAsync();

            // Having to do it this way because the database field doesnt allow
            var todaysCheckCount = await Database.Table<Completion>().CountAsync(x => x.GoalID == goal.ID && x.CreationDate > today && x.CreationDate < tomorrow);

            // if goal checked has changed
            if (goal.Checked != todaysCheckCount > 0)
            {
                // update the goals checked
                // look at batching this in future
                goal.Checked = todaysCheckCount > 0;
                // Save the goal
                await SaveGoalAsync(goal);
            }
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
