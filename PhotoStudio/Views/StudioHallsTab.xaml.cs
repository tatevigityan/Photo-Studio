using System.Windows.Controls;
using PhotoStudio.ViewModels;

namespace PhotoStudio
{
    public partial class StudioHallsTab : UserControl
    {
        public StudioHallsTab(ApplicationViewModel appViewModel)
        {
            InitializeComponent();
            DataContext = appViewModel;
        }
    }
}
