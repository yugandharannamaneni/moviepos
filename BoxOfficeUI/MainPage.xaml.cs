using BoxOffice.DAL;
using BoxOffice.Model;
using BoxOfficeUI.SeatLayout;
using BoxOfficeUI.Util;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace BoxOfficeUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();

                Helper.LoadDropDownSource(cmbTheatres, new SeatTemplateRepository().GetTheatres(), "THEATRENAME", "Id");
                txtUserName.Focus();
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerAsync(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(() => LoadWholeSeats()));
        }

        private void LoadWholeSeats()
        {
            try
            {
                Helper.GetScreenSeats(0);
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DisplayBodyGrid.Children.Clear();
            switch ((TabMaster.SelectedItem as TabItem).Name)
            {
                case "LAYOUTDESIGN":
                    DisplayBodyGrid.Children.Add(new LayoutDesign());
                    break;
                case "MANAGERBLOCKING":
                    DisplayBodyGrid.Children.Add(new ManagerBlocking());
                    break;
                case "SCHEDULING":
                    DisplayBodyGrid.Children.Add(new Scheduling());
                    break;
                case "BOOKING":
                    DisplayBodyGrid.Children.Add(new Booking());
                    break;
                case "REPORTS":
                    DisplayBodyGrid.Children.Add(new Reports());
                    break;
                case "HOLDTRAY":
                    DisplayBodyGrid.Children.Add(new HoldTray());
                    break;
                default:
                    DisplayBodyGrid.Children.Add(new LayoutDesign());
                    break;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblMessage.Content = string.Empty;
                if (string.IsNullOrEmpty(txtUserName.Text.Trim()) && string.IsNullOrEmpty(txtPassword.Password.Trim()))
                {
                    lblMessage.Content = "Please Enter User name and Password";
                }
                else if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
                {
                    lblMessage.Content = "Please Enter User name";
                }
                else if (string.IsNullOrEmpty(txtPassword.Password.Trim()))
                {
                    lblMessage.Content = "Please Enter Password";
                }
                else
                {
                    IEnumerable<User> objUser = new SeatLayoutConfig().LoginUser(txtUserName.Text, txtPassword.Password);

                    if (objUser == null || objUser.Count() == 0)
                    {
                        lblMessage.Content = "Invalid User name or Password";
                        return;
                    }

                    Helper.Theater = (cmbTheatres.SelectedItem as Theatres).THEATRENAME;
                    Helper.UserName = objUser.FirstOrDefault().UserName;
                    lblUserDisplayName.Content = objUser.FirstOrDefault().UserName;

                    string[] userPermissions = objUser.FirstOrDefault().Permissions.Split(',');

                    LAYOUTDESIGN.Visibility =
                        MANAGERBLOCKING.Visibility =
                        SCHEDULING.Visibility =
                        BOOKING.Visibility =
                        REPORTS.Visibility =
                        HOLDTRAY.Visibility = Visibility.Collapsed;

                    foreach (string tabId in userPermissions)
                    {
                        switch (tabId)
                        {
                            case "1":
                                LAYOUTDESIGN.Visibility = Visibility.Visible;
                                break;
                            case "2":
                                MANAGERBLOCKING.Visibility = Visibility.Visible;
                                break;
                            case "3":
                                SCHEDULING.Visibility = Visibility.Visible;
                                break;
                            case "4":
                                BOOKING.Visibility = Visibility.Visible;
                                break;
                            case "5":
                                REPORTS.Visibility = Visibility.Visible;
                                break;
                            case "6":
                                HOLDTRAY.Visibility = Visibility.Visible;
                                break;
                        }
                    }

                    TabMaster.SelectedIndex = userPermissions.Contains("4") ? 3 : Convert.ToInt32(userPermissions[0]) - 1;

                    LoginGrid.Visibility = Visibility.Collapsed;
                    UserGrid.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            if (ModernDialog.ShowMessage("Are you sure you want to Log Out?", string.Format("Confirmation"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                txtPassword.Password = string.Empty;
                LoginGrid.Visibility = Visibility.Visible;
                UserGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void txtUserName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}