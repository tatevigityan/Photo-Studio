using System.Windows.Controls;
using PhotoStudio.ViewModels;

namespace PhotoStudio
{
    public partial class TabReport : UserControl
    {
        public TabReport(ApplicationViewModel appViewModel)
        {
            InitializeComponent();
            DataContext = appViewModel;
        }
    }
}
