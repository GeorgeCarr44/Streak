using CommunityToolkit.Mvvm.ComponentModel;
using Streak.Data;
using Streak.Models;
using Streak.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using System.Xml;

namespace Streak
{
    public partial class GoalsPage : ContentPage
    {

        GoalsDatabase database;
        public ObservableCollection<Goal> Goals { get; set; } = new();
        
        //Private fields to handle special interation.
        private System.Timers.Timer _timer;
        private Stopwatch stopWatch;
        private Goal _currentSelectedGoal;
        public GoalsPage(GoalsDatabase goalsDatabase)
        {
            InitializeComponent();
            database = goalsDatabase;
            BindingContext = this;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            await RefreshGoals();
        }

        private async Task RefreshGoals()
        {

            var goals = await database.GetGoalsAsync();

            //foreach (var goal in goals)
            //{
            //    goal.Checked = true;
            //}

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Goals.Clear();
                foreach (var g in goals)
                {
                    //Just for test data uncheck each
                    if (GoalDisplays(g))
                    {
                        Goals.Add(g);
                    }
                }

                //This is the new goal button
                Goals.Add(new Goal() { ID = 0, Name = "New Goal" });
            });
        }

        private bool GoalDisplays(Goal g)
        {
            switch ((GoalFrequency)g.SelectedFrequencyID)
            {
                case GoalFrequency.EveryOtherDay:
                    //(DateTime.Now - g.CreationDate).TotalDays
                    //This gets the age of the goal in days
                    //does a mod to determine if its an even or of number of days
                    //if it is even then it shows
                    return ((DateTime.Now - g.CreationDate).Days % 2) == 0;
                case GoalFrequency.SelectDayOfWeek:
                    DayOfWeek dow = DateTime.Now.DayOfWeek;
                    if (dow == DayOfWeek.Monday && g.Monday)
                        return true;
                    if(dow == DayOfWeek.Tuesday && g.Tuesday)
                        return true;
                    if(dow == DayOfWeek.Wednesday && g.Wednesday)
                        return true; 
                    if (dow == DayOfWeek.Thursday && g.Thursday)
                        return true;
                    if(dow == DayOfWeek.Friday && g.Friday)
                        return true;
                    if(dow == DayOfWeek.Saturday && g.Saturday)
                        return true; 
                    if(dow == DayOfWeek.Sunday && g.Sunday)
                        return true; 

                    return false;
                case GoalFrequency.EveryDay: 
                default:
                    return true;
            }
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(EditGoalPage), true, new Dictionary<string, object>
            {
                ["Goal"] = new Goal()
            });
        }

        async void CheckAllGoals(object sender, EventArgs e)
        { 
            // Get the goal
            foreach(Goal g in Goals)
            {
                g.Checked = true;
                await database.SaveGoalAsync(g);
            }
        }


        async void OnItemPressed(object sender, EventArgs e)
        {
            // Get the goal
            var border = (Button)sender;
            _currentSelectedGoal = (Goal)border.BindingContext;

            // if there is not goal dont start the timer return an await release.
            // Need to test how slide/drag click works here?
            if(_currentSelectedGoal.ID == 0) return;
            //Timer to trigger Held Event
            _timer = new System.Timers.Timer();
            _timer.Elapsed += new ElapsedEventHandler(OnHeldEvent);
            _timer.Interval = 800;
            //Stopwatch to see if Held Event would have triggered then ignore the Release
            stopWatch = new Stopwatch();
            _timer.Enabled = true;
            stopWatch.Start();

            //(sender as Button).Text = "You pressed me!";
        }

        // Specify what you want to happen when the Elapsed event is raised.
        async void OnHeldEvent(object source, ElapsedEventArgs e)
        {
            //Dispose of our current running timer
            _timer.Stop();
            //one a goal is done dont allow it to be checked again until tomorrow
            if (!_currentSelectedGoal.Checked)
            {
                //toggle the check
                await CompleteGoal(_currentSelectedGoal);
            }
        }

        private async Task CompleteGoal(Goal goal)
        {
            var completionsAddedCount = database.CreateCompletionAsync(goal);
            //Create a new 
            await completionsAddedCount;
            await RefreshGoals();
        }

        async void OnItemReleased(object sender, EventArgs e)
        {
            stopWatch.Stop();
            _timer.Stop();
            //if its under the hold time otherwise this is handled by holding
            if(stopWatch.Elapsed.TotalMilliseconds < 800)
            {
                //Dispose of our current running timer
                var border = (Button)sender;
                var Goal = (Goal)border.BindingContext;

                if (Goal.ID == 0)
                    return;

                await Shell.Current.GoToAsync(nameof(EditGoalPage), true, new Dictionary<string, object>
                {
                    ["Goal"] = Goal
                });
            }

        }
    }
}
