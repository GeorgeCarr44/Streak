using Streak.Data;
using Streak.Models;

namespace Streak.Views;

[QueryProperty("Goal", "Goal")]
public partial class EditGoalPage : ContentPage
{
    Goal goal;
    public Goal Goal
    {
        get => BindingContext as Goal;
        set => BindingContext = value;
    }
    GoalsDatabase database;
    public EditGoalPage(GoalsDatabase goalDatabase)
    {
        InitializeComponent();
        database = goalDatabase;
    }

    protected override async void OnAppearing()
    {
        deleteButton.IsVisible = Goal.ID != 0;
        RefreshDaysOfTheWeek();
        base.OnAppearing();
    }

    private void RefreshDaysOfTheWeek()
    {
        var gf = (GoalFrequency)Goal.SelectedFrequencyID;
        switch (gf)
        {
            case GoalFrequency.EveryDay:
            case GoalFrequency.EveryOtherDay:
                // Hide Buttons
                DaysOfTheWeek.IsVisible = false;
                break;
            case GoalFrequency.SelectDayOfWeek:
                // Show Buttons
                DaysOfTheWeek.IsVisible = true;
                break;
        }
    }

    void OnFrequencyChange(object sender, EventArgs e)
    {
        RefreshDaysOfTheWeek();
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Goal.Name))
        {
            await DisplayAlert("Name Required", "Please enter a name for the Goal.", "OK");
            return;
        }

        Goal.Name = Goal.Name.Trim();

        await database.SaveGoalAsync(Goal);
        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (Goal.ID == 0)
            return;
        await database.DeleteGoalAsync(Goal);
        await Shell.Current.GoToAsync("..");
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}