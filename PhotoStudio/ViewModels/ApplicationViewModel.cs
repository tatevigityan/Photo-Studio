﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Linq;
using LiveCharts;
using DAL.Models;
using DAL;

namespace PhotoStudio.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private int monthIncome;
        private int quarterIncome;
        private int yearIncome;
        private int membershipPrice;
        private string[] tabs = { "booking", "report", "charts" };

        private Visibility validationVisibility;

        private List<Hall> halls;
        private List<Service> services;
        private List<IncomeReport> incomeReports;

        private UserData currentUser;
        private Auth auth;
        private Navbar navbar;
        private TabReport tabReport;
        private TabBooking tabBooking;
        private TabStatistics tabStatistics;
        private DataBaseDataOperation databaseContext;

        public event PropertyChangedEventHandler PropertyChanged;

        public int currentMonthIncome { get => monthIncome; set { monthIncome = value; notifyPropertyChanged(); } }
        public int currentQuarterIncome { get => quarterIncome; set { quarterIncome = value; notifyPropertyChanged(); } }
        public int currentYearIncome { get => yearIncome; set { yearIncome = value; notifyPropertyChanged(); } }
        public int currentMembershipPrice { get => membershipPrice; set { membershipPrice = value; notifyPropertyChanged(); } }
        public List<Hall> hallList { get => halls; set { halls = value; notifyPropertyChanged(); } }
        public List<Service> serviceList { get => services; set { services = value; notifyPropertyChanged(); } }
        public List<IncomeReport> incomeList { get => incomeReports; set { incomeReports = value; notifyPropertyChanged(); } }
        public Visibility authValidationVisibility { get => validationVisibility; set { validationVisibility = value; notifyPropertyChanged(); } }

        public ApplicationViewModel(Auth window)
        {
            auth = window;
            authValidationVisibility = Visibility.Hidden;
            navbar = new Navbar(this);
        }

        public RelayCommand authCommand
        {
            get
            {
                return new RelayCommand(obj => {
                    try
                    {
                        databaseContext = new DataBaseDataOperation();
                        currentUser = databaseContext.getCurrentUser(auth.loginInput.Text, auth.passwordInput.Password);
                        if (currentUser != null)
                        {
                            loadTabs();
                        }
                        else
                        {
                            auth.loginInput.Text = "";
                            auth.passwordInput.Password = "";
                            authValidationVisibility = Visibility.Visible;
                        }
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                });
            }
        }

        public RelayCommand inputTextChangedCommand
        {
            get
            {
                return new RelayCommand(obj => {
                    try
                    {
                        authValidationVisibility = Visibility.Hidden;
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                });
            }
        }

        public RelayCommand switchProfileCommand
        {
            get
            {
                return new RelayCommand(obj => {
                    try
                    {
                        auth = new Auth();
                        auth.Show();
                        navbar.Close();
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                });
            }
        }

        public RelayCommand createBookingCommand
        {
            get
            {
                return new RelayCommand(obj =>
                  {
                      try
                      {
                          MessageBox.Show("Booking created!");
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  });
            }
        }

        public RelayCommand openBookingsTabCommand
        {
            get
            {
                return new RelayCommand(obj => {
                    try
                    {
                        tabBooking = new TabBooking(this);
                        navbar.MainContent.Children.Clear();
                        navbarTabSelection("booking");
                        loadTabBookings();
                        navbar.MainContent.Children.Add(tabBooking);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                });
            }
        }

        public RelayCommand openChartsTabCommand
        {
            get
            {
                return new RelayCommand(obj => {
                    try
                    {
                        tabStatistics = new TabStatistics(this);
                        navbar.MainContent.Children.Clear();
                        navbarTabSelection("charts");
                        loadTabStatistics();
                        navbar.MainContent.Children.Add(tabStatistics);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                });
            }
        }

        public RelayCommand openReportTabCommand
        {
            get
            {
                return new RelayCommand(obj => {
                    try
                    {
                        tabReport = new TabReport(this);
                        navbar.MainContent.Children.Clear();
                        navbarTabSelection("report");
                        tabReport.StartDate.SelectedDate = DateTime.Now.AddMonths(-1);
                        tabReport.EndDate.SelectedDate = DateTime.Now.AddMonths(1);
                        updateReportCommand.Execute(null);
                        navbar.MainContent.Children.Add(tabReport);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                });
            }
        }

        public RelayCommand updateReportCommand
        {
            get
            {
                return new RelayCommand(obj =>
                  {
                      try
                      {
                          incomeList = databaseContext.getBookings(tabReport.StartDate.SelectedDate, tabReport.EndDate.SelectedDate);
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  });
            }
        }

        private void loadTabs()
        {
            if (currentUser.role == "director")
            {
                navbar.charts.Visibility = Visibility.Visible;
                navbar.report.Visibility = Visibility.Visible;
                navbar.booking.Visibility = Visibility.Hidden;
                openChartsTabCommand.Execute(null);
            }
            else
            {
                navbar.charts.Visibility = Visibility.Hidden;
                navbar.report.Visibility = Visibility.Hidden;
                navbar.booking.Visibility = Visibility.Visible;
                openBookingsTabCommand.Execute(null);
            }
            navbar.Show();
            auth.Close();
        }

        private void loadTabStatistics()
        {
            currentMonthIncome = databaseContext.getCurrentMonthIncome();
            currentQuarterIncome = databaseContext.getCurrentQuarterIncome();
            currentYearIncome = databaseContext.getCurrentYearIncome();

            List<CategoryHall> studioHallCategories = databaseContext.getHallCategories();
            ChartValues<int> quantity;

            foreach (CategoryHall category in studioHallCategories)
            {
                quantity = new ChartValues<int>();
                quantity.Add(databaseContext.getHallsCount(category.id));
                tabStatistics.Chart.ActualSeries.Where(s => s.Title == category.name).First().Values = quantity;
            }
        }

        private void loadTabBookings()
        {
            var selectedItem = (ComboBoxItem)tabBooking.hallCategoryComboBox.SelectedItem;
            halls = databaseContext.getHalls();
            tabBooking.bookingGrid.Children.Clear();

            int i = 0, j = 0;

            foreach (Hall hall in halls)
            {
                Button hallButton = new Button();
                hallButton.Style = tabBooking.TryFindResource("HallStyle") as Style;
                hallButton.Margin = new Thickness(180 * i, j * 180, 0, 0);
                hallButton.DataContext = hall;

                tabBooking.bookingGrid.Children.Add(hallButton);

                i++;
                if (i > 1)
                {
                    i = 0;
                    j++;
                }
            }
        }

        private void navbarTabSelection(string selectedTab)
        {
            foreach (string tab in tabs)
                (navbar.FindName(tab) as Button)?.SetValue(Control.BackgroundProperty, new SolidColorBrush(tab == selectedTab ? Color.FromRgb(210, 180, 140) : Color.FromRgb(255, 255, 255)));
        }

        private void notifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}