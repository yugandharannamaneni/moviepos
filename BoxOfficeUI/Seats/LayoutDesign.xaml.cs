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
using System.Windows.Input;
using System.Windows.Threading;

namespace BoxOfficeUI.SeatLayout
{
    /// <summary>
    /// Interaction logic for LayoutDesign.xaml
    /// </summary>
    public partial class LayoutDesign : UserControl
    {
        #region Loading Page & DropDowns
        public LayoutDesign()
        {
            try
            {
                InitializeComponent();
                //To do If Customer wants entaire Column selection and Unselection
                // If Required make ShowHeaderCheckBoxes value to True
                this.ShowHeaderCheckBoxes = true;

                ManageLoadingDataVisibility(true, true);

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.RunWorkerAsync(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
                ManageLoadingDataVisibility(false, false);
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() => LoadDropdownsData()));
        }

        private void LoadDropdownsData()
        {
            try
            {
                Helper.LoadDropDownSource(cmbScreen, new SeatLayoutConfig().GetScreens(), "ScreenName", "Id");

                IList<ControlItems> objControlItems = new List<ControlItems>();
                objControlItems.Add(new ControlItems() { Id = "1", Value = "L -> R And A -> Z" });
                objControlItems.Add(new ControlItems() { Id = "2", Value = "R -> L And A -> Z" });
                objControlItems.Add(new ControlItems() { Id = "3", Value = "L -> R And Z -> A" });
                objControlItems.Add(new ControlItems() { Id = "4", Value = "R -> L And Z -> A" });
                Helper.LoadDropDownSource(cmbSeatingDirection, objControlItems, "Value", "Id");
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false, false);
                LogExceptions.LogException(ex);
            }
        }

