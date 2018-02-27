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
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace BoxOfficeUI.SeatLayout
{
    /// <summary>
    /// Interaction logic for LayoutDesign.xaml
    /// </summary>
    public partial class HoldTray : UserControl
    {
        #region Loading Page & DropDowns
        public HoldTray()
        {
            try
            {
                InitializeComponent();

                dprStartingDate.DisplayDateStart = DateTime.Now;

                dprStartingDate.DisplayDateEnd = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationSettings.AppSettings["CalenderDuration"]));

                foreach (int index in Enumerable.Range(1, Convert.ToInt32(ConfigurationSettings.AppSettings["MaxBookingSeats"])).ToList())
                {
                    cmbSeatsCount.Items.Add(new ListBoxItem() { Content = index });
                }
                cmbSeatsCount.SelectedIndex = 0;

                dprStartingDate.SelectedDate = DateTime.Now;
                dprStartingDate.SelectedDateChanged += dprStartingDate_SelectedDateChanged;

                LoadMovies();
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void dprStartingDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadMovies();
        }

        private void LoadMovies()
        {
            try
            {
                ManageLoadingDataVisibility(true);
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.RunWorkerAsync(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DateTime dtSelectedDate = DateTime.Now;

                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => dtSelectedDate = Convert.ToDateTime(dprStartingDate.SelectedDate)));

                IEnumerable<Movie> movies = new MovieTimingsRepository().GetMovies(dtSelectedDate);

                if (movies == null || movies.Count() == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => ManageLoadingDataVisibility(false)));

                    return;
                }

                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => Helper.LoadDropDownSource(cmbMovie, movies, "MovieName", "Id")));
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }

        private void cmbMovie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ManageLoadingDataVisibility(true);

                IEnumerable<Movie> movieTimes = new MovieTimingsRepository().GetMovieDates(Convert.ToInt32(cmbMovie.SelectedValue), Convert.ToDateTime(dprStartingDate.SelectedDate));

                if (movieTimes == null || movieTimes.Count() == 0)
                {
                    ManageLoadingDataVisibility(false);

                    return;
                }

                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => Helper.LoadDropDownSource(cmbMovieTime, movieTimes, "S_Date", "S_Date")));
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }

        private void cmbMovieTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ManageLoadingDataVisibility(true);

                IEnumerable<Screen> screens = new SeatLayoutConfig().GetScreens(Convert.ToDateTime(dprStartingDate.SelectedDate), Convert.ToInt32((cmbMovieTime.SelectedItem as Movie).Id));

                if (screens == null || screens.Count() == 0)
                {
                    ManageLoadingDataVisibility(false);
                    return;
                }

                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => Helper.LoadDropDownSource(cmbScreen, screens, "ScreenName", "Id")));
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }

        private void cmbScreen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbScreen.SelectedValue == null)
                    return;

                ManageLoadingDataVisibility(true);
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => GeneratingLayout()));
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }

        private void btnminus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dprStartingDate.SelectedDate = dprStartingDate.SelectedDate.Value.AddDays(-1);
                DateTime dtr = new DateTime(Convert.ToDateTime(dprStartingDate.SelectedDate.Value.Date).Year, Convert.ToDateTime(dprStartingDate.SelectedDate.Value.Date).Month, Convert.ToDateTime(dprStartingDate.SelectedDate.Value.Date).Day);

                btnminus.IsEnabled = (dtr <= DateTime.Now) ? false : true;
                btnplus.IsEnabled = true;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void btnplus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dprStartingDate.SelectedDate = dprStartingDate.SelectedDate.Value.AddDays(1);
                DateTime dtr = new DateTime(Convert.ToDateTime(dprStartingDate.DisplayDateEnd.Value.Date).Year, Convert.ToDateTime(dprStartingDate.DisplayDateEnd.Value.Date).Month, Convert.ToDateTime(dprStartingDate.DisplayDateEnd.Value.Date).Day);

                btnminus.IsEnabled = true;
                btnplus.IsEnabled = (dtr <= dprStartingDate.SelectedDate) ? false : true;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }
        #endregion

        #region Page events
        private void tglSeat_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                ToggleButton objCurrentCheckbox = sender as ToggleButton;
                SeatProperties objSeat = objCurrentCheckbox.Tag as SeatProperties;

                int i = Convert.ToInt32((cmbSeatsCount.SelectedValue as ListBoxItem).Content);

                if (Seats.Where(w => w.Row == objSeat.Row && w.ScreenClassId == objSeat.ScreenClassId && w.Id != 0 && w.IsEnabled).Count() < i)
                {
                    ModernDialog.ShowMessage(string.Format("There are few seats avalible in this Class, Less than {0} so you can't continue booking.", i), "Error", MessageBoxButton.OK);
                    objSeat.IsChecked = false;
                    return;
                }

                if ((objSeat.IsChecked || objSeat.Id == Seats.Where(w => w.Row == objSeat.Row && w.ScreenClassId == objSeat.ScreenClassId && w.Id != 0).Min(w => w.Id)) && (Seats.Where(w => w.ScreenClassId == objSeat.ScreenClassId && w.Id != objSeat.Id && w.IsChecked).Count() == i || Seats.Where(w => w.ScreenClassId == objSeat.ScreenClassId && w.Id != objSeat.Id && w.IsChecked && w.IsEnabled).Count() == 0))
                {
                    Seats.Where(w => w.Id != objSeat.Id && w.IsChecked && w.IsEnabled).ToList<SeatProperties>().ForEach(x =>
                    {
                        x.IsChecked = false;
                    });
                }

                if (Seats.Where(x => x.IsEnabled && x.IsChecked && x.Id != objSeat.Id).Count() == 0 && objSeat.IsChecked)
                {
                    foreach (SeatProperties obj in Seats.Where(w => w.Row == objSeat.Row && w.ScreenClassId == objSeat.ScreenClassId && w.IsEnabled && !w.IsChecked && !string.IsNullOrEmpty(w.DisplayText) && w.DisplayText != "999" && w.Column >= objSeat.Column).ToList().OrderBy(w => w.Column))
                    {
                        i--;

                        if (i == 0)
                            break;

                        obj.IsChecked = objSeat.IsChecked;
                    }
                }
                else if (Seats.Where(x => x.IsEnabled && x.IsChecked && x.Id != objSeat.Id).Count() == i)
                {
                    objSeat.IsChecked = false;
                }

                if (objSeat.Column == 0)
                {
                    Seats.Where(w => w.Row == objSeat.Row && w.ScreenClassId == objSeat.ScreenClassId).ToList<SeatProperties>().ForEach(x =>
                    {
                        x.IsChecked = objSeat.IsChecked;
                    });
                }
                else if (!objSeat.IsChecked)
                {
                    Seats.Where(w => w.Row == objSeat.Row && w.ScreenClassId == objSeat.ScreenClassId && w.Column == 0).FirstOrDefault().IsChecked = false;
                }
                else if (Seats.Where(w => w.Row == objSeat.Row && w.ScreenClassId == objSeat.ScreenClassId && w.Column != 0).Count() == Seats.Where(w => w.Row == objSeat.Row && w.ScreenClassId == objSeat.ScreenClassId && w.IsChecked).Count())
                {
                    Seats.Where(w => w.Row == objSeat.Row && w.ScreenClassId == objSeat.ScreenClassId && w.Column == 0).FirstOrDefault().IsChecked = true;
                }

                btnBlockSeats.IsEnabled =
                btnClearSeats.IsEnabled = Seats.Where(w => w.IsEnabled && w.IsChecked).Count() > 0 ? true : false;
                lblAmount.Content = Seats.Where(x => x.IsEnabled && x.IsChecked).Count() > 0 ? string.Format("Amount - {0}", Convert.ToString(Seats.Where(x => x.IsEnabled && x.IsChecked).ToList().Sum(x => x.TicketCost))) : string.Empty;
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void btnBookSeats_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {

                ManageLoadingDataVisibility(true);

                if (Seats.Where(x => x.IsEnabled && x.IsChecked).ToList().Count() == 0)
                {
                    ModernDialog.ShowMessage("Please Select seats", "Alert", MessageBoxButton.OK);
                }
                else if (Seats.Where(x => x.IsEnabled && x.IsChecked).ToList().Count() > Convert.ToInt32(ConfigurationSettings.AppSettings["MaxBookingSeats"]))
                {
                    ModernDialog.ShowMessage(string.Format("You can select Maximum {0} seats only", Convert.ToInt32(ConfigurationSettings.AppSettings["MaxBookingSeats"])), "Alert", MessageBoxButton.OK);
                }
                else if (Seats.Where(x => x.IsEnabled && x.IsChecked).ToList().GroupBy(l => l.ScreenClassId).Count() > 1)
                {
                    ModernDialog.ShowMessage("Please Book tickets from Only one Class, You Can't book tickets from multiple classes", "Alert", MessageBoxButton.OK);
                }
                else if (Seats.Where(x => x.IsEnabled && x.IsChecked).ToList().Count() != Convert.ToInt32((cmbSeatsCount.SelectedValue as ListBoxItem).Content))
                {
                    ModernDialog.ShowMessage("Required seats and selected seats count is not matched.", "Alert", MessageBoxButton.OK);
                }
                else if (ModernDialog.ShowMessage("Are you sure, you want to Hold selected Seat's ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var selectedSeats = Seats.Where(x => x.IsEnabled && x.IsChecked).ToList();
                    var seats = string.Join(",", selectedSeats.Select(x => x.Id));

                    var result = new MovieTimingsRepository().InsertMovieBooking(Convert.ToInt32(cmbMovie.SelectedValue), selectedSeats.Count(), Convert.ToDouble(Seats.Where(x => x.IsEnabled && x.IsChecked).ToList().Sum(x => x.TicketCost)), selectedSeats.FirstOrDefault().ScreenClassId, Convert.ToInt32(cmbScreen.SelectedValue), Convert.ToInt32((cmbMovieTime.SelectedItem as Movie).Id), seats);

                    if (result == 0)
                    {
                        ModernDialog.ShowMessage("Seats Holding failed..", "Alert", MessageBoxButton.OK);
                    }
                    else
                    {
                        ModernDialog.ShowMessage("Selected Seats Holded successfully.", "Alert", MessageBoxButton.OK);

                        GenerateLayout();
                        ClearSelection();
                    }
                   
                }

                ManageLoadingDataVisibility(false);
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }
        #endregion

        #region Loading Seat Layout
        private void GenerateLayout()
        {
            try
            {
                Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() => ManageLoadingDataVisibility(true)));

                Seats = new ObservableCollection<SeatProperties>();

                BackgroundWorker worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                worker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWork);
                worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    GeneratingLayout();
                });
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ManageLoadingDataVisibility(false);
        }

        private void GeneratingLayout()
        {
            try
            {
                Seats = new ObservableCollection<SeatProperties>();
                IEnumerable<Seat> objScreenSeats = new SeatLayoutConfig().GetScreeSeats(Convert.ToInt32(cmbScreen.SelectedValue), 0, (cmbMovieTime.SelectedItem as Movie).Id);

                int columns = objScreenSeats.ToList<Seat>().Max(w => w.ColumnValue);

                var groupList = objScreenSeats.ToList<Seat>().GroupBy(l => l.ScreenClassId)
                             .Select(cl => new Seat
                             {
                                 ScreenClassId = cl.First().ScreenClassId,
                                 RowValue = cl.Max(c => c.RowValue),
                                 ColumnValue = cl.Max(c => c.ColumnValue),
                                 DisplayOrder = cl.First().DisplayOrder,
                                 ScreenClass = cl.First().ScreenClass
                             }).ToList();

                int rowsforAllClasses = groupList.Sum(w => w.RowValue);
                foreach (Seat objscreenClass in groupList.OrderBy(w => w.DisplayOrder))
                {
                    int rows = objscreenClass.RowValue;
                    IEnumerable<Seat> objSubClassSeatsList = objScreenSeats.Where(w => w.ScreenClassId == objscreenClass.ScreenClassId);

                    Seats.Add(new SeatProperties()
                    {
                        CheckboxVisibility = Visibility.Visible,
                        Column = objscreenClass.ScreenClassId,
                        ColumnText = Convert.ToString(objscreenClass.ScreenClassId),
                        DisplayText = objscreenClass.ScreenClass,
                        Id = 0,
                        IsChecked = false,
                        IsEnabled = false,
                        Row = objscreenClass.ScreenClassId,
                        RowNum = objscreenClass.ScreenClassId,
                        RowText = Convert.ToString(objscreenClass.ScreenClassId),
                        ScreenClassId = objSubClassSeatsList.FirstOrDefault().ScreenClassId,
                        SeatColor = Helper.ReturnColor("INACTIVE"),
                        SeatHeight = 25,
                        SeatOrientation = Orientation.Horizontal,
                        SeatWidth = (25 * columns) + 50,
                        TextVisibility = Visibility.Visible,
                        VendorId = objscreenClass.VendorId
                    });

                    for (int row = 1; row <= rows; row++)
                    {
                        GenerateRowNameColumn(objSubClassSeatsList.Where(w => w.RowValue == row).FirstOrDefault(), true, false);
                        foreach (Seat objSeat in objSubClassSeatsList.Where(w => w.RowValue == row).OrderBy(w => w.ColumnValue))
                        {
                            GenerateRowNameColumn(objSeat, false, objSeat.IsReserved != 1);
                        }

                        int currentRowColumns = objSubClassSeatsList.ToList<Seat>().Where(w => w.RowValue == row).Count() > 0 ? objSubClassSeatsList.ToList<Seat>().Where(w => w.RowValue == row).Count() : 0;

                        if (groupList.Where(w => w.ColumnValue == columns).Count() != groupList.Count())
                            currentRowColumns = currentRowColumns - 1;

                        if (currentRowColumns > 0 && currentRowColumns < columns)
                        {
                            for (int i = currentRowColumns; i <= columns; i++)
                            {
                                GenerateRowNameColumn(new Seat(), false, false);
                            }
                        }
                    }
                }

                foreach (Seat objSeat in groupList)
                {
                    SeatProperties objGroupSeat = Seats.Where(x => x.Id == 0 && x.ScreenClassId == objSeat.ScreenClassId).FirstOrDefault();

                    objGroupSeat.DisplayText = string.Format("{0} - {1}", objSeat.ScreenClass, Seats.Where(x => x.Id != 0 && x.ScreenClassId == objSeat.ScreenClassId).FirstOrDefault().TicketCost);
                }

                itmSeats.Width = (25 * (columns + (groupList.Count > 1 ? 2 : 1)));
                itmSeats.Height = (30 * (rowsforAllClasses + groupList.Count()));
                itmSeats.ItemsSource = Seats;

                ManageLoadingDataVisibility(false);
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }

        private void GenerateRowNameColumn(Seat objSeat, bool isSelectAllColumn, bool isEnable = true)
        {
            try
            {
                Seats.Add(new SeatProperties()
                {
                    CheckboxVisibility = isSelectAllColumn ? Visibility.Visible : (objSeat.FK_SeatStatus_ID != 0 ? Visibility.Visible : Visibility.Collapsed),
                    Column = isSelectAllColumn ? 0 : objSeat.ColumnValue,
                    ColumnText = isSelectAllColumn ? Convert.ToString(0) : (!string.IsNullOrEmpty(objSeat.RowText) ? Convert.ToString(objSeat.ColumnText) : string.Empty),
                    DisplayText = isSelectAllColumn ? Convert.ToString(objSeat.RowText) : (!string.IsNullOrEmpty(objSeat.RowText) ? Convert.ToString(objSeat.ColumnText) : string.Empty),
                    Id = objSeat.ID,
                    IsChecked = !isSelectAllColumn && objSeat.IsReserved == 1,
                    IsEnabled = isEnable,
                    Row = objSeat.RowValue,
                    RowNum = objSeat.RowValue,
                    RowText = Convert.ToString(objSeat.RowText),
                    ScreenClassId = objSeat.ScreenClassId,
                    SeatColor = Helper.ReturnColor(objSeat.Color),
                    SeatHeight = 25,
                    SeatOrientation = Orientation.Horizontal,
                    SeatWidth = isSelectAllColumn ? 50 : 20,
                    TextVisibility = Visibility.Visible,
                    TicketCost = objSeat.Price,
                    VendorId = objSeat.VendorId
                });
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void ManageLoadingDataVisibility(bool visibile)
        {
            this.IsHitTestVisible = visibile ? false : true;
            pgrProgress.Visibility = visibile ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

        #region Properties
        private ObservableCollection<SeatProperties> Seats { get; set; }

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

        private void btnClearSeats_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
        }

        private void ClearSelection()
        {
            Seats.Where(w => w.IsChecked && w.IsEnabled).ToList<SeatProperties>().ForEach(x =>
            {
                x.IsChecked = false;
            });

            btnBlockSeats.IsEnabled =
                btnClearSeats.IsEnabled = Seats.Where(w => w.IsEnabled && w.IsChecked).Count() > 0 ? true : false;

            lblAmount.Content = "Amount : 0";
        }
        #endregion
    }
}