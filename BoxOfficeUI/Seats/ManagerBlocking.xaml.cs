using BoxOffice.DAL;
using BoxOffice.Model;
using BoxOfficeUI.Util;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace BoxOfficeUI.SeatLayout
{
    /// <summary>
    /// Interaction logic for LayoutDesign.xaml
    /// </summary>
    public partial class ManagerBlocking : UserControl, INotifyPropertyChanged
    {
        #region Loading Page & DropDowns
        public ManagerBlocking()
        {
            InitializeComponent();

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerAsync(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() => LoadDropdownData()));
        }

        private void LoadDropdownData()
        {
            Helper.LoadDropDownSource(cmbScreen, new SeatLayoutConfig().GetScreens(), "ScreenName", "Id");
            Vendors = new ObservableCollection<Vendors>(new SeatLayoutConfig().GetVendors());
            Helper.LoadDropDownSource(cmbVendor, Vendors, "VendorName", "Id");
        }
        #endregion

        #region Page events
        private void cmbScreen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ManageLoadingDataVisibility(true);
            GenerateLayout();
        }

        private void cmbVendor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtTemplate.Text = (cmbVendor.SelectedValue as Vendors).TEMPLATENAME;
            txtTemplate.IsEnabled = string.IsNullOrEmpty(txtTemplate.Text) ? true : false;
            this.vendor_Id = (cmbVendor.SelectedValue as Vendors).Id;
            this.template_Id = (cmbVendor.SelectedValue as Vendors).TEMPLATE_ID;

            ManageLoadingDataVisibility(true);
            Seats.Where(w => w.VendorId > 0).ToList<SeatProperties>().ForEach(x =>
            { x.IsEnabled = false; });

            Seats.Where(w => w.VendorId == this.vendor_Id).ToList<SeatProperties>().ForEach(x =>
            { x.IsEnabled = true; });
            ManageLoadingDataVisibility(false);
        }

        private void tglSeat_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                ToggleButton objCurrentCheckbox = sender as ToggleButton;
                SeatProperties objSeat = objCurrentCheckbox.Tag as SeatProperties;
                if (objSeat.Column == 0)
                {
                    Seats.Where(w => w.Row == objSeat.Row && w.ScreenClassId == objSeat.ScreenClassId && w.IsEnabled).ToList<SeatProperties>().ForEach(x =>
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

                btnUnBlockSeats.IsEnabled = Seats.Where(w => w.VendorId == 0 && w.IsChecked).Count() > 0 ? false : true;

                btnBlockSeats.IsEnabled = Seats.Where(w => w.VendorId == this.vendor_Id && !w.IsChecked).Count() > 0 ? false : true;
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
                if (string.IsNullOrEmpty(txtTemplate.Text))
                {
                    ModernDialog.ShowMessage("Please enter Template Name", "Alert", MessageBoxButton.OK);
                    return;
                }


                ManageLoadingDataVisibility(true);
                Button btn = sender as Button;

                string selectedSeats = string.Empty;
                string unSelectedSeats = string.Empty;

                if (Convert.ToInt32(btn.Tag) == 1)
                {
                    if (Seats.Where(w => w.Column != 0 && w.IsChecked && w.VendorId == 0).Count() == 0)
                    {
                        ManageLoadingDataVisibility(false);
                        ModernDialog.ShowMessage("Please select Seats", "Alert", MessageBoxButton.OK);
                        return;
                    }

                    if (ModernDialog.ShowMessage("Are you sure, you want to block selected Seat's ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        ManageLoadingDataVisibility(false);
                        return;
                    }

                    foreach (SeatProperties sp in Seats.Where(w => w.Column != 0 && w.IsChecked && w.IsEnabled && w.VendorId == 0))
                    {
                        selectedSeats = selectedSeats + sp.Id + ",";
                    }
                }
                else
                {
                    if (Seats.Where(w => w.Column != 0 && !w.IsChecked && w.VendorId == this.vendor_Id).Count() == 0)
                    {
                        ManageLoadingDataVisibility(false);
                        ModernDialog.ShowMessage("Please un select Seats", "Alert", MessageBoxButton.OK);
                        return;
                    }

                    if (ModernDialog.ShowMessage("Are you sure, you want to unblock the un selected Seats ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        ManageLoadingDataVisibility(false);
                        return;
                    }

                    foreach (SeatProperties sp in Seats.Where(w => w.Column != 0 && !w.IsChecked && w.IsEnabled && w.VendorId == this.vendor_Id))
                    {
                        unSelectedSeats = unSelectedSeats + sp.Id + ",";
                    }
                }

                int result = new SeatTemplateRepository().insertSeatTemplate(this.template_Id, selectedSeats.Trim().TrimEnd(','), Convert.ToInt32(cmbScreen.SelectedValue), this.vendor_Id, txtTemplate.Text, unSelectedSeats);

                if (result == 100)
                {
                    txtTemplate.IsEnabled = false;

                    ModernDialog.ShowMessage((Convert.ToInt32(btn.Tag) == 1) ? "Selected Seats blocked successfully." : "Selected Seats un blocked successfully.", "Alert", MessageBoxButton.OK);

                    GenerateLayout();

                    Vendors = new ObservableCollection<Vendors>(new SeatLayoutConfig().GetVendors());

                    this.template_Id = Vendors.Where(w => w.Id == this.vendor_Id).FirstOrDefault().TEMPLATE_ID;
                }
                else if (result == 101)
                {
                    ModernDialog.ShowMessage("Template already exist..", "Alert", MessageBoxButton.OK);
                }
                else
                {
                    ModernDialog.ShowMessage("Seats blocked failed..", "Alert", MessageBoxButton.OK);
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
                IEnumerable<Seat> objScreenSeats = new SeatLayoutConfig().GetScreeSeats(Convert.ToInt32(cmbScreen.SelectedValue), 0, 0);

                if (objScreenSeats == null || objScreenSeats.Count() == 0)
                {
                    itmSeats.ItemsSource = Seats;
                    return;
                }

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
                        GenerateRowNameColumn(objSubClassSeatsList.Where(w => w.RowValue == row).FirstOrDefault(), true, objSubClassSeatsList.Where(w => w.RowValue == row && w.FK_SeatStatus_ID == 1).Count() > 0);
                        foreach (Seat objSeat in objSubClassSeatsList.Where(w => w.RowValue == row).OrderBy(w => w.ColumnValue))
                        {
                            GenerateRowNameColumn(objSeat, false);
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

                itmSeats.Width = (25 * (columns + (groupList.Count > 1 ? 2 : 1)));// + 50;
                itmSeats.Height = (30 * (rowsforAllClasses + groupList.Count()));
                itmSeats.ItemsSource = Seats;
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
                    IsChecked = objSeat.VendorId != 0 ? true : false,
                    IsEnabled = (objSeat.VendorId != 0 ? (objSeat.VendorId == this.vendor_Id ? true : false) : isEnable),
                    Row = objSeat.RowValue,
                    RowNum = objSeat.RowValue,
                    RowText = Convert.ToString(objSeat.RowText),
                    ScreenClassId = objSeat.ScreenClassId,
                    SeatColor = Helper.ReturnColor((objSeat.VendorId > 0) ? objSeat.Color : (objSeat.FK_SeatStatus_ID == 0 && !isSelectAllColumn) ? "INACTIVE" : "ACTIVE"),
                    SeatHeight = 25,
                    SeatOrientation = Orientation.Horizontal,
                    SeatWidth = isSelectAllColumn ? 50 : 20,
                    TextVisibility = Visibility.Visible,
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

        private int vendor_Id;

        private int template_Id;

        private ObservableCollection<Vendors> vendors;
        public ObservableCollection<Vendors> Vendors
        {
            get
            {
                return vendors;
            }
            set
            {
                vendors = value;
                NotifyPropertyChanged("Vendors");
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
    }
}