using BoxOfficeUI.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BoxOfficeUI.CalenderView
{
    public partial class MonthView : UserControl
    {
        #region Constructor and Loaded
        public MonthView()
        {
            InitializeComponent();
            Loaded += MonthView_Loaded;
        }

        private void MonthView_Loaded(object sender, RoutedEventArgs e)
        {
            //-- Want to have the calendar show up, even if no appoints are assigned 
            //   Note - in my own app, appointments are loaded by a backgroundWorker thread to avoid a laggy UI
            if (monthAppointments == null)
                BuildCalendarUI();
        }
        #endregion

        #region Custom events Addind
        public event DisplayMonthChangedEventHandler DisplayMonthChanged;
        public delegate void DisplayMonthChangedEventHandler(MonthChangedEventArgs e, int addMonths);

        public event DayBoxDoubleClickedEventHandler DayBoxDoubleClicked;
        public delegate void DayBoxDoubleClickedEventHandler(NewAppointmentEventArgs e);

        public event SchedulingDblClickedEventHandler SchedulingDblClicked;
        public delegate void SchedulingDblClickedEventHandler(int SchedulingId);
        #endregion

        #region Properties and Variables
        static System.DateTime displayStartDate;
        public System.DateTime DisplayStartDate
        {
            get { return displayStartDate; }
            set
            {
                displayStartDate = value;
                displayMonth = displayStartDate.Month;
                displayYear = displayStartDate.Year;
            }
        }

        private System.DateTime displayEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        public System.DateTime DisplayEndDate
        {
            get { return displayEndDate; }
            set
            {
                displayEndDate = value;
            }
        }

        private int displayMonth = displayStartDate.Month;
        private int displayYear = displayStartDate.Year;

        private List<Scheduling> monthAppointments;
        internal List<Scheduling> MonthAppointments
        {
            get { return monthAppointments; }
            set
            {
                monthAppointments = value;
                BuildCalendarUI();
            }
        }

        static CultureInfo cultureInfo = new CultureInfo(CultureInfo.CurrentUICulture.LCID);
        private System.Globalization.Calendar sysCal = cultureInfo.Calendar;

        private string GetAbbreviatedMonthName(int iMonthNo)
        {
            return new DateTime(2000, iMonthNo, 1).ToString("MMMM");
        }
        #endregion

        #region Adding Calaender
        private void BuildCalendarUI()
        {
            try
            {
                int iDaysInMonth = sysCal.GetDaysInMonth(displayStartDate.Year, displayStartDate.Month);
                int iOffsetDays = Convert.ToInt32(System.Enum.ToObject(typeof(System.DayOfWeek), displayStartDate.DayOfWeek));
                int iWeekCount = 0;
                int CurrentDay = System.DateTime.Now.Day;

                WeekOfDaysControls weekRowCtrl = new WeekOfDaysControls();

                MonthViewGrid.Children.Clear();
                AddRowsToMonthGrid(iDaysInMonth, iOffsetDays);
                MonthYearLabel.Content = GetAbbreviatedMonthName(displayMonth) + " " + displayYear;

                for (int i = 1; i <= iDaysInMonth; i++)
                {
                    bool showCurrentDay = ((new System.DateTime(displayYear, displayMonth, i)) >= System.DateTime.Today) && (new System.DateTime(displayYear, displayMonth, i) <= new System.DateTime(DisplayEndDate.Year, DisplayEndDate.Month, DisplayEndDate.Day));

                    if ((i != 1) && System.Math.IEEERemainder((i + iOffsetDays - 1), 7) == 0)
                    {
                        //-- add existing weekrowcontrol to the monthgrid
                        Grid.SetRow(weekRowCtrl, iWeekCount);
                        MonthViewGrid.Children.Add(weekRowCtrl);
                        //-- make a new weekrowcontrol
                        weekRowCtrl = new WeekOfDaysControls();
                        iWeekCount += 1;
                    }

                    //-- load each weekrow with a DayBoxControl whose label is set to day number
                    DayBoxControl dayBox = new DayBoxControl();
                    dayBox.DayNumberLabel.Content = showCurrentDay ? i.ToString() : "";
                    dayBox.Tag = i;

                    if (showCurrentDay)
                    {
                        dayBox.MouseDoubleClick += DayBox_DoubleClick;
                    }

                    //-- customize daybox for today:
                    if ((new System.DateTime(displayYear, displayMonth, i)) == System.DateTime.Today)
                    {
                        //dayBox.DayLabelRowBorder.Background = (Brush)dayBox.TryFindResource("OrangeGradientBrush");
                        //dayBox.DayLabelRowBorder.Background = Brushes.Wheat;
                    }

                    //-- for design mode, add appointments to random days for show...
                    if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                    {
                        if (new Random().Next(1) < 0.25)
                        {
                            DayBoxAppointmentControl apt = new DayBoxAppointmentControl();
                            apt.DisplayHeader.Text = "Apt on " + i + "th";
                            dayBox.DayAppointmentsStack.Children.Add(apt);
                        }

                    }
                    else if (monthAppointments != null && showCurrentDay)
                    {
                        //-- Compiler warning about unpredictable results if using i (the iterator) in lambda, the 
                        //   "hint" suggests declaring another var and set equal to iterator var
                        int iday = i;
                        List<Scheduling> aptInDay = monthAppointments.FindAll(new System.Predicate<Scheduling>((Scheduling schdule) => Convert.ToDateTime(schdule.StartTime).Day == iday));
                        foreach (Scheduling a in aptInDay)
                        {
                            DayBoxAppointmentControl apt = new DayBoxAppointmentControl();
                            apt.DisplayHeader.Text = a.Subject;
                            apt.DisplayContent.Text = a.Details;
                            apt.Tag = a.SchedulId;
                            apt.MouseDoubleClick += Appointment_DoubleClick;
                            dayBox.DayAppointmentsStack.Children.Add(apt);
                        }
                    }

                    Grid.SetColumn(dayBox, (i - (iWeekCount * 7)) + iOffsetDays);
                    weekRowCtrl.WeekRowGrid.Children.Add(dayBox);
                }
                Grid.SetRow(weekRowCtrl, iWeekCount);
                MonthViewGrid.Children.Add(weekRowCtrl);
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void AddRowsToMonthGrid(int DaysInMonth, int OffSetDays)
        {
            try
            {
                MonthViewGrid.RowDefinitions.Clear();
                System.Windows.GridLength rowHeight = new System.Windows.GridLength(60, System.Windows.GridUnitType.Star);

                int EndOffSetDays = 7 - (Convert.ToInt32(System.Enum.ToObject(typeof(System.DayOfWeek), displayStartDate.AddDays(DaysInMonth - 1).DayOfWeek)) + 1);

                for (int i = 1; i <= Convert.ToInt32((DaysInMonth + OffSetDays + EndOffSetDays) / 7); i++)
                {
                    dynamic rowDef = new RowDefinition();
                    rowDef.Height = rowHeight;
                    MonthViewGrid.RowDefinitions.Add(rowDef);
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }
        #endregion

        #region Changing Month
        private void UpdateMonth(int MonthsToAdd)
        {
            try
            {
                MonthChangedEventArgs ev = new MonthChangedEventArgs();
                ev.OldDisplayStartDate = displayStartDate;
                this.DisplayStartDate = displayStartDate.AddMonths(MonthsToAdd);
                ev.NewDisplayStartDate = displayStartDate;
                if (DisplayMonthChanged != null)
                {
                    DisplayMonthChanged(ev, MonthsToAdd);
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void MonthGoPrev_MouseLeftButtonUp(System.Object sender, MouseButtonEventArgs e)
        {
            UpdateMonth(-1);
            showButtonsStatus();
        }

        private void MonthGoNext_MouseLeftButtonUp(System.Object sender, MouseButtonEventArgs e)
        {
            UpdateMonth(1);
            showButtonsStatus();
        }

        private void showButtonsStatus()
        {
            MonthGoPrev.IsHitTestVisible = (new System.DateTime(displayYear, displayMonth, 1) > DateTime.Now); 
            MonthGoPrev.Opacity = (MonthGoPrev.IsHitTestVisible) ? 1.0 : 0.5;

            MonthGoNext.IsHitTestVisible = (new System.DateTime(displayYear, displayMonth, DateTime.DaysInMonth(displayYear, displayMonth)) <= new System.DateTime(DisplayEndDate.Year, DisplayEndDate.Month, DisplayEndDate.Day));
            MonthGoNext.Opacity = (MonthGoNext.IsHitTestVisible) ? 1.0 : 0.5;
        }
        #endregion

        #region Day selection and Appointment selection events
        private void Appointment_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (object.ReferenceEquals(e.Source.GetType(), typeof(DayBoxAppointmentControl)))
                {
                    if (((DayBoxAppointmentControl)e.Source).Tag != null)
                    {
                        //-- You could put your own call to your appointment-displaying code or whatever here..
                        if (SchedulingDblClicked != null)
                        {
                            SchedulingDblClicked(Convert.ToInt32(((DayBoxAppointmentControl)e.Source).Tag));
                        }
                    }
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }

        private void DayBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //-- call to FindVisualAncestor to make sure they didn't click on existing appointment (in which case,
                //   that appointment window is already opened by handler Appointment_DoubleClick)

                if (object.ReferenceEquals(e.Source.GetType(), typeof(DayBoxControl)) && Utilities.FindVisualAncestor(typeof(DayBoxAppointmentControl), e.OriginalSource) == null)
                {
                    NewAppointmentEventArgs ev = new NewAppointmentEventArgs();
                    if (((DayBoxControl)e.Source).Tag != null)
                    {
                        ev.StartDate = new System.DateTime(displayYear, displayMonth, Convert.ToInt32(((DayBoxControl)e.Source).Tag), 10, 0, 0);
                        ev.EndDate = Convert.ToDateTime(ev.StartDate).AddHours(2);
                    }
                    if (DayBoxDoubleClicked != null)
                    {
                        DayBoxDoubleClicked(ev);
                    }
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }
        #endregion

        #region struct EventArgs
        public struct MonthChangedEventArgs
        {
            public System.DateTime OldDisplayStartDate;
            public System.DateTime NewDisplayStartDate;
        }

        public struct NewAppointmentEventArgs
        {
            public System.DateTime? StartDate;
            public System.DateTime? EndDate;
            public int? CandidateId;
            public int? RequirementId;
        }
        #endregion

        #region Visual helper class
        class Utilities
        {
            //-- Many thanks to Bea Stollnitz, on whose blog I found the original C# version of below in a drag-drop helper class... 
            public static FrameworkElement FindVisualAncestor(System.Type ancestorType, System.Windows.Media.Visual visual)
            {

                while ((visual != null && !ancestorType.IsInstanceOfType(visual)))
                {
                    visual = (System.Windows.Media.Visual)System.Windows.Media.VisualTreeHelper.GetParent(visual);
                }
                return (FrameworkElement)visual;
            }

            internal static object FindVisualAncestor(Type type, object originalSource)
            {
                return Utilities.FindVisualAncestor(type, originalSource as Visual);
            }
        }
        #endregion
    }
}