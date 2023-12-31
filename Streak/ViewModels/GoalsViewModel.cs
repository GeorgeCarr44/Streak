using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Streak.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streak.ViewModels
{
    public partial class GoalsViewModel : ObservableObject
    {
        IConnectivity connectivity;
        public GoalsViewModel(IConnectivity connectivity)
        {
            //Get Data
            Goals = new ObservableCollection<Goal>();
            this.connectivity = connectivity;
        }

        [ObservableProperty]
        ObservableCollection<Goal> goals;

        [RelayCommand]
        void Tap(string s)
        {

        }

    }
}
