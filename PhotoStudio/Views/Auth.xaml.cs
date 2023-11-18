using System.Windows;
using PhotoStudio.ViewModels;

namespace PhotoStudio
{
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel(this);
        }
    }
}

