using BoxOffice.DAL;
using BoxOffice.Model;
using BoxOfficeUI.Util;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace BoxOfficeUI.SeatLayout
{
    /// <summary>
    /// Interaction logic for LayoutDesign.xaml
    /// </summary>
    public partial class Booking : UserControl
    {
        #region Loading Page & DropDowns
        public Booking()
        {
            try
            {
                InitializeComponent();

                dprStartingDate.DisplayDateStart = DateTime.Now;

                dprStartingDate.DisplayDateEnd = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationSettings.AppSettings["CalenderDuration"]));

                foreach (int index in Enumerable.Range(1, Convert.ToInt32(ConfigurationSettings.AppSettings["MaxBookingSeats"])).ToList())
                {
                    lstSeats.Items.Add(new ListBoxItem() { Content = index });
                }
                lstSeats.SelectedIndex = 0;

                dprStartingDate.SelectedDate = DateTime.Now;
                dprStartingDate.SelectedDateChanged += dprStartingDate_SelectedDateChanged;

                ManageLoadingDataVisibility(true);
                LoadMovies();

                //txbBookedSeats.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(Helper.ReturnColor("RESERVED")));
                //txbAvalibleSeats.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom(Helper.ReturnColor("ACTIVE")));
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
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => ManageLoadingDataVisibility(false, false, false, false, false)));
                    return;
                }

                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => Helper.LoadDropDownSource(lstMovie, movies, "", "")));
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }

        private void lstMovie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ManageLoadingDataVisibility(true);

                IEnumerable<Movie> movieTimes = new MovieTimingsRepository().GetMovieDates(Convert.ToInt32(lstMovie.SelectedValue), Convert.ToDateTime(dprStartingDate.SelectedDate));

                if (movieTimes == null || movieTimes.Count() == 0)
                {
                    ManageLoadingDataVisibility(false, false, false, false);

                    return;
                }

                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => Helper.LoadDropDownSource(lstTimes, movieTimes, "", "")));
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }

        private void lstTimes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ManageLoadingDataVisibility(true);

                IEnumerable<Screen> screens = new SeatLayoutConfig().GetScreens(Convert.ToDateTime(dprStartingDate.SelectedDate), Convert.ToInt32((lstTimes.SelectedItem as Movie).Id));

                if (screens == null || screens.Count() == 0)
                {
                    ManageLoadingDataVisibility(false, false);
                    return;
                }

                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => Helper.LoadDropDownSource(lstScreen, screens, "", "")));
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }

        private void lstScreen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstScreen.SelectedValue == null)
                    return;

                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => txbScreen.Text = (lstScreen.SelectedItem as Screen).ScreenName));

                ManageLoadingDataVisibility(true);
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => GeneratingLayout(Convert.ToInt32(lstScreen.SelectedValue))));
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

                int i = Convert.ToInt32((lstSeats.SelectedValue as ListBoxItem).Content);

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

                txbAmount.Text = Convert.ToString(Seats.Where(x => x.IsEnabled && x.IsChecked).ToList().Sum(x => x.TicketCost));

                string tickets = string.Empty; string screenClass = string.Empty;
                foreach (SeatProperties objSeatProperties in Seats.Where(x => x.IsEnabled && x.IsChecked).OrderBy(x => x.Row).ThenBy(x => x.Column))
                {
                    screenClass = objSeatProperties.ScreenClass;
                    tickets = tickets + string.Format("{0} {1}, ", objSeatProperties.RowText, objSeatProperties.ColumnText);
                }
                txbTotalAmount.Text = screenClass + " - " + tickets.Trim().TrimEnd(',') + " Total - ";

                stkBooking.Visibility = (Seats.Where(x => x.IsEnabled && x.IsChecked).Count() > 0) ? Visibility.Visible : Visibility.Collapsed;
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
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => ManageLoadingDataVisibility(true)));
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
                else if (Seats.Where(x => x.IsEnabled && x.IsChecked).ToList().Count() != Convert.ToInt32((lstSeats.SelectedValue as ListBoxItem).Content))
                {
                    ModernDialog.ShowMessage("Required seats and selected seats count is not matched.", "Alert", MessageBoxButton.OK);
                }
                else
                {
                    //if (ModernDialog.ShowMessage("Are you sure you want to continue booking?", string.Format("Confirmation"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    //{
                    var selectedSeats = Seats.Where(x => x.IsEnabled && x.IsChecked).ToList();
                    var seats = string.Join(",", selectedSeats.Select(x => x.Id));

                    var result = new MovieTimingsRepository().InsertMovieBooking(Convert.ToInt32(lstMovie.SelectedValue), selectedSeats.Count(), Convert.ToDouble(Seats.Where(x => x.IsEnabled && x.IsChecked).ToList().Sum(x => x.TicketCost)), selectedSeats.FirstOrDefault().ScreenClassId, Convert.ToInt32(lstScreen.SelectedValue), Convert.ToInt32((lstTimes.SelectedItem as Movie).Id), seats);

                    if (result == 0)
                    {
                        if (ModernDialog.ShowMessage("Tickets already reserved, Please reload and try another seats..", "Alert", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            LoadBookedTickets();
                        }
                    }
                    else
                    {
                        order_Id = result;
                        PrintDocument pd = new PrintDocument();
                        PaperSize ps = new PaperSize("", 475, 550);

                        pd.PrintPage += new PrintPageEventHandler(ticket_PrintPage);

                        pd.PrintController = new StandardPrintController();
                        pd.DefaultPageSettings.Margins.Left = 0;
                        pd.DefaultPageSettings.Margins.Right = 0;
                        pd.DefaultPageSettings.Margins.Top = 0;
                        pd.DefaultPageSettings.Margins.Bottom = 0;
                        
                        pd.DefaultPageSettings.PaperSize = ps;
                        pd.Print();

                        //ModernDialog.ShowMessage("Tickets booking confired successfully", "Alert", MessageBoxButton.OK);
                        LoadBookedTickets();
                        stkBooking.Visibility = Visibility.Collapsed;
                    }
                    //}
                }

                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => ManageLoadingDataVisibility(false)));
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }
        #endregion

        #region Loading Seat Layout
        private void GeneratingLayout(int screenId)
        {
            try
            {
                Seats = new ObservableCollection<SeatProperties>();

                IEnumerable<Seat> objScreenSeats = Helper.GetScreenSeats(screenId);
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
                        ScreenClass = objscreenClass.ScreenClass,
                        SeatColor = Helper.ReturnColor("INACTIVE"),
                        SeatHeight = 25,
                        SeatOrientation = Orientation.Horizontal,
                        SeatWidth = (25 * columns) + 80,
                        TextVisibility = Visibility.Visible,
                        VendorId = objscreenClass.VendorId
                    });

                    for (int row = 1; row <= rows; row++)
                    {
                        GenerateRowNameColumn(objSubClassSeatsList.Where(w => w.RowValue == row).FirstOrDefault(), true, false);
                        foreach (Seat objSeat in objSubClassSeatsList.Where(w => w.RowValue == row).OrderBy(w => w.ColumnValue))
                        {
                            GenerateRowNameColumn(objSeat, false, objSeat.VendorId == 0 ? true : false);
                        }

                        int currentRowColumns = objSubClassSeatsList.ToList<Seat>().Where(w => w.RowValue == row).Count() > 0 ? objSubClassSeatsList.ToList<Seat>().Where(w => w.RowValue == row).Count() : 0;

                        if (groupList.Where(w => w.ColumnValue == columns).Count() != groupList.Count())
                            currentRowColumns = currentRowColumns - 1;

                        if (currentRowColumns > 0 && currentRowColumns < columns)
                        {
                            for (int i = currentRowColumns; i <= columns; i++)
                            {
                                GenerateRowNameColumn(new Seat(), false);
                            }
                        }
                    }
                }

                itmSeats.Width = (25 * (columns + 2)) + 30;
                itmSeats.Height = (30 * (rowsforAllClasses + groupList.Count()));
                itmSeats.ItemsSource = Seats;

                LoadBookedTickets();
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
                    Id = isSelectAllColumn ? 0 : objSeat.ID,
                    IsChecked = (!isSelectAllColumn && objSeat.VendorId != 0) ? true : false,
                    IsEnabled = isSelectAllColumn ? false : (isEnable ? true : false),
                    Row = objSeat.RowValue,
                    RowNum = objSeat.RowValue,
                    RowText = Convert.ToString(objSeat.RowText),
                    ScreenClassId = objSeat.ScreenClassId,
                    ScreenClass = objSeat.ScreenClass,
                    SeatColor = Helper.ReturnColor("ACTIVE"),
                    SeatHeight = 25,
                    SeatOrientation = Orientation.Horizontal,
                    SeatWidth = isSelectAllColumn ? 80 : 20,
                    TextVisibility = Visibility.Visible,
                    VendorId = objSeat.VendorId
                });
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void ManageLoadingDataVisibility(bool visibile, bool showLayout = true, bool showTimes = true, bool showScreens = true, bool showMovies = true)
        {
            try
            {
                itmSeats.Visibility = !showLayout ? Visibility.Collapsed : Visibility.Visible;
                this.IsHitTestVisible = visibile ? false : true;
                pgrProgress.Visibility = visibile ? Visibility.Visible : Visibility.Collapsed;
                lstScreen.Visibility = showScreens ? Visibility.Visible : Visibility.Collapsed;
                lstTimes.Visibility = showTimes ? Visibility.Visible : Visibility.Collapsed;
                lstMovie.Visibility = showMovies ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }

        private void LoadBookedTickets()
        {
            try
            {
                if (lstTimes.SelectedItem == null)
                    return;

                IEnumerable<Seat> objScreenSeats = new SeatLayoutConfig().GetShowTickets(Convert.ToInt32(lstScreen.SelectedValue), Convert.ToInt32((lstTimes.SelectedItem as Movie).Id));
                foreach (Seat obj in objScreenSeats)
                {
                    Seats.Where(x => x.Id == obj.ID).ToList<SeatProperties>().ForEach(w =>
                    {
                        w.IsEnabled = obj.Is_Enabled;
                        w.TicketCost = obj.Price;
                        w.IsChecked = !obj.Is_Enabled;
                        w.SeatColor = Helper.ReturnColor(obj.Color);
                    });
                }

                var groupList = Seats.Where(l => l.ScreenClassId != 0).GroupBy(l => l.ScreenClassId)
                             .Select(cl => new Seat
                             {
                                 ScreenClassId = cl.First().ScreenClassId,
                                 ScreenClass = cl.First().ScreenClass,
                                 Price = cl.First().TicketCost
                             }).ToList();

                brdClass2.Visibility =
                brdClass3.Visibility = txbClass2.Visibility = txbClass3.Visibility =  Visibility.Collapsed;

                int i = 0;

                foreach (Seat objSeat in groupList)
                {
                    if (i == 0)
                    {
                        txbClass1.Text = string.Format("{0}  -  {1} Sold ({2} Rs), {3} Available", objSeat.ScreenClass, objScreenSeats.Where(w => w.ScreenClassId == objSeat.ScreenClassId && w.Color == "RESERVED").Count(), objScreenSeats.Where(w => w.ScreenClassId == objSeat.ScreenClassId && w.Color == "RESERVED").Sum(w => w.Price), objScreenSeats.Where(w => w.ScreenClassId == objSeat.ScreenClassId && w.Color == "ACTIVE").Count());
                    }
                    else if (i == 1)
                    {
                        txbClass2.Visibility = 
                        brdClass2.Visibility = Visibility.Visible;

                        txbClass2.Text = string.Format("{0}  -  {1} Sold ({2} Rs), {3} Available", objSeat.ScreenClass, objScreenSeats.Where(w => w.ScreenClassId == objSeat.ScreenClassId && w.Color == "RESERVED").Count(), objScreenSeats.Where(w => w.ScreenClassId == objSeat.ScreenClassId && w.Color == "RESERVED").Sum(w => w.Price), objScreenSeats.Where(w => w.ScreenClassId == objSeat.ScreenClassId && w.Color == "ACTIVE").Count());
                    }
                    else if (i == 2)
                    {
                        txbClass3.Visibility =
                        brdClass3.Visibility = Visibility.Visible;
                        txbClass3.Text = string.Format("{0}  -  {1} Sold ({2} Rs), {3} Available", objSeat.ScreenClass, objScreenSeats.Where(w => w.ScreenClassId == objSeat.ScreenClassId && w.Color == "RESERVED").Count(), objScreenSeats.Where(w => w.ScreenClassId == objSeat.ScreenClassId && w.Color == "RESERVED").Sum(w => w.Price), objScreenSeats.Where(w => w.ScreenClassId == objSeat.ScreenClassId && w.Color == "ACTIVE").Count());
                    }

                    SeatProperties objGroupSeat = Seats.Where(x => x.Id == 0 && x.ScreenClassId == objSeat.ScreenClassId).FirstOrDefault();

                    objGroupSeat.DisplayText = string.Format("{0} - {1}", objSeat.ScreenClass, Seats.Where(x => x.Id != 0 && x.CheckboxVisibility == Visibility.Visible && x.ScreenClassId == objSeat.ScreenClassId).FirstOrDefault().TicketCost);

                    i++;
                }
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }
        #endregion

        #region Properties
        private int order_Id = 0;

        private ObservableCollection<SeatProperties> Seats { get; set; }
        #endregion

        private void btnClearSeats_Click(object sender, RoutedEventArgs e)
        {
            Seats.Where(w => w.IsChecked && w.IsEnabled).ToList<SeatProperties>().ForEach(x =>
            {
                x.IsChecked = false;
            });

            stkBooking.Visibility = Visibility.Collapsed;
        }

        private void ticket_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                double singleTicketPrice = Seats.Where(x => x.IsEnabled && x.IsChecked).OrderBy(x => x.Column).FirstOrDefault().TicketCost;
                double totalTickes = Seats.Where(x => x.IsEnabled && x.IsChecked).Count();

                int SPACE = 0;

                Graphics g = e.Graphics;
                Font fHeader = new Font("Lucida Console", 10, System.Drawing.FontStyle.Bold);
                Font fHeader1 = new Font("Lucida Console", 9, System.Drawing.FontStyle.Bold);
                Font fBody = new Font("Lucida Console", 8, System.Drawing.FontStyle.Bold);
                Font fBody1 = new Font("Lucida Console", 8, System.Drawing.FontStyle.Bold);
                Font ffoter = new Font("Lucida Console", 8, System.Drawing.FontStyle.Regular);

                Font rs = new Font("Stencil", 25, System.Drawing.FontStyle.Bold);
                Font fTType = new Font("", 150, System.Drawing.FontStyle.Bold);
                SolidBrush sb = new SolidBrush(System.Drawing.Color.Black);

                g.DrawString(Helper.Theater, fHeader, sb, 10, 0);
                g.DrawString((lstMovie.SelectedItem as Movie).MovieName, fBody, sb, 10, SPACE + 20);

                g.DrawString(string.Format("Screen : {0}", (lstScreen.SelectedItem as Screen).ScreenName), fBody, sb, 10, SPACE + 35);
                g.DrawString(string.Format("Time : {0} Price : {1}", (lstTimes.SelectedItem as Movie).S_Date.Trim(), totalTickes * singleTicketPrice), fBody, sb, 10, SPACE + 50);

                g.DrawString(string.Format("Show Date : {0}", dprStartingDate.SelectedDate.Value.ToShortDateString()), fBody, sb, 10, SPACE + 65);

                string tickets = string.Empty; string screenClass = string.Empty;
                foreach (SeatProperties objSeatProperties in Seats.Where(x => x.IsEnabled && x.IsChecked).OrderBy(x => x.Column))
                {
                    screenClass = objSeatProperties.ScreenClass;
                    tickets = tickets + string.Format("{0}-{1},", objSeatProperties.RowText, objSeatProperties.ColumnText);
                }

                g.DrawString(string.Format("{0} : {1}", screenClass, totalTickes), fHeader, sb, 10, SPACE + 80);

                g.DrawString(string.Format("{0}", tickets.Trim().TrimEnd(',')), fBody1, sb, 10, SPACE + 100);

                g.DrawString(string.Format("OrderId - {0}, Time - {1}", order_Id, DateTime.Now.ToString("dd/MM/yyyy hh:mm tt")), ffoter, sb, 10, SPACE + 115);

                g.DrawString(string.Format("Cashier : {0}", Helper.UserName), ffoter, sb, 10, SPACE + 130);

                g.DrawString("Powered by BookMyShow", ffoter, sb, 10, SPACE + 145);

                g.DrawString("-----------------------------------", ffoter, sb, 10, SPACE + 155);

                g.DrawString(Helper.Theater, fHeader1, sb, 10, SPACE + 170);
                g.DrawString((lstMovie.SelectedItem as Movie).MovieName, fBody, sb, 10, SPACE + 185);

                g.DrawString(string.Format("Time : {0} Price : {1}", (lstTimes.SelectedItem as Movie).S_Date.Trim(), totalTickes * singleTicketPrice), ffoter, sb, 10, SPACE + 200);

                g.DrawString(string.Format("Show Date : {0}", dprStartingDate.SelectedDate.Value.ToShortDateString()), ffoter, sb, 10, SPACE + 215);

                g.DrawString(string.Format("{0} : {1} OrderId : {2}", screenClass, totalTickes, order_Id), fBody, sb, 10, SPACE + 230);

                g.DrawString(string.Format("{0}", tickets.Trim().TrimEnd(',')), fBody1, sb, 10, SPACE + 245);
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false);
                LogExceptions.LogException(ex);
            }
        }
    }
}