﻿using CommunityToolkit.Mvvm.ComponentModel;
using Streak.Data;
using Streak.Models;
using Streak.Views;
using System.Collections.ObjectModel;
using System.Timers;

namespace Streak
{
    public partial class GoalsPage : ContentPage
    {

        GoalsDatabase database;
        public ObservableCollection<Goal> Goals { get; set; } = new();
        public string isCurrentlyPressed;


        private string _lblText;
        public string LblText
        {
            get
            {
                return _lblText;
            }
            set
            {
                _lblText = value;
                OnPropertyChanged();
            }
        }

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

                //This is the new goal button
                Goals.Add(new Goal() { ID = 0, Name = "New Goal" });
            });
        }

        async void OnItemAdded(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(EditGoalPage), true, new Dictionary<string, object>
            {
                ["Goal"] = new Goal()
            });
        }

        async void OnItemClicked(object sender, EventArgs e)
        {
            var border = (Button)sender;
            var Goal = (Goal)border.BindingContext;

            if (Goal.ID == 0)
                return;

            await Shell.Current.GoToAsync(nameof(EditGoalPage), true, new Dictionary<string, object>
            {
                ["Goal"] = Goal
            });
        }


        async void OnItemPressed(object sender, EventArgs e)
        {
            LblText = "true";

            ////Hold time
            //System.Timers.Timer aTimer = new System.Timers.Timer();
            //aTimer.Elapsed += new ElapsedEventHandler(OnHeldEvent);
            //aTimer.Interval = 5000;
            //aTimer.Enabled = true;

            //Console.WriteLine("Press \'q\' to quit the sample.");
            //while (Console.Read() != 'q') ;
            
            (sender as Button).Text = "You pressed me!";
        }

        // Specify what you want to happen when the Elapsed event is raised.
        private static void OnHeldEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Hello World!");
        }

        async void OnItemReleased(object sender, EventArgs e)
        {
            LblText = "false";
            (sender as Button).Text = "You released me!";
        }
    }
}
