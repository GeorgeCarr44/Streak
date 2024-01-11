using SQLite;
using Streak.Models;

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

            var rowsAdded = await Database.InsertAsync(completion);

            UpdateGoalsCurrentStreak(goal);

            return rowsAdded;

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
            var goals = await Database.Table<Goal>().OrderByDescending(x => x.CurrentStreak).ToListAsync();
            //foreach (var goal in goals)
            //{
                //UpdateGoalsCheckedValue(goal);
                //UpdateGoalsCurrentStreak(goal);
            //}

            return goals;
        }

        private async void UpdateGoalsCurrentStreak(Goal goal)
        {
            // Get all completions for the goal
            var completions = await Database.Table<Completion>().Where(x => x.GoalID == goal.ID).OrderByDescending(x => x.CreationDate).ToListAsync();
            //This is for when the goals can be completed more than once
            int goalDailyCompletionsRequired = 1;
            var lower = DateTime.Today;
            var upper = DateTime.Today.AddDays(1);

            //set todays check
            goal.Checked = completions.Where(x => x.CreationDate > lower && x.CreationDate < upper).Count() >= goalDailyCompletionsRequired;
            int currentStreak = goal.Checked ? 1 : 0;


            //then check the previous day to see if were currently working on a streak
            lower = lower.AddDays(-1);
            upper = upper.AddDays(-1);
            int testCount = completions.Where(x => x.CreationDate > lower && x.CreationDate < upper).Count();

            // or first?
            while (testCount >= goalDailyCompletionsRequired)
            {
                //update the current streak
                currentStreak++;
                //then check the next day
                lower = lower.AddDays(-1);
                upper = upper.AddDays(-1);
                testCount = completions.Where(x => x.CreationDate > lower && x.CreationDate < upper).Count();
            }
            //set the current streak
            goal.CurrentStreak = currentStreak;

            // Update the longest streak if your on that run
            if (goal.CurrentStreak > goal.LongestStreak)
                goal.LongestStreak = goal.CurrentStreak;

            await SaveGoalAsync(goal);

        }

        private async Task<int> GetCompletionCountBetweenDates(int goalID, DateTime lowerBound, DateTime upperBound)
        {
            return await Database.Table<Completion>().CountAsync(x => x.GoalID == goalID && x.CreationDate > lowerBound && x.CreationDate < upperBound);
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
