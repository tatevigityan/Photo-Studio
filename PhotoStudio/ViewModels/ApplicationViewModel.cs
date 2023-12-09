using System;
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
        private int foundClientId;
        private int currentMonthIncome;
        private int currentQuarterIncome;
        private int currentYearIncome;
        private int selectedServiceId;
        private int selectedStudioHallId;
        private int selectedServicePrice;
        private int currentMembershipPrice;
        private string[] tabs = { "booking", "services", "report", "charts" };

        private Visibility tabsVisibility;
        private Visibility authValidationVisibility;

        private List<Client> clients;
        private List<Booking> bookings;
        private List<StudioHall> studioHalls;
        private List<IncomeReport> incomeReports;
        private List<StudioService> studioServices;
        private List<StudioServiceMembership> memberships;

        private Client client;
        private UserData currentUser;
        private Booking foundBooking;
        private Auth authWindow;
        private Navbar navbarWindow;
        private ServicesTab servicesTab;
        private StatisticsTab statisticsTab;
        private StudioHallsTab studioHallsTab;
        private IncomeReportTab incomeReportTab;
        private DataBaseDataOperation databaseContext;

        private RelayCommand authCommand;
        private RelayCommand switchProfileCommand;
        private RelayCommand createBookingCommand;
        private RelayCommand updateReportCommand;
        private RelayCommand addServiceCommand;
        private RelayCommand createServiceMembershipCommand;
        private RelayCommand deleteServiceMembershipCommand;
        private RelayCommand inputTextChangedCommand;
        private RelayCommand openBookingDialogCommand;
        private RelayCommand openBookingsTabCommand;
        private RelayCommand openServicesTabCommand;
        private RelayCommand openChartsTabCommand;
        private RelayCommand openReportTabCommand;
        private RelayCommand searchBookingCommand;
        private RelayCommand searchStudioHallCommand;
        private RelayCommand updateMembershipPriceCommand;
        private RelayCommand saveMembershipCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public int CurrentMonthIncome { get => currentMonthIncome; set { currentMonthIncome = value; notifyPropertyChanged(); } }
        public int CurrentQuarterIncome { get => currentQuarterIncome; set { currentQuarterIncome = value; notifyPropertyChanged(); } }
        public int CurrentYearIncome { get => currentYearIncome; set { currentYearIncome = value; notifyPropertyChanged(); } }
        public int CurrentMembershipPrice { get => currentMembershipPrice; set { currentMembershipPrice = value; notifyPropertyChanged(); } }
        public Booking FoundBooking { get => foundBooking; set { foundBooking = value; notifyPropertyChanged(); } }
        public List<Client> Clients { get => clients; set { clients = value; notifyPropertyChanged(); } }
        public List<StudioHall> StudioHalls { get => studioHalls; set { studioHalls = value; notifyPropertyChanged(); } }
        public List<StudioService> StudioServices { get => studioServices; set { studioServices = value; notifyPropertyChanged(); } }
        public List<StudioServiceMembership> StudioServiceMemberships { get => memberships; set { memberships = value; notifyPropertyChanged(); } }
        public List<IncomeReport> IncomeReports { get => incomeReports; set { incomeReports = value; notifyPropertyChanged(); } }
        public List<Booking> Bookings { get => bookings; set { bookings = value; notifyPropertyChanged(); } }
        public Visibility AuthValidationVisibility { get => authValidationVisibility; set { authValidationVisibility = value; notifyPropertyChanged(); } }
        public Visibility TabsVisibility { get => tabsVisibility; set { tabsVisibility = value; notifyPropertyChanged(); } }

        public ApplicationViewModel(Auth window)
        {
            authWindow = window;
            AuthValidationVisibility = Visibility.Hidden;
            navbarWindow = new Navbar(this);
        }

        public RelayCommand CreateBookingCommand
        {
            get
            {
                return createBookingCommand ??
                  (createBookingCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          //Booking reservation = new Booking();
                          //reservation.RoomId = RoomId;
                          //reservation.UserId = currentUser.Id;
                          ////reservation.ArrivalDate = studioHalls.ReservationStart.DisplayDate;
                          ////reservation.DepartureDate = studioHalls.ReservationEnd.DisplayDate;
                          //reservation.TotalPrice = Rooms.Find(r => r.Id == RoomId).Cost * (int)(reservation.DepartureDate.Subtract(reservation.ArrivalDate).TotalDays);
                          //reservation.ReservationDate = DateTime.Today;
                          //reservation.ServicesNumber = 0;
                          //reservation.isActive = true;
                          //reservation.Number = reservation.Id + reservator.Passport.Replace(" ", "");
                          //reservation.Guests.Add(reservator);

                          //foreach (Guest newGuest in Guests)
                          //{
                          //    reservation.Guests.Add(newGuest);
                          //    databaseContext.AddGuest(newGuest);
                          //}

                          //reservation.NumberOfPeople = reservation.Guests.Count;

                          //databaseContext.AddGuest(reservator);

                          //databaseContext.CreateReservation(reservation);
                          //databaseContext.ReservateRoom(RoomId);

                          //studioHalls.ReservationGrid.Height = studioHalls.GuestGrid.Height = 300;
                          
                          
                          
                          
                          
                          
                          
                          
                          //studioHallsTab.GuestGrid.Visibility = Visibility.Hidden;
                          //studioHallsTab.SuccessGrid.Visibility = Visibility.Visible;
                          //loadStudioHallsTab();
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        public RelayCommand DialogNextCommand
        {
            get
            {
                return createBookingCommand ??
                  (createBookingCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (validateBookingDialog())
                          {
                              //reservator = new Guest();
                              //reservator.Name = studioHalls.GuestName.Text;
                              //reservator.Surname = studioHalls.GuestSurname.Text;
                              //reservator.Phone = studioHalls.GuestPhone.Text;

                              //bool enabled = true;

                              //var guest = databaseContext.FindGuest(reservator);

                              //if (guest != null && guest.Reservations != null)
                              //{
                              //    foreach (Booking reservation in guest.Reservations)
                              //        if (isBookingActive(reservation))
                              //        {
                              //            enabled = false;
                              //            reservator = guest;
                              //        }
                              //}

                              //if (enabled)
                              //    studioHalls.ReservationGrid.Visibility = Visibility.Visible;
                              //else
                              //    MessageBox.Show("Guest already has active reservation!");

                          }
                          else
                              MessageBox.Show("Please, fill all fields!");
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        public RelayCommand AddServiceCommand
        {
            get
            {
                return addServiceCommand ??
                  (addServiceCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          var button = (obj as Button);
                          var service = button.DataContext as StudioService;

                          selectedServiceId = service.id;

                          //servicesTab.Dialog.DataContext = null;
                          //servicesTab.CreateButton.DataContext = button;

                          //if (servicesTab.ReservationCard.Visibility == Visibility.Visible && (memberships.Count == 0
                          //  || memberships.Where(m => m.serviceId == selectedServiceId).ToList().Count == 0))
                          //{
                          //    servicesTab.serviceDuration.Value = 1;
                          //    selectedServicePrice = service.hourlyPrice;
                          //    CurrentMembershipPrice = service.hourlyPrice;

                          //    servicesTab.Dialog.IsOpen = true;
                          //}
                      }
                      catch (Exception error)
                      {
                          MessageBox.Show(error.Message);
                      }
                  }));
            }
        }

        public RelayCommand CreateServiceMembershipCommand
        {
            get
            {
                return createServiceMembershipCommand ??
                  (createServiceMembershipCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          //servicesTab.Dialog.IsOpen = false;

                          StudioServiceMembership membership = new StudioServiceMembership();
                          //membership.Duration = (int)servicesTab.serviceDuration.Value;
                          //membership.TotalPrice = MembershipPrice;
                          //membership.ReservationId = FoundBooking.Id;
                          //membership.GuestId = foundGuestId;
                          //membership.ServiceId = ServiceId;
                          //membership.StartDate = DateTime.Now;

                          memberships.Add(membership);

                          //FoundBooking.totalPrice += CurrentMembershipPrice;
                          //servicesTab.ReservationCard.DataContext = null;
                          //servicesTab.ReservationCard.DataContext = FoundBooking;

                          var button = (obj as Button);
                          var service = button.DataContext as StudioService;

                          service.closeVisibility = Visibility.Visible;

                          button.DataContext = null;
                          button.DataContext = service;
                      }
                      catch (Exception error)
                      {
                          MessageBox.Show(error.Message);
                      }
                  }));
            }
        }

        public RelayCommand DeleteServiceMembershipCommand
        {
            get
            {
                return deleteServiceMembershipCommand ??
                  (deleteServiceMembershipCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          var button = (obj as Button);
                          var service = button.DataContext as StudioService;

                          service.closeVisibility = Visibility.Hidden;

                          //FoundBooking.totalPrice -= memberships.FirstOrDefault(m => m.serviceId == service.id).totalPrice;
                          //servicesTab.ReservationCard.DataContext = null;
                          //servicesTab.ReservationCard.DataContext = FoundBooking;
                          memberships.RemoveAll(m => m.serviceId == service.id);

                          button.DataContext = null;
                          button.DataContext = service;
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        public RelayCommand SearchStudioHallCommand
        {
            get
            {
                return searchStudioHallCommand ??
                  (searchStudioHallCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                         // loadStudioHallsTab();
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        public RelayCommand SearchBookingCommand
        {
            get
            {
                return searchBookingCommand ??
                  (searchBookingCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          //bookings = databaseContext.getAllBookings();

                          FoundBooking = null;

                          //foreach (Booking booking in bookings)
                          //    if (booking.bookingDate == servicesTab.ReservationDate.DisplayDate)
                          //        foreach (Guest guest in booking.client)
                          //            if (guest.Name == servicesTab.GuestName.Text && guest.Surname == servicesTab.GuestSurname.Text
                          //              && guest.Passport == servicesTab.GuestPassport.Text)
                          //            {
                          //                FoundBooking = booking;
                          //                foundGuestId = guest.Id;
                          //            }

                          if (FoundBooking != null)
                          {
                              //var user = databaseContext.GetUsers().Where(r => r.Id == FoundBooking.UserId).FirstOrDefault();
                              //FoundBooking.Manager = user.Name + " " + user.SecondName;
                              //FoundBooking.Room = databaseContext.GetRooms().Where(r => r.Id == FoundBooking.RoomId).FirstOrDefault().Number;

                              //servicesTab.ReservationCard.DataContext = FoundBooking;
                              //servicesTab.ReservationsNotFound.Visibility = Visibility.Hidden;
                              //servicesTab.ReservationCard.Visibility = Visibility.Visible;

                              //memberships = databaseContext.GetMemberships(FoundBooking.Id, foundGuestId);
                              //foreach (StudioService service in Services)
                              //{
                              //    if (memberships.Where(m => m.ServiceId == service.Id).ToList().Count == 0)
                              //        service.closeVisibility = Visibility.Hidden;
                              //    else
                              //        service.closeVisibility = Visibility.Visible;

                              //}
                          }
                          else
                          {
                              //servicesTab.ReservationsNotFound.Visibility = Visibility.Visible;
                              //servicesTab.ReservationCard.Visibility = Visibility.Hidden;
                          }
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        private void loadStudioHallsTab()
        {
            //var selectedItem = (ComboBoxItem)studioHallsTab.roomCategoryComboBox.SelectedItem;

            //studioHalls = databaseContext.getAvailableStudioHalls();

            //studioHallsTab.bookRoomGrid.Children.Clear();

            int i = 0, j = 0;

            //foreach (StudioHall studioHall in studioHalls)
            //{
            //    Button roomButton = new Button();
            //    //if (studioHall.Status == "Available")
            //    //    roomButton.Style = studioHallsTab.TryFindResource(studioHall.category + "RoomStyle") as Style;
            //    //else
            //    //    roomButton.Style = studioHallsTab.TryFindResource(studioHall. + "RoomStyle") as Style;

            //    roomButton.Margin = new Thickness(180 * i, j * 180, 0, 0);

            //    roomButton.DataContext = studioHall;

            //    //studioHallsTab.bookRoomGrid.Children.Add(roomButton);

            //    i++;
            //    if (i > 3)
            //    {
            //        i = 0;
            //        j++;
            //    }
            //}
        }
        private void loadServicesTab(ComboBoxItem category)
        {
            studioServices = databaseContext.getStudioServices();
            //servicesTab.servicesGrid.Children.Clear();

            if (category.Name == "All")
            {
                int i = 0;

                foreach (StudioService service in studioServices)
                {
                    Button serviceCard = new Button();
                    //serviceCard.Style = servicesTab.TryFindResource(service.Category + "ServiceStyle") as System.Windows.Style;
                    serviceCard.Margin = new Thickness(0, i * 95, 0, 0);
                    serviceCard.DataContext = service;
                    //servicesTab.servicesGrid.Children.Add(serviceCard);
                    i++;
                }
            }
            else
            {
                int i = 0;

                //foreach (StudioService service in studioServices)
                //    if (service.Category == category.Name)
                //    {
                //        Button serviceCard = new Button();
                //        serviceCard.Style = servicesTab.TryFindResource(category.Name + "ServiceStyle") as System.Windows.Style;
                //        serviceCard.Margin = new Thickness(0, i * 95, 0, 0);
                //        serviceCard.DataContext = service;
                //        servicesTab.servicesGrid.Children.Add(serviceCard);
                //        i++;
                //    }
            }
        }
        private bool isBookingActive(Booking booking)
        {
            //todo
            if (booking.date > DateTime.Now)
                return false;
            return true;
        }
        private bool validateBookingDialog()
        {
            return true;
            //return studioHallsTab.GuestName.Text != "" && studioHallsTab.GuestSurname.Text != "" && studioHallsTab.GuestPhone.Text != "";
        }
        public void clearBookingDialog()
        {
            //studioHallsTab.GuestName.Text = "";
            //studioHallsTab.GuestSurname.Text = "";
            //studioHallsTab.GuestPhone.Text = "";


            //studioHalls.ReservationStart.SelectedDate = null;
            //studioHalls.ReservationEnd.SelectedDate = null;
            //studioHalls.ReservationStart.DisplayDateStart = DateTime.Now;
            //studioHalls.ReservationStart.DisplayDateEnd = DateTime.Now.AddMonths(2);
            //studioHalls.ReservationEnd.DisplayDateStart = DateTime.Now.AddDays(10);
            //studioHalls.ReservationEnd.DisplayDateEnd = DateTime.Now.AddMonths(3);


            //studioHallsTab.GuestGrid.Visibility = Visibility.Visible;
            //studioHallsTab.SuccessGrid.Visibility = Visibility.Hidden;
        }
        public RelayCommand OpenBookingsTabCommand
        {
            get
            {
                return openBookingsTabCommand ?? (openBookingsTabCommand = new RelayCommand(obj => {
                    try
                    {
                        //databaseContext.RoomControl();
                        //memberships = new List<StudioServiceMembership>();
                        //bookings = databaseContext.getAllBookings();

                        studioHallsTab = new StudioHallsTab(this);
                        navbarWindow.MainContent.Children.Clear();
                        navbarTabSelection("booking");
                        //loadStudioHallsTab();
                        navbarWindow.MainContent.Children.Add(studioHallsTab);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                }));
            }
        }
        public RelayCommand OpenServicesTabCommand
        {
            get
            {
                return openServicesTabCommand ?? (openServicesTabCommand = new RelayCommand(obj => {
                    try
                    {
                        //servicesTab = new ServicesTab(this);
                        //servicesTab.ReservationsNotFound.Visibility = Visibility.Hidden;
                        //servicesTab.ReservationCard.Visibility = Visibility.Hidden;

                        //StudioServices = databaseContext.getStudioServices();

                        //foreach (StudioService service in StudioServices)
                        //    service.closeVisibility = Visibility.Hidden;

                        //servicesTab.ReservationDate.SelectedDate = null;
                        //loadServicesTab((ComboBoxItem)servicesTab.serviceCategoryComboBox.SelectedItem);
                        //navbarWindow.MainContent.Children.Clear();
                        navbarTabSelection("services");
                        //navbarWindow.MainContent.Children.Add(servicesTab);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                }));
            }
        }
        public RelayCommand OpenBookingDialogCommand
        {
            get
            {
                return openBookingDialogCommand ??
                  (openBookingDialogCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          clearBookingDialog();
                          selectedStudioHallId = ((obj as Button).DataContext as StudioHall).id;
                          clients = new List<Client>();
                          //studioHallsTab.BookingDialog.IsOpen = true;
                      }
                      catch (Exception error)
                      {
                          MessageBox.Show(error.Message);
                      }
                  }));
            }
        }
        public RelayCommand UpdateMembershipPriceCommand
        {
            get
            {
                return updateMembershipPriceCommand ??
                  (updateMembershipPriceCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          //CurrentMembershipPrice = selectedServicePrice * (int)servicesTab.serviceDuration.Value;
                      }
                      catch (Exception error)
                      {
                          MessageBox.Show(error.Message);
                      }
                  }));
            }
        }
        public RelayCommand SaveMembershipCommand
        {
            get
            {
                return saveMembershipCommand ??
                  (saveMembershipCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          databaseContext.removeStudioServiceMemberships(FoundBooking.id, foundClientId);

                          foreach (StudioServiceMembership membership in memberships)
                              databaseContext.createStudioServiceMembership(membership);

                          foreach (StudioService service in studioServices)
                              service.closeVisibility = Visibility.Hidden;

                          //servicesTab.ReservationCard.Visibility = Visibility.Hidden;

                          //servicesTab.GuestName.Text = "";
                          //servicesTab.GuestSurname.Text = "";
                          //servicesTab.GuestPhone.Text = "";
                          //servicesTab.GuestPassport.Text = "";
                          //servicesTab.ReservationDate.SelectedDate = null;
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
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
                        if (currentUser != null)
                        {
                            loadTabs();
                        }
                        else
                        {
                            authWindow.loginInput.Text = "";
                            authWindow.passwordInput.Password = "";
                            AuthValidationVisibility = Visibility.Visible;
                        }
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
        public RelayCommand SwitchProfileCommand
        {
            get
            {
                return switchProfileCommand ?? (switchProfileCommand = new RelayCommand(obj => {
                    try
                    {
                        authWindow = new Auth();
                        authWindow.Show();
                        navbarWindow.Close();
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                }));
            }
        }
        public RelayCommand OpenChartsTabCommand
        {
            get
            {
                return openChartsTabCommand ?? (openChartsTabCommand = new RelayCommand(obj => {
                    try
                    {
                        statisticsTab = new StatisticsTab(this);
                        navbarWindow.MainContent.Children.Clear();
                        navbarTabSelection("charts");
                        loadChartsTab();
                        navbarWindow.MainContent.Children.Add(statisticsTab);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                }));
            }
        }
        public RelayCommand OpenReportTabCommand
        {
            get
            {
                return openReportTabCommand ?? (openReportTabCommand = new RelayCommand(obj => {
                    try
                    {
                        incomeReportTab = new IncomeReportTab(this);
                        navbarWindow.MainContent.Children.Clear();
                        navbarTabSelection("report");
                        incomeReportTab.StartDate.SelectedDate = DateTime.Now.AddMonths(-1);
                        incomeReportTab.EndDate.SelectedDate = DateTime.Now.AddMonths(1);
                        UpdateReportCommand.Execute(null);
                        navbarWindow.MainContent.Children.Add(incomeReportTab);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                }));
            }
        }
        public RelayCommand UpdateReportCommand
        {
            get
            {
                return updateReportCommand ??
                  (updateReportCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          IncomeReports = databaseContext.getBookings(incomeReportTab.StartDate.SelectedDate, incomeReportTab.EndDate.SelectedDate);
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }
        private void loadTabs()
        {
            if (currentUser.role == "manager")
            {
                TabsVisibility = Visibility.Visible;
                OpenBookingsTabCommand.Execute(null);
            }
            else
            {
                TabsVisibility = Visibility.Hidden;
                OpenChartsTabCommand.Execute(null);
            }
            navbarWindow.Show();
            authWindow.Close();
        }
        private void loadChartsTab()
        {
            CurrentMonthIncome = databaseContext.getCurrentMonthIncome();
            CurrentQuarterIncome = databaseContext.getCurrentQuarterIncome();
            CurrentYearIncome = databaseContext.getCurrentYearIncome();

            ChartValues<int> quantity = new ChartValues<int>();
            quantity.Add(databaseContext.getStudioHallsCount("love story"));
            statisticsTab.Chart.ActualSeries.Where(s => s.Title == "love story").First().Values =
                quantity;

            quantity = new ChartValues<int>();
            quantity.Add(databaseContext.getStudioHallsCount("new year"));
            statisticsTab.Chart.ActualSeries.Where(s => s.Title == "new year").First().Values =
                quantity;

            quantity = new ChartValues<int>();
            quantity.Add(databaseContext.getStudioHallsCount("professional"));
            statisticsTab.Chart.ActualSeries.Where(s => s.Title == "professional").First().Values =
                quantity;

            quantity = new ChartValues<int>();
            quantity.Add(databaseContext.getStudioHallsCount("nature"));
            statisticsTab.Chart.ActualSeries.Where(s => s.Title == "nature").First().Values =
                quantity;
        }
        private void navbarTabSelection(string selectedTab)
        {
            foreach (string tab in tabs)
                (navbarWindow.FindName(tab) as Button)?.SetValue(Control.BackgroundProperty, new SolidColorBrush(tab == selectedTab ? Color.FromRgb(210, 180, 140) : Color.FromRgb(255, 255, 255)));
        }
        private void notifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
