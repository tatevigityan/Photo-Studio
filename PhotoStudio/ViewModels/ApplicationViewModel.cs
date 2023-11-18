using DAL;
using DAL.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PhotoStudio.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private Visibility authValidationVisibility;
        private RelayCommand inputTextChangedCommand;
        private RelayCommand authCommand;
        private DataBaseDataOperation databaseContext;
        private UserData currentUser;
        private Auth authWindow;

        public Visibility AuthValidationVisibility { get => authValidationVisibility; set { authValidationVisibility = value; notifyPropertyChanged(); } }

        public ApplicationViewModel(Auth window)
        {
            AuthValidationVisibility = Visibility.Hidden;
            authWindow = window;
        }

        public RelayCommand AuthCommand
        {
            get
            {
                return authCommand ?? (authCommand = new RelayCommand(obj => {
                    try
                    {
                        databaseContext = new DataBaseDataOperation();
                        currentUser = databaseContext.getCurrentUser(authWindow.loginInput.Text, authWindow.passwordInput.Password);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                }));
            }
        }

        public RelayCommand InputTextChangedCommand
        {
            get
            {
                return inputTextChangedCommand ?? (inputTextChangedCommand = new RelayCommand(obj => {
                    try
                    {
                        AuthValidationVisibility = Visibility.Hidden;
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void notifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}