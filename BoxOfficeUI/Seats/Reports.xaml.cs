using BoxOfficeUI.Util;
using System;
using System.Windows.Controls;

namespace BoxOfficeUI.SeatLayout
{
    /// <summary>
    /// Interaction logic for LayoutDesign.xaml
    /// </summary>
    public partial class Reports : UserControl
    {
        public Reports()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }
        }
    }
}