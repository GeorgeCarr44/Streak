﻿using Streak.Data;
using Streak.Models;
using Streak.Views;
using System.Collections.ObjectModel;

namespace Streak
{
    public partial class GoalsPage : ContentPage
    {

        GoalsDatabase database;
        public ObservableCollection<Goal> Goals { get; set; } = new();
        

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
                Goals.Clear();
                foreach (var item in items)
                    Goals.Add(item);
            });
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(EditGoalPage), true, new Dictionary<string, object>
            {
                ["Goal"] = new Goal()
            });
        }
    }

}
