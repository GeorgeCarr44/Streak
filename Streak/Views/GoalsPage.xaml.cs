using Streak.ViewModels;

namespace Streak
{
    public partial class GoalsPage : ContentPage
    {


        public GoalsPage(GoalsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }


    }

}