        private void cmbScreen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Helper.LoadDropDownSource(cmbClass, new SeatLayoutConfig().GetScreeClasses(Convert.ToInt32(cmbScreen.SelectedValue)), "ScreenClassName", "ScreenClassId");
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false, false);
                LogExceptions.LogException(ex);
            }
        }

        private void cmbClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                IEnumerable<Seat> objScreenSeats = new ObservableCollection<Seat>(new SeatLayoutConfig().GetScreeSeats(Convert.ToInt32(cmbScreen.SelectedValue), Convert.ToInt32(cmbClass.SelectedValue), 0).ToList());

                Seats = new ObservableCollection<SeatProperties>();

                txtColumns.IsEnabled =
                txtRows.IsEnabled =
                cmbSeatingDirection.IsEnabled =
                btnGenerate.IsEnabled =
                btnSave.IsEnabled = objScreenSeats.Count() > 0 ? false : true;

                txbLayoutMessage.Visibility = objScreenSeats.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;
                btnDelete.IsEnabled = objScreenSeats.Count() > 0 ? true : false;

                itmSeats.ItemsSource = Seats;

                ManageLoadingDataVisibility(false, false);
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false, false);
                LogExceptions.LogException(ex);
            }
        }
        #endregion

        #region Generating Layout based on rows and columns
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txbMessage.Height = 30;
                if (string.IsNullOrEmpty(txtRows.Text) && string.IsNullOrEmpty(txtColumns.Text))
                {
                    txbMessage.Text = "Please enter Rows and Columns to generate layout";
                }
                else if (string.IsNullOrEmpty(txtRows.Text))
                {
                    txbMessage.Text = "Please enter Rows to generate layout";
                }
                else if (string.IsNullOrEmpty(txtColumns.Text))
                {
                    txbMessage.Text = "Please enter Columns to generate layout";
                }
                else
                {
                    txbMessage.Height = 0;
                    txbMessage.Text = "";
                    Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() => ManageLoadingDataVisibility(true, true)));

                    Seats = new ObservableCollection<SeatProperties>();

                    BackgroundWorker worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                    worker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWork);
                    worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                    worker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false, false);
                LogExceptions.LogException(ex);
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    switch (Convert.ToInt32(cmbSeatingDirection.SelectedValue))
                    {
                        case 1:
                            GenerateLtoRLayout(Convert.ToInt32(txtRows.Text), Convert.ToInt32(txtColumns.Text), true);
                            break;
                        case 2:
                            GenerateRtoLLayout(Convert.ToInt32(txtRows.Text), Convert.ToInt32(txtColumns.Text), true);
                            break;
                        case 3:
                            GenerateLtoRLayout(Convert.ToInt32(txtRows.Text), Convert.ToInt32(txtColumns.Text), false);
                            break;
                        case 4:
                            GenerateRtoLLayout(Convert.ToInt32(txtRows.Text), Convert.ToInt32(txtColumns.Text), false);
                            break;
                    }

                    itmSeats.Width = (40 * Convert.ToInt32(txtColumns.Text)) + 80;
                    itmSeats.Height = (30 * Convert.ToInt32(txtRows.Text)) + 80;
                    itmSeats.ItemsSource = Seats;
                });
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false, false);
                LogExceptions.LogException(ex);
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ManageLoadingDataVisibility(false, false);
        }

        private void GenerateLtoRLayout(int rows, int columns, bool ascendingOrder)
        {
            try
            {
                if (ascendingOrder)
                {
                    for (int row = 0; row <= rows; row++)
                    {
                        GenerateRowSelection(row);
                        for (int column = 1; column <= columns; column++)
                        {
                            GenerateColumnSelection(row, column);
                        }
                    }
                }
                else
                {
                    GenerateRowSelection(0);
                    for (int column = 1; column <= columns; column++)
                    {
                        GenerateColumnSelection(0, column);
                    }

                    for (int row = rows; row > 0; row--)
                    {
                        GenerateRowSelection(row);
                        for (int column = 1; column <= columns; column++)
                        {
                            GenerateColumnSelection(row, column);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void GenerateRtoLLayout(int rows, int columns, bool ascendingOrder)
        {
            try
            {
                if (ascendingOrder)
                {
                    for (int row = 0; row <= rows; row++)
                    {
                        GenerateRowSelection(row);
                        for (int column = columns; column >= 1; column--)
                        {
                            GenerateColumnSelection(row, column);
                        }
                    }
                }
                else
                {
                    GenerateRowSelection(0);
                    for (int column = columns; column >= 1; column--)
                    {
                        GenerateColumnSelection(0, column);
                    }

                    for (int row = rows; row > 0; row--)
                    {
                        GenerateRowSelection(row);
                        for (int column = columns; column >= 1; column--)
                        {
                            GenerateColumnSelection(row, column);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void GenerateRowSelection(int row)
        {
            try
            {
                Seats.Add(new SeatProperties()
                {
                    CheckboxVisibility = row > 0 ? Visibility.Visible : Visibility.Collapsed,
                    Column = 0,
                    ColumnText = Convert.ToString(0),
                    DisplayText = Helper.IntToAA(row),
                    IsChecked = true,
                    IsEnabled = (row == 1) ? false : true,
                    Row = row,
                    RowNum = row,
                    RowText = Helper.IntToAA(row),
                    SeatColor = (row == 1) ? Helper.ReturnColor("INACTIVE") : Helper.ReturnColor("ACTIVE"),
                    SeatHeight = row == 0 ? 90 : 30,
                    SeatOrientation = row == 0 ? Orientation.Vertical : Orientation.Horizontal,
                    SeatWidth = 80,
                    TextVisibility = Visibility.Visible
                });
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void GenerateColumnSelection(int row, int column)
        {
            try
            {
                Seats.Add(new SeatProperties()
                {
                    CheckboxVisibility = (row == 0) ? (ShowHeaderCheckBoxes ? Visibility.Visible : Visibility.Collapsed) : Visibility.Visible,
                    Column = column,
                    ColumnText = Convert.ToString(column),
                    DisplayText = Convert.ToString(column),
                    IsChecked = true,
                    IsEnabled = (row == 0 && column == 0) ? false : true,
                    Row = row,
                    RowNum = row,
                    RowText = Helper.IntToAA(row),
                    SeatColor = (row == 0 && column == 1) ? Helper.ReturnColor("INACTIVE") : Helper.ReturnColor("ACTIVE"),
                    SeatHeight = row == 0 ? 80 : 30,
                    SeatOrientation = row == 0 ? Orientation.Vertical : Orientation.Horizontal,
                    SeatWidth = 40,
                    TextVisibility = (row == 0 ? Visibility.Visible : Visibility.Collapsed)
                });
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void ManageLoadingDataVisibility(bool ishide, bool showProgress)
        {
            itmSeats.Visibility = ishide ? Visibility.Collapsed : Visibility.Visible;
            this.IsHitTestVisible = showProgress ? false : true;
            pgrProgress.Visibility = showProgress ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

        #region Seat selection and un selection
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ManageLoadingDataVisibility(true, true);
                CheckBox objCurrentCheckbox = sender as CheckBox;
                SeatProperties objSeatProperties = objCurrentCheckbox.Tag as SeatProperties;

                if (objSeatProperties.Column == 0)
                {
                    RowSeatsManagement(objSeatProperties);
                }
                else if (objSeatProperties.Row == 0)
                {
                    ColumnSeatsManagement(objSeatProperties);
                }
                else if (Seats.Where(w => w.Row == objSeatProperties.Row && w.Column > 0 && w.Column != objSeatProperties.Column && w.IsChecked).Count() == 0)
                {
                    RearrangeRowSeats(objSeatProperties);
                }
                else
                {
                    if (Seats.Where(w => w.Column == objSeatProperties.Column && w.Row != objSeatProperties.Row && w.IsChecked).Count() == 0)
                    {
                        Seats.Where(w => w.Row == 0).ToList<SeatProperties>().ForEach(x =>
                            { x.CheckboxVisibility = Visibility.Collapsed; x.TextVisibility = Visibility.Collapsed; });
                    }
                    RearrangeColumnSeats(objSeatProperties);
                }
                ManageLoadingDataVisibility(false, false);
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false, false);
                LogExceptions.LogException(ex);
            }
        }

        private void RowSeatsManagement(SeatProperties objSeat)
        {
            try
            {
                if (Seats.Where(w => w.Row == objSeat.Row - 1 && w.IsChecked).Count() == 0 && Seats.Where(w => w.Row == objSeat.Row - 2 && w.IsChecked).Count() == 0 && !objSeat.IsChecked)
                {
                    ModernDialog.ShowMessage("You are already unselected previous two row's, So you can't perform this action", "Alert", MessageBoxButton.OK);
                    objSeat.IsChecked = true;
                    return;
                }
                else if (Seats.Where(w => w.Row == objSeat.Row + 1 && w.IsChecked).Count() == 0 && Seats.Where(w => w.Row == objSeat.Row + 2 && w.IsChecked).Count() == 0 && !objSeat.IsChecked)
                {
                    ModernDialog.ShowMessage("You are already unselected next two row's, So you can't perform this action", "Alert", MessageBoxButton.OK);
                    objSeat.IsChecked = true;
                    return;
                }

                //var result = ModernDialog.ShowMessage(objSeatProperties.IsChecked ? "Are you sure, you want to Select whole row ?" : "Are you sure, you want to UnSelect whole row ?", "Confirmation", MessageBoxButton.YesNo);
                var result = MessageBoxResult.Yes;
                if (result == MessageBoxResult.Yes)
                {
                    int maxrow = Seats.Where(w => w.Column == 0 && w.Row <= objSeat.Row && w.IsChecked).Max(w => w.RowNum);
                    if (!objSeat.IsChecked)
                    {
                        Seats.Where(w => w.Row == objSeat.Row).ToList<SeatProperties>().ForEach(x =>
                        {
                            x.IsChecked = objSeat.IsChecked; x.DisplayText = string.Empty; x.RowNum = 0; x.RowText = string.Empty; //x.SeatToolTip = string.Empty;
                        });
                    }
                    else
                    {
                        Seats.Where(w => w.Row == objSeat.Row && Seats.Where(o => o.Column == w.Column && o.IsChecked).Count() > 0).ToList<SeatProperties>().ForEach(x =>
                        {
                            x.IsChecked = objSeat.IsChecked; x.RowNum = maxrow + 1; x.DisplayText = (x.Column == 0 ? Helper.IntToAA(x.RowNum) : x.ColumnText); x.RowText = Helper.IntToAA(x.RowNum); //x.SeatToolTip = (x.Row == 0 || x.Column == 0 || string.IsNullOrEmpty(x.ColumnText)) ? string.Empty : string.Format("{0} - {1}", Helper.IntToAA(x.RowNum), Convert.ToString(x.ColumnText));
                        });
                    }

                    maxrow = Seats.Where(w => w.Column == 0 && w.Row <= objSeat.Row && w.IsChecked).Max(w => w.RowNum);

                    UpdateNextRpwsandColumns(objSeat, maxrow, 0);
                }
                else if (result == MessageBoxResult.No)
                {
                    objSeat.IsChecked = !objSeat.IsChecked;
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void ColumnSeatsManagement(SeatProperties objSeat)
        {
            try
            {
                //var result = ModernDialog.ShowMessage(objSeatProperties.IsChecked ? "Are you sure, you want to Select whole column ?" : "Are you sure, you want to UnSelect whole column ?", "Confirmation", MessageBoxButton.YesNo);
                var result = MessageBoxResult.Yes;
                if (result == MessageBoxResult.Yes)
                {

                    int maxColumn = Seats.Where(w => w.Row == 0 && w.Column <= objSeat.Column && w.IsChecked && !string.IsNullOrEmpty(w.ColumnText)).Max(w => Convert.ToInt32(w.ColumnText));
                    if (!objSeat.IsChecked)
                    {
                        Seats.Where(w => w.Column == objSeat.Column).ToList<SeatProperties>().ForEach(x =>
                        {
                            x.IsChecked = objSeat.IsChecked; x.DisplayText = string.Empty; x.ColumnText = string.Empty;
                        });
                    }
                    else
                    {
                        Seats.Where(w => w.Column == objSeat.Column && Seats.Where(o => o.Row == w.Row && o.IsChecked).Count() > 0).ToList<SeatProperties>().ForEach(x =>
                        {
                            x.IsChecked = objSeat.IsChecked; x.ColumnText = Convert.ToString(maxColumn + 1); x.DisplayText = x.ColumnText; //x.SeatToolTip = (x.Row == 0 || x.Column == 0) ? string.Empty : string.Format("{0} - {1}", Helper.IntToAA(x.RowNum), Convert.ToString(x.ColumnText));
                        });
                    }

                    maxColumn = Seats.Where(w => w.Row == 0 && w.Column <= objSeat.Column && w.IsChecked && !string.IsNullOrEmpty(w.ColumnText)).Max(w => Convert.ToInt32(w.ColumnText));

                    UpdateNextRpwsandColumns(objSeat, 0, maxColumn);
                }
                else if (result == MessageBoxResult.No)
                {
                    objSeat.IsChecked = !objSeat.IsChecked;
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void RearrangeRowSeats(SeatProperties objSeat)
        {
            try
            {
                Seats.Where(w => w.Column == 0).ToList<SeatProperties>().ForEach(x =>
                {
                    x.IsChecked = false; x.RowNum = 0; x.DisplayText = x.RowText = string.Empty;
                });

                int currentRow = 0;
                foreach (SeatProperties objSeatGroup in Seats.Where(w => w.Column == 0 && w.Row > currentRow).ToList<SeatProperties>())
                {
                    if (Seats.Where(w => w.Column > 0 && w.Row == objSeatGroup.Row && w.IsChecked).Count() > 0)
                    {
                        currentRow = currentRow + 1;
                        objSeatGroup.IsChecked = true;
                        objSeatGroup.RowNum = currentRow;
                        objSeatGroup.DisplayText = Helper.IntToAA(objSeatGroup.RowNum);
                        objSeatGroup.RowText = Helper.IntToAA(currentRow);
                    }
                }

                RearrangeColumnSeats(objSeat);
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void RearrangeColumnSeats(SeatProperties objSeat)
        {
            try
            {
                Seats.Where(w => w.Row == objSeat.Row && w.Column >= objSeat.Column).ToList<SeatProperties>().ForEach(x =>
                {
                    x.ColumnText = x.DisplayText = string.Empty;
                });

                int currentColumn = 0;
                foreach (SeatProperties obj in Seats.Where(w => w.Row == objSeat.Row && w.IsChecked).OrderBy(w => w.Column))
                {
                    currentColumn = currentColumn + (obj.Column == 0 ? 0 : 1);
                    obj.ColumnText = obj.Column == 0 ? obj.ColumnText : Convert.ToString(currentColumn);
                    obj.DisplayText = obj.Column == 0 ? obj.DisplayText : Convert.ToString(currentColumn);
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void UpdateNextRpwsandColumns(SeatProperties objSeat, int maxrow, int maxColumn)
        {
            try
            {
                int currentRow = 0;
                int currentColumn = 0;

                foreach (SeatProperties obj in Seats.Where(w => w.Row == objSeat.Row && w.IsChecked).OrderBy(w => w.Column))
                {
                    currentColumn = currentColumn + (obj.Column == 0 ? 0 : 1);
                    obj.ColumnText = obj.Column == 0 ? obj.ColumnText : Convert.ToString(currentColumn);
                    obj.DisplayText = obj.Column == 0 ? obj.DisplayText : Convert.ToString(currentColumn);
                }

                //if (maxrow > 0)
                //{
                    foreach (SeatProperties obj in Seats.Where(w => w.Row > objSeat.Row && w.IsChecked).OrderBy(w => w.Row))
                    {
                        maxrow = obj.Row != currentRow ? maxrow + 1 : maxrow;
                        currentRow = obj.Row != currentRow ? obj.Row : currentRow;
                        obj.RowNum = maxrow;
                        obj.DisplayText = (obj.Column == 0 ? Helper.IntToAA(obj.RowNum) : obj.ColumnText);
                        obj.RowText = Helper.IntToAA(obj.RowNum);
                    }
                //}

                //if (maxColumn > 0)
                //{
                    currentColumn = maxColumn;
                    foreach (SeatProperties obj in Seats.Where(w => w.Column > objSeat.Column && w.IsChecked).OrderBy(w => w.Column))
                    {
                        maxColumn = obj.Column != currentColumn ? maxColumn + 1 : maxColumn;
                        currentColumn = obj.Column != currentColumn ? obj.Column : currentColumn;
                        obj.ColumnText = Convert.ToString(maxColumn);
                        obj.DisplayText = obj.ColumnText;
                    }
                //}
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }
        #endregion

        #region Save & Delete events
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Seats == null || Seats.Count() == 0)
                {
                    txbMessage.Text = "Please enter Rows and Columns to generate layout";
                    txbMessage.Height = 30;
                    return;
                }
                txbMessage.Height = 0;
                var result = ModernDialog.ShowMessage("Are you sure, you want to Save Layout ?", "Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    ManageLoadingDataVisibility(false, true);
                    Screenclasses objScreenclasses = new Screenclasses();
                    objScreenclasses.ScreenId = Convert.ToInt32(cmbScreen.SelectedValue);
                    objScreenclasses.ScreenClassId = Convert.ToInt32(cmbClass.SelectedValue);
                    objScreenclasses.RowCount = Convert.ToInt32(txtRows.Text);
                    objScreenclasses.ColumnCount = Convert.ToInt32(txtColumns.Text);

                    objScreenclasses.seats = new List<Seat>();

                    foreach (SeatProperties sp in Seats.Where(w => w.Column != 0 && w.Row != 0))
                    {
                        objScreenclasses.seats.Add(new Seat()
                        {
                            ColumnText = Convert.ToString(sp.ColumnText),
                            ColumnValue = sp.Column,
                            RowText = sp.RowText,
                            RowValue = sp.Row
                        });
                    }

                    int insertRresult = new SeatLayoutConfig().BulkInsertSeat(objScreenclasses);

                    if (insertRresult == 1)
                    {
                        Helper.LoadDropDownSource(cmbClass, new SeatLayoutConfig().GetScreeClasses(Convert.ToInt32(cmbScreen.SelectedValue)), "ScreenClassName", "ScreenClassId", false);
                    }

                    ModernDialog.ShowMessage(insertRresult == 1 ? "Screen Layout saved succefully" : "Unable to save Screen Layout at this moment.. Try after sometime", "Alert", MessageBoxButton.OK);

                    ManageLoadingDataVisibility(false, false);
                }
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false, false);
                LogExceptions.LogException(ex);
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Helper.NumericTextbox(sender, e);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var result = ModernDialog.ShowMessage("Are you sure, you want to delete Layout ?", "Confirmation", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                    return;

                var deleteResult = new SeatLayoutConfig().DeleteLayout(Convert.ToInt32(cmbScreen.SelectedValue), Convert.ToInt32(cmbClass.SelectedValue));

                ModernDialog.ShowMessage(deleteResult == 1 ? "Screen Layout deleted successfully." : "Sorry!.. We are unable to delete Screen Layout. Screen layout is in using", "Alert", MessageBoxButton.OK);

                if (deleteResult == 1)
                {
                    Helper.LoadDropDownSource(cmbClass, new SeatLayoutConfig().GetScreeClasses(Convert.ToInt32(cmbScreen.SelectedValue)), "ScreenClassName", "ScreenClassId", false);
                }
            }
            catch (Exception ex)
            {
                ManageLoadingDataVisibility(false, false);
                LogExceptions.LogException(ex);
            }
        }
        #endregion

        #region Properties
        private bool ShowHeaderCheckBoxes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<SeatProperties> seats;

        public ObservableCollection<SeatProperties> Seats
        {
            get
            {
                return seats;
            }
            set
            {
                seats = value;
                NotifyPropertyChanged("Seats");
            }
        }

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