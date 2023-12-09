using System.Windows.Controls;
using PhotoStudio.ViewModels;

namespace PhotoStudio
{
    public partial class ServicesTab : UserControl
    {
        public ServicesTab(ApplicationViewModel appViewModel)
        {
            InitializeComponent();
            DataContext = appViewModel;
        }
    }
}
