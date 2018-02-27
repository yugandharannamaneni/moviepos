﻿using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace BoxOfficeUI.Content.ModernFrame
{
    /// <summary>
    /// Interaction logic for CancelNavigateSample.xaml
    /// </summary>
    public partial class CancelNavigateSample : UserControl, IContent
    {
        public CancelNavigateSample()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            // display the current navigated fragment
            fragmentNav.BBCode = string.Format(CultureInfo.CurrentUICulture, "Current navigation fragment: '[b]{0}[/b]'", e.Fragment);
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            // clear fragment text
            fragmentNav.BBCode = null;
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // ask user if navigating away is ok
            string frameDescription;
            if (e.IsParentFrameNavigating){
                frameDescription = "A parent frame";
            }
            else {
                frameDescription = "This frame";
            }

            // modern message dialog supports BBCode tags
            var question = string.Format(CultureInfo.CurrentUICulture, "[b]{0}[/b] is about to navigate to new content. Do you want to allow this?", frameDescription);

            if (MessageBoxResult.No == ModernDialog.ShowMessage(question, "navigate", System.Windows.MessageBoxButton.YesNo)) {
                e.Cancel = true;
            }
        }
    }
}
