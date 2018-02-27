using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace BoxOfficeUI.Util
{
    public class SeatProperties : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion

        private Visibility checkboxVisibility;
        public Visibility CheckboxVisibility
        {
            get
            {
                return checkboxVisibility;
            }
            set
            {
                checkboxVisibility = value;
                NotifyPropertyChanged("CheckboxVisibility");
            }
        }

        public int Column { get; set; }

        public string ColumnText { get; set; }

        private string displayText;
        public string DisplayText
        {
            get
            {
                return displayText;
            }
            set
            {
                displayText = value;
                NotifyPropertyChanged("DisplayText");
            }
        }

        public int Id { get; set; }

        private bool isChecked;
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                NotifyPropertyChanged("IsEnabled");
            }
        }

        public int Row { get; set; }

        public int RowNum { get; set; }

        public string RowText { get; set; }

        public int ScreenClassId { get; set; }

        public string ScreenClass { get; set; }

        private string seatColor;
        public string SeatColor
        {
            get
            {
                return seatColor;
            }
            set
            {
                seatColor = value;
                NotifyPropertyChanged("SeatColor");
            }
        }

        private string seatNoColor;
        public string SeatNoColor
        {
            get
            {
                return seatNoColor;
            }
            set
            {
                seatNoColor = value;
                NotifyPropertyChanged("SeatNoColor");
            }
        }

        public double SeatHeight { get; set; }

        public Orientation SeatOrientation { get; set; }

        public double SeatWidth { get; set; }

        private Visibility textVisibility;
        public Visibility TextVisibility
        {
            get
            {
                return textVisibility;
            }
            set
            {
                textVisibility = value;
                NotifyPropertyChanged("TextVisibility");
            }
        }

        public double TicketCost { get; set; }

        public int VendorId { get; set; }
    }
}