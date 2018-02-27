using System;
using System.Windows.Controls;

namespace BoxOfficeUI.Content.ModernFrame
{
    /// <summary>
    /// Interaction logic for ErrorSample.xaml
    /// </summary>
    public partial class ErrorSample : UserControl
    {
        public ErrorSample()
        {
            InitializeComponent();

            // raise exception to create navigation failure
            throw new NotSupportedException();
        }
    }
}
