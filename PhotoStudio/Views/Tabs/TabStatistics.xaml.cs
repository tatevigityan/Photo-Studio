using System.Windows.Controls;
using PhotoStudio.ViewModels;

namespace PhotoStudio
{
    public partial class TabStatistics : UserControl
    {
        public TabStatistics(ApplicationViewModel appViewModel)
        {
            InitializeComponent();
            DataContext = appViewModel;
        }
    }
}
