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

        private void ApplicationBarIconButtonOk_Click(object sender, EventArgs e)
        {
            //Update Application Settings
            /*App.AppSettings.ShowPrice = (bool)this.ShowPrice.IsChecked;
            App.AppSettings.ShowTags = (bool)this.ShowTags.IsChecked;
            App.AppSettings.ShowUnits = (bool)this.ShowUnits.IsChecked;
            App.AppSettings.ShowQuantity = (bool)this.ShowQuantity.IsChecked;
            App.AppSettings.ShowPriority = (bool)this.ShowPriority.IsChecked;

            App.AppSettings.HideCheckedItems = (bool)this.HideCheckedItems.IsChecked;
            App.AppSettings.FastScrolling = (bool)this.FastScrolling.IsChecked;
            App.AppSettings.ShakeToCleanUp = (bool)this.ShakeToCleanUp.IsChecked;
            App.AppSettings.TrackPerStorePrices = (bool)this.TrackPerStorePrices.IsChecked;
            App.AppSettings.DisableScreenLock = (bool)this.DisableScreenLock.IsChecked;
            App.AppSettings.QuickEditMode = (bool)this.QuickEditMode.IsChecked;
            App.AppSettings.Filters = (bool)this.Filters.IsChecked;
            App.AppSettings.ResetQuality = (bool)this.ResetQuality.IsChecked;*/

            MessageBox.Show("Application Settings were successfully saved", "Information", MessageBoxButton.OK);
            NavigationService.GoBack();
        }

        private void ApplicationBarIconButtonCancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ResetQuality_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("Reset Quality", "Info", MessageBoxButton.OK);
        }
    }
}
