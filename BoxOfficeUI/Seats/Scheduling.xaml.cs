using BoxOffice.DAL;
using BoxOffice.Model;
using BoxOfficeUI.Util;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace BoxOfficeUI.SeatLayout
{
    /// <summary>
    /// Interaction logic for LayoutDesign.xaml
    /// </summary>
    public partial class Scheduling : UserControl, INotifyPropertyChanged
    {
        #region Loading Page & DropDowns
        public Scheduling()
        {
            try
            {
                InitializeComponent();

                dprStartingDate.DisplayDateStart = DateTime.Now;
                SchedulCalendar.DisplayStartDate = System.DateTime.Now.AddDays(-1 * (System.DateTime.Now.Day - 1));

                dprStartingDate.DisplayDateEnd = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationSettings.AppSettings["CalenderDuration"]));
                SchedulCalendar.DisplayEndDate = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationSettings.AppSettings["CalenderDuration"]) - 1);

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.RunWorkerAsync(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() => LoadDropdownData()));
        }

        private void LoadDropdownData()
        {
            try
            {
                int selectedIndex = lstScreens.SelectedIndex;
                lstScreens.ItemsSource = new SeatLayoutConfig().GetScreens();
                lstScreens.SelectedIndex = selectedIndex == -1 ? 0 : selectedIndex;

                Helper.LoadDropDownSource(cmbMovie, new MovieTimingsRepository().GetMovies(), "MovieName", "Id");
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }
        #endregion

        #region Screen and Calender events
        private void lstScreens_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() => pgrProgress.Visibility = Visibility.Visible));

                Dispatcher.Invoke(DispatcherPriority.Background,
                                  new Action(() => LoadSchedule()));
            }
            catch (Exception ex)
            {
                pgrProgress.Visibility = Visibility.Collapsed;
                LogExceptions.LogException(ex);
            }
        }

        private void LoadSchedule()
        {
            try
            {
                LoadSchedulingMovies();

                calanderGrid.Visibility = Visibility.Visible;
                SeatTemplates = new ObservableCollection<Vendors>(new SeatLayoutConfig().GetScreenTemplates(Convert.ToInt32(lstScreens.SelectedValue)));

                Helper.LoadDropDownSource(listSeatTemplates, SeatTemplates.ToList(), "TEMPLATENAME", "TEMPLATE_ID");

                listSeatTemplates.Visibility = (SeatTemplates.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
                lblTemplate.Visibility = (SeatTemplates.Count > 0) ? Visibility.Visible : Visibility.Collapsed;

                LoadTicketPrices();
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void SchedulCalendar_SchedulingDblClicked(int Scheduling_Id)
        {
            try
            {
                var result = ModernDialog.ShowMessage("Are you sure, you want to Inactivate Scheduled Movie Timing ?", "Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    new MovieTimingsRepository().InactiveMovieTiming(Scheduling_Id);
                    ModernDialog.ShowMessage("Scheduled Movie Timing Inactivated", "Alert", MessageBoxButton.OK);
                    LoadSchedulingMovies();
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void SchedulCalendar_DayBoxDoubleClicked(BoxOfficeUI.CalenderView.MonthView.NewAppointmentEventArgs e)
        {
            try
            {
                ShowControlsVisibility(true);
                dprStartingDate.SelectedDateChanged -= DprStartingDate_SelectedDateChanged;
                dprStartingDate.SelectedDate = new DateTime(Convert.ToDateTime(e.StartDate.Value.Date).Year, Convert.ToDateTime(e.StartDate.Value.Date).Month, Convert.ToDateTime(e.StartDate.Value.Date).Day);
                dprStartingDate.SelectedDateChanged += DprStartingDate_SelectedDateChanged;

                LoadHoursAndMinuts(e.StartDate.Value.Date == DateTime.Now.Date ? DateTime.Now.Hour : 1);
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void DprStartingDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadHoursAndMinuts(dprStartingDate.SelectedDate == DateTime.Now.Date ? DateTime.Now.Hour : 1);
        }

        private void SchedulCalendar_DisplayMonthChanged(CalenderView.MonthView.MonthChangedEventArgs e, int addMonths)
        {
            SchedulCalendar.DisplayStartDate.AddMonths(addMonths);
            LoadSchedulingMovies();
        }
        #endregion

        #region Adding New Movie events
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime schudeldOn = Convert.ToDateTime(dprStartingDate.SelectedDate);
                DateTime showTime = new DateTime(schudeldOn.Year, schudeldOn.Month, schudeldOn.Day, Convert.ToInt32((cmbHours.SelectedItem as ComboBoxItem).Content), Convert.ToInt32((cmbMinuts.SelectedItem as ComboBoxItem).Content), 0);

                if (string.IsNullOrEmpty(txtShowTimes.Text))
                {
                    ModernDialog.ShowMessage("Please select Show times", "Alert", MessageBoxButton.OK);
                    return;
                }

                var result = ModernDialog.ShowMessage("Are you sure, you want to Save new Movie Timing ?", "Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    List<MovieTimings> objMovieTimingsList = new List<MovieTimings>();
                    List<Screenclasses> objScreenclasses = new List<Screenclasses>();

                    foreach (TicketPricesAndTaxes obj in this.TicketPricesAndTaxes)
                    {
                        objScreenclasses.Add(new Screenclasses()
                        {
                            CGST = obj.CGST,
                            SGST = obj.SGST,
                            ScreenClassName = obj.ClassName,
                            ScreenClassId = Convert.ToInt32(obj.FK_ScreenClasses_ID),
                            MC = obj.MC,
                            TicketPrice = obj.TicketPrice
                        });
                    }

                    for (int i = 0; i < Convert.ToInt32(txtDaysCount.Text); i++)
                    {
                        showTime = showTime.AddDays(i == 0 ? 0 : 1);

                        foreach (string time in txtShowTimes.Text.Split(','))
                        {
                            showTime = new DateTime(showTime.Year, showTime.Month, showTime.Day, Convert.ToInt32(time.Split(':')[0]), Convert.ToInt32(time.Split(':')[1]), 0, 0); ;
                            objMovieTimingsList.Add(new MovieTimings()
                            {
                                ScreenId = Convert.ToInt32(lstScreens.SelectedValue),
                                MovieId = Convert.ToInt32(cmbMovie.SelectedValue),
                                Date = Convert.ToDateTime(dprStartingDate.SelectedDate),
                                IsOnline = Convert.ToBoolean(chkInternetSales.IsChecked),
                                ShowDateTime = showTime,
                                scs = objScreenclasses,
                                TemplateIds = string.Join(",", SeatTemplates.ToList().Where(w => w.IsSelected).Select(s => s.TEMPLATE_ID))
                            });
                        }
                    }

                    int returnResult = new MovieTimingsRepository().InsertMovieTiming(objMovieTimingsList);

                    if (returnResult > 0)
                    {
                        LoadSchedulingMovies();

                        ShowControlsVisibility(false);

                        txtDaysCount.Text = "1";
                        txtShowTimes.Text = string.Empty;

                        ModernDialog.ShowMessage("New schedule saved successfully", "Alert", MessageBoxButton.OK);
                    }
                    else if (returnResult == -102)
                    {
                        ModernDialog.ShowMessage("Already show is scheduled. Minimum you should maintain 1.30 Min", "Alert", MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ShowControlsVisibility(false);
        }

        private void textBox_DecimalTextInput(object sender, TextCompositionEventArgs e)
        {
            Helper.DecimalTextbox(sender, e);
        }

        private void btnminus_Click(object sender, RoutedEventArgs e)
        {
            txtDaysCount.Text = Convert.ToString(Convert.ToInt32(txtDaysCount.Text) - 1);
            btnminus.IsHitTestVisible = Convert.ToInt32(txtDaysCount.Text) > 1;
        }

        private void btnplus_Click(object sender, RoutedEventArgs e)
        {
            txtDaysCount.Text = Convert.ToString(Convert.ToInt32(txtDaysCount.Text) + 1);
            btnminus.IsHitTestVisible = Convert.ToInt32(txtDaysCount.Text) > 1;
        }

        private void cmbAutoSelection_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                ComboBox myComboBox = sender as ComboBox;
                // Get the textbox part of the combobox
                TextBox textBox = myComboBox.Template.FindName("PART_EditableTextBox", myComboBox) as TextBox;

                // holds the list of combobox items as strings
                List<String> items = new List<String>();

                // indicates whether the new character added should be removed
                bool shouldRemove = true;

                for (int i = 0; i < myComboBox.Items.Count; i++)
                {
                    items.Add(((ComboBoxItem)myComboBox.Items.GetItemAt(i)).Content.ToString());
                }

                for (int i = 0; i < items.Count; i++)
                {
                    // legal character input
                    if (textBox.Text != "" && items.ElementAt(i).StartsWith(textBox.Text))
                    {
                        shouldRemove = false;
                        break;
                    }
                }

                // illegal character input
                if (textBox.Text != "" && shouldRemove)
                {
                    textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
                    textBox.CaretIndex = textBox.Text.Length;
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }

        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Helper.NumericTextbox(sender, e);
        }

        private void TimeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Helper.TimeTextbox(txtShowTimes.Text);
        }

        #endregion

        #region Data Loading Methods
        private void LoadTicketPrices()
        {
            try
            {
                TicketPricesAndTaxes = new ObservableCollection<BoxOffice.Model.TicketPricesAndTaxes>();
                foreach (TicketPricesAndTaxes obj in new MovieTimingsRepository().GetTicketPriceAndTax(Convert.ToInt32(lstScreens.SelectedValue)).ToList())
                {
                    this.TicketPricesAndTaxes.Add(new BoxOffice.Model.TicketPricesAndTaxes()
                    {
                        CGST = obj.CGST,
                        SGST = obj.SGST,
                        ClassName = obj.ClassName,
                        FK_ScreenClasses_ID = obj.FK_ScreenClasses_ID,
                        Id = obj.Id,
                        MC = obj.MC,
                        TaxConstantValue = obj.TaxConstantValue,
                        TaxPercentage = obj.TaxPercentage,
                        TicketPrice = obj.TicketPrice
                    });
                }

                dgrdCosts.ItemsSource = this.TicketPricesAndTaxes;
                pgrProgress.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                pgrProgress.Visibility = Visibility.Collapsed;
                LogExceptions.LogException(ex);
            }
        }

        private void LoadSchedulingMovies()
        {
            try
            {
                IEnumerable<MovieTimings> objMovieTimings = new MovieTimingsRepository().GetScreenMovies(Convert.ToInt32(lstScreens.SelectedValue), SchedulCalendar.DisplayStartDate.Month, SchedulCalendar.DisplayStartDate.Year);

                List<CalenderView.Scheduling> objCalenderViewScheduling = new List<CalenderView.Scheduling>();
                foreach (MovieTimings obj in objMovieTimings)
                {
                    objCalenderViewScheduling.Add(new CalenderView.Scheduling()
                    {
                        SchedulId = obj.Id,
                        StartTime = obj.ShowDateTime,
                        Details = obj.MovieName,
                        Subject = obj.MovieTime

                    });
                }
                SchedulCalendar.MonthAppointments = objCalenderViewScheduling;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void LoadHoursAndMinuts(int hour)
        {
            try
            {
                List<ComboBoxItem> ObjHours = new List<ComboBoxItem>();
                foreach (int index in Enumerable.Range(hour, 24 - hour).ToList())
                {
                    if (index >= Convert.ToInt32(ConfigurationSettings.AppSettings["CalenderDuration"]))
                        ObjHours.Add(new ComboBoxItem() { Content = index });
                }
                cmbHours.ItemsSource = ObjHours;
                cmbHours.SelectedIndex = 0;

                List<ComboBoxItem> ObjMinutes = new List<ComboBoxItem>();
                foreach (int index in Enumerable.Range(0, 59).ToList())
                {
                    if (index % 5 == 0)
                        ObjMinutes.Add(new ComboBoxItem() { Content = (index == 0 ? "00" : (index == 5 ? "05" : Convert.ToString(index))) });
                }

                cmbMinuts.ItemsSource = ObjMinutes;
                cmbMinuts.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void ShowControlsVisibility(bool newMovietimingVisibility)
        {
            ctrlsGrid.Visibility = newMovietimingVisibility ? Visibility.Visible : Visibility.Collapsed;
            SchedulCalendar.Visibility = newMovietimingVisibility ? Visibility.Collapsed : Visibility.Visible;
        }
        #endregion

        #region Properties
        private ObservableCollection<TicketPricesAndTaxes> ticketPricesAndTaxes;

        public ObservableCollection<TicketPricesAndTaxes> TicketPricesAndTaxes
        {
            get
            {
                return ticketPricesAndTaxes;
            }
            set
            {
                ticketPricesAndTaxes = value;
            }
        }

        private ObservableCollection<Vendors> seatTemplates;
        public ObservableCollection<Vendors> SeatTemplates
        {
            get
            {
                return seatTemplates;
            }
            set
            {
                seatTemplates = value;
                NotifyPropertyChanged("SeatTemplates");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion

        private void btnAddTime_Click(object sender, RoutedEventArgs e)
        {
            if (filterDates())
            {
                ModernDialog.ShowMessage(string.Format("Already show is scheduled. Minimum you should maintain {0} Min gap", Convert.ToInt32(ConfigurationSettings.AppSettings["MinShowTime"])), "Alert", MessageBoxButton.OK);
                return;
            }

            string resultText = (!string.IsNullOrEmpty(txtShowTimes.Text) ? txtShowTimes.Text + ", " : "") + string.Format("{0}:{1}", (cmbHours.SelectedItem as ComboBoxItem).Content, (cmbMinuts.SelectedItem as ComboBoxItem).Content) + ", ";

            txtShowTimes.Text = resultText.Trim().TrimEnd(',');
        }

        private bool filterDates()
        {
            bool isAleardyExists = false;

            if (!string.IsNullOrEmpty(txtShowTimes.Text))
            {
                int[] showTimes = new int[txtShowTimes.Text.Split(',').Count() + 1];
                int index = 0;

                foreach (string overAllTime in txtShowTimes.Text.Split(','))
                {
                    int hours = Convert.ToInt32(overAllTime.Split(':')[0]);
                    showTimes[index] = hours * 60 + Convert.ToInt32(overAllTime.Split(':')[1]);

                    int currentTime = Convert.ToInt32((cmbHours.SelectedItem as ComboBoxItem).Content) * 60 + Convert.ToInt32((cmbMinuts.SelectedItem as ComboBoxItem).Content);
                    if (showTimes.Where(x => x >= currentTime && x <= currentTime + Convert.ToInt32(ConfigurationSettings.AppSettings["MinShowTime"])).Count() > 0
                        || showTimes.Where(x => x <= currentTime && x >= currentTime - Convert.ToInt32(ConfigurationSettings.AppSettings["MinShowTime"])).Count() > 0)
                    {
                        isAleardyExists = true;
                        break;
                    }
                }
            }
            return isAleardyExists;
        }

        private void btnTimeClear_Click(object sender, RoutedEventArgs e)
        {
            txtShowTimes.Text = string.Empty;
        }
    }
}