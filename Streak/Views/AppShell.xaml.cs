using Streak.Views;

namespace Streak
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditGoalPage), typeof(EditGoalPage));
        }
    }
}
