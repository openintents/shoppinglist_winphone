using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace OIShoppingListWinPhone
{
    public partial class AdvancedSettingsPage : PhoneApplicationPage
    {
        public AdvancedSettingsPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(AdvancedSettingsPage_Loaded);
        }

        void AdvancedSettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            /*this.ShowPrice.IsChecked = App.AppSettings.ShowPrice;
            this.ShowTags.IsChecked = App.AppSettings.ShowTags;
            this.ShowUnits.IsChecked = App.AppSettings.ShowUnits;
            this.ShowQuantity.IsChecked = App.AppSettings.ShowQuantity;
            this.ShowPriority.IsChecked = App.AppSettings.ShowPriority;

            this.HideCheckedItems.IsChecked = App.AppSettings.HideCheckedItems;
            this.FastScrolling.IsChecked = App.AppSettings.FastScrolling;
            this.ShakeToCleanUp.IsChecked = App.AppSettings.ShakeToCleanUp;
            this.TrackPerStorePrices.IsChecked = App.AppSettings.TrackPerStorePrices;
            this.DisableScreenLock.IsChecked = App.AppSettings.DisableScreenLock;
            this.QuickEditMode.IsChecked = App.AppSettings.QuickEditMode;
            this.Filters.IsChecked = App.AppSettings.Filters;
            this.ResetQuality.IsChecked = App.AppSettings.ResetQuality;*/
        }        

        private void ResetQuality_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("Reset Quality", "Info", MessageBoxButton.OK);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetAllSettings_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
