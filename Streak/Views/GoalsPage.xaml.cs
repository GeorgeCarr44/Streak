using Streak.Data;
using Streak.Models;
using System.Collections.ObjectModel;

namespace Streak
{
    public partial class GoalsPage : ContentPage
    {

        GoalsDatabase database;
        public ObservableCollection<Goal> Goals { get; set; } = new()
        {
            new Goal(){ID = 1, Name = "Test"},
            new Goal(){ID = 2, Name = "Test2"},
            new Goal(){ID = 3, Name = "Test3"} 
        };

        public GoalsPage(GoalsDatabase goalsDatabase)
        {
            InitializeComponent();
            database = goalsDatabase;
            BindingContext = this;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            var items = await database.GetGoalsAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                //Goals.Clear();
                //foreach (var item in items)
                //    Goals.Add(item);

            });
        }
    }

}
