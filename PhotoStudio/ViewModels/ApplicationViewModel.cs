using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using Microsoft.Office.Interop.Word;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Linq;
using System.IO;
using LiveCharts;
using DAL.Models;
using DAL;
using MaterialDesignThemes.Wpf;
using MahApps.Metro.Controls.Dialogs;

namespace PhotoStudio.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private int monthIncome;
        private int quarterIncome;
        private int yearIncome;
        private int membershipPrice;
        private int totalSum;
        private string[] tabs = { "booking", "statistics", "report" };
        private DateTime date;

        private Visibility validationVisibility;
        private Visibility visibility;
        private Visibility error;

        private List<Hall> halls;
        private List<Service> services;
        private List<IncomeReport> incomeReports;
        private List<CategoryHall> categories;
        private List<TimeSlot> hours;

        private UserData currentUser;
        private Hall hall;
        private Client client;
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
        public int totalBookingSum { get => totalSum; set { totalSum = value; notifyPropertyChanged(); } }
        public Hall selectedHall { get => hall; set { hall = value; notifyPropertyChanged(); } }
        public Client selectedClient { get => client; set { client = value; notifyPropertyChanged(); } }
        public DateTime bookingDate { get => date; set { date = value; notifyPropertyChanged(); } }
        public List<Hall> hallList { get => halls; set { halls = value; notifyPropertyChanged(); } }
        public List<TimeSlot> hourList { get => hours; set { hours = value; notifyPropertyChanged(); } }
        public List<Service> serviceList { get => services; set { services = value; notifyPropertyChanged(); } }
        public List<IncomeReport> incomeList { get => incomeReports; set { incomeReports = value; notifyPropertyChanged(); } }
        public List<CategoryHall> hallCategories { get => categories; set { categories = value; notifyPropertyChanged(); } }
        public Visibility authValidationVisibility { get => validationVisibility; set { validationVisibility = value; notifyPropertyChanged(); } }
        public Visibility fieldsVisibility { get => visibility; set { visibility = value; notifyPropertyChanged(); } }
        public Visibility errorVisibility { get => error; set { error = value; notifyPropertyChanged(); } }

        public ApplicationViewModel(Auth window)
        {
            auth = window;
            authValidationVisibility = Visibility.Hidden;
            fieldsVisibility = Visibility.Collapsed;
            errorVisibility = Visibility.Collapsed;
            bookingDate = DateTime.Now;
            selectedClient = new Client();
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

        public RelayCommand createBooking
        {
            get
            {
                return new RelayCommand(obj => {
                    try
                    {
                        selectedHall = obj as Hall;
                        hourList = databaseContext.getAvailableTimeSlots(bookingDate, selectedHall.id);
                        serviceList = databaseContext.getServices();

                        if (hourList.Count < 1)
                        {
                            errorVisibility = Visibility.Visible;
                            fieldsVisibility = Visibility.Collapsed;
                        } else
                        {
                            errorVisibility = Visibility.Collapsed;
                            fieldsVisibility = Visibility.Visible;
                        }
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                });
            }
        }

        public RelayCommand completeBooking
        {
            get
            {
                return new RelayCommand(async obj => {
                    try
                    {
                        string validationMessage = validateSelectedTimeSlots(hourList);

                        if (validationMessage != null)
                        {
                            MessageBox.Show(validationMessage);
                        } else
                        {
                            DateTime currentTime = bookingDate;
                            int clientId = databaseContext.getOrCreateClient(selectedClient);
                            int userId = databaseContext.getCurrentUserId(currentUser.username);

                            Booking booking = new Booking {
                                clientId = clientId,
                                userId = userId,
                                hallId = selectedHall.id,
                                durationHours = hourList.Where(h => h.isSelected).ToList().Count,
                                totalPrice = totalBookingSum,
                                dateTime = new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day, hours.FirstOrDefault(h => h.isSelected).startTime, 0, 0, 0),
                                services = serviceList.Where(s => s.isSelected).ToList(),
                            };
                            databaseContext.createNewBooking(booking);

                            cancelBooking.Execute(null);
                            MessageBox.Show("Booking completed!");
                        }
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                });
            }
        }

        public RelayCommand cancelBooking
        {
            get
            {
                return new RelayCommand(obj => {
                    try
                    {
                        selectedHall = null;
                        totalBookingSum = 0;
                        selectedClient = new Client();
                        serviceList = new List<Service>();
                        hourList = new List<TimeSlot>();
                        fieldsVisibility = Visibility.Hidden;
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
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
                        hallCategories = databaseContext.getHallCategories();
                        halls = databaseContext.getHalls(4);
                        configureDatePicker();
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
                        navbarTabSelection("statistics");
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

        public RelayCommand saveReportCommand
        {
            get
            {
                return new RelayCommand(obj =>
                 {
                      try {
                          saveFile();
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                });
            }
        }

        public RelayCommand changeHallCategoryCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    try
                    {
                        CategoryHall category = (CategoryHall)tabBooking.hallCategoryComboBox.SelectedItem;
                        halls = databaseContext.getHalls(category.id);
                        errorVisibility = Visibility.Collapsed;
                        loadTabBookings();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        public RelayCommand updateTotalSumCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    try
                    {
                        if (selectedHall != null)
                        {
                            int hours = hourList.Where(s => s.isSelected).ToList().Count;
                            totalBookingSum = serviceList.Where(s => s.isSelected).Sum(s => s.hourlyPrice) * hours + hours * selectedHall.hourlyPrice;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        public string validateSelectedTimeSlots(List<TimeSlot> timeSlots)
        {
            if (selectedClient.name == "" || selectedClient.name == null)
            {
                return "Имя клиента не заполнено!";
            }

            if (selectedClient.phone == "" || selectedClient.phone == null)
            {
                return "Номер телефона клиента не заполнено!";
            }

            int selectedCount = 0;
            int lastSelectedIndex = -1;

            for (int i = 0; i < timeSlots.Count; i++)
            {
                if (timeSlots[i].isSelected)
                {
                    selectedCount++;

                    if (lastSelectedIndex != -1 && i != lastSelectedIndex + 1)
                    {
                        return "Временной интервал не может прерываться!";
                    }

                    lastSelectedIndex = i;
                }
            }

            if (selectedCount == 0)
            {
                return "Пожалуйста, выберите временной интервал!";
            }

            return null;
        }

        private void loadTabs()
        {
            if (currentUser.role == "director")
            {
                navbar.statistics.Visibility = Visibility.Visible;
                navbar.report.Visibility = Visibility.Visible;
                navbar.booking.Visibility = Visibility.Hidden;
                openChartsTabCommand.Execute(null);
            }
            else
            {
                navbar.statistics.Visibility = Visibility.Hidden;
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
            tabBooking.bookingGrid.Children.Clear();

            int index = 0;

            foreach (Hall hall in halls)
            {
                Button hallButton = new Button();
                hallButton.Style = tabBooking.TryFindResource("hallButtonStyle") as System.Windows.Style;
                hallButton.Margin = new Thickness(0, index * 320, 0, 0);
                hallButton.DataContext = hall;

                StackPanel contentPanel = new StackPanel();

                TextBlock titleTextBlock = new TextBlock();
                titleTextBlock.Text = hall.name;
                titleTextBlock.Foreground = Brushes.White;
                titleTextBlock.FontFamily = new FontFamily("Century Gothic");
                titleTextBlock.FontSize = 20;
                titleTextBlock.TextAlignment = TextAlignment.Center;
                titleTextBlock.HorizontalAlignment = HorizontalAlignment.Left;

                Image hallImage = new Image();
                hallImage.Height = 270;
                hallImage.Source = new BitmapImage(new Uri($"pack://application:,,,/PhotoStudio;component/Resources/Halls/{hall.imageTitle}", UriKind.Absolute));

                contentPanel.Children.Add(titleTextBlock);
                contentPanel.Children.Add(hallImage);

                hallButton.Content = contentPanel;
                hallButton.Command = createBooking;
                hallButton.CommandParameter = hall;

                tabBooking.bookingGrid.Children.Add(hallButton);
                index++;
            }
        }

        private void selectedDateChanged(object sender, EventArgs e)
        {
            if (selectedHall != null)
            {
                hourList = databaseContext.getAvailableTimeSlots(bookingDate, selectedHall.id);
                serviceList = databaseContext.getServices();
            }
        }

        private void configureDatePicker()
        {
            tabBooking.bookingDatePicker.DisplayDateStart = DateTime.Now;
            tabBooking.bookingDatePicker.DisplayDateEnd = DateTime.Now.AddDays(30);
            tabBooking.bookingDatePicker.SelectedDateChanged += selectedDateChanged;

            for (DateTime date = (DateTime)tabBooking.bookingDatePicker.DisplayDateStart; date <= tabBooking.bookingDatePicker.DisplayDateEnd; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    tabBooking.bookingDatePicker.BlackoutDates.Add(new CalendarDateRange(date));
                }
            }
        }

        private void saveFile()
        {
            try
            {
                _Application oWord = new Microsoft.Office.Interop.Word.Application();
                object oMissing = System.Reflection.Missing.Value;
                object oEndOfDoc = "\\endofdoc";

                string dateNow = DateTime.Now.ToString("dd MMMM yyyy H.mm.ss");
                DirectoryInfo di = Directory.CreateDirectory(@"C:\Reports");
                string path = @"C:\Reports\Report from  - " + dateNow + ".docx";

                oWord.Visible = false;

                Document wrdDocument;
                wrdDocument = oWord.Documents.Add();
                wrdDocument.ActiveWindow.Visible = false;
                wrdDocument.PageSetup.TopMargin = oWord.InchesToPoints((float)0.5);
                wrdDocument.PageSetup.BottomMargin = oWord.InchesToPoints((float)0.5);
                wrdDocument.PageSetup.LeftMargin = oWord.InchesToPoints((float)0.5);
                wrdDocument.PageSetup.RightMargin = oWord.InchesToPoints((float)0.5);

                var title = wrdDocument.Content.Paragraphs.Add();
                title.Range.Text = "Отчет";
                title.Range.Font.Bold = 1;
                title.Range.Font.Size = 16;
                title.Range.Font.Name = "Times New Roman";
                title.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                title.Range.InsertParagraphAfter();
                title.Range.Paragraphs.SpaceAfter = 10;

                title.Range.Text = "Дата: " + dateNow;
                title.Range.Font.Bold = 0;
                title.Range.Font.Size = 12;
                title.Range.Font.Name = "Times New Roman";
                title.Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                title.Format.SpaceAfter = 2;
                title.Range.InsertParagraphAfter();
                title.Range.Paragraphs.SpaceAfter = 0;

                title.Range.Text = "Бронирования фотостудии 'Ламбада'";
                title.Range.Font.Bold = 0;
                title.Range.Font.Size = 14;
                title.Range.Font.Name = "Times New Roman";
                title.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                title.Range.InsertParagraphAfter();
                title.Range.Paragraphs.SpaceAfter = 0;

                title.Range.Text = "С " + tabReport.StartDate.SelectedDate.Value.ToShortDateString() + " По " + tabReport.EndDate.SelectedDate.Value.ToShortDateString();
                title.Range.Font.Bold = 0;
                title.Range.Font.Size = 12;
                title.Range.Font.Name = "Times New Roman";
                title.Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                title.Range.Paragraphs.SpaceAfter = 20;

                Table oTable;
                Range wrdRng = wrdDocument.Bookmarks.get_Item(ref oEndOfDoc).Range;
                oTable = wrdDocument.Tables.Add(wrdRng, 4, 4, ref oMissing, ref oMissing);
                oTable.Range.ParagraphFormat.SpaceAfter = 6;
                oTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                oTable.Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
                oTable.Range.Paragraphs.SpaceAfter = 0;
                oTable.Range.Paragraphs.SpaceBefore = 0;

                oTable.Cell(1, 1).Range.Text = "Зал";
                oTable.Cell(1, 2).Range.Text = "Дата бронирования";
                oTable.Cell(1, 3).Range.Text = "Итоговая стоимость";

                int totalIncome = 0;

                for (int r = 1; r <= incomeReports.Count + 1; r++)
                    for (int c = 1; c <= 3; c++)
                    {
                        oTable.Cell(r, c).Range.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
                        oTable.Cell(r, c).Range.Borders[WdBorderType.wdBorderRight].LineStyle = WdLineStyle.wdLineStyleSingle;
                        oTable.Cell(r, c).Range.Borders[WdBorderType.wdBorderLeft].LineStyle = WdLineStyle.wdLineStyleSingle;
                        oTable.Cell(r, c).Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;

                        oTable.Cell(r, c).Range.Font.Bold = 1;
                        oTable.Cell(r, c).Range.Font.Size = 12;
                        oTable.Cell(r, c).Range.Font.Name = "Times New Roman";

                        if (r > 1)
                        {
                            switch (c)
                            {
                                case 1:
                                    oTable.Cell(r, c).Range.Text = incomeReports[r - 2].Hall;
                                    break;
                                case 2:
                                    oTable.Cell(r, c).Range.Text = incomeReports[r - 2].Date;
                                    break;
                                case 3:
                                    {
                                        oTable.Cell(r, c).Range.Text = incomeReports[r - 2].Price;
                                        totalIncome += Convert.ToInt32(incomeReports[r - 2].Price);
                                    }
                                    break;
                            }

                            oTable.Cell(r, c).Range.Font.Bold = 0;
                        }
                    }

                title.Range.Text = "Суммированный доход за период: " + totalIncome.ToString() + " руб.";
                title.Range.Font.Bold = 1;
                title.Range.Font.Size = 12;
                title.Range.Font.Name = "Times New Roman";
                title.Format.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                title.Range.InsertParagraphAfter();
                title.Range.Paragraphs.SpaceAfter = 0;

                oWord.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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