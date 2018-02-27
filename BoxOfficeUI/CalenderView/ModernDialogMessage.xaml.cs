using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Controls;

namespace BoxOfficeUI.CalenderView
{
    /// <summary>
    /// Interaction logic for ModernDialogMessage.xaml
    /// </summary>
    public partial class ModernDialogMessage : ModernDialog
    {
        public ModernDialogMessage()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
        }
    }
}
