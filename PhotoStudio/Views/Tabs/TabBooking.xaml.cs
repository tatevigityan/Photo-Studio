using System.Windows.Controls;
using PhotoStudio.ViewModels;

namespace PhotoStudio
{
    public partial class TabBooking : UserControl
    {
        public TabBooking(ApplicationViewModel appViewModel)
        {
            InitializeComponent();
            DataContext = appViewModel;
        }
    }
}
