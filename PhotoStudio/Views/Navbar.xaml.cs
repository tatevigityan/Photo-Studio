using System.Windows;
using PhotoStudio.ViewModels;

namespace PhotoStudio
{
    public partial class Navbar : Window
    {
        public Navbar(ApplicationViewModel appViewModel)
        {
            InitializeComponent();
            DataContext = appViewModel;
        }
    }
}
