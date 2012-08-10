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
        //Bool flag for displaying whether page is loaded or not
        private bool bLoaded = false;

        public AdvancedSettingsPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(AdvancedSettingsPage_Loaded);
        }

        //Start initialisation of LitPickers SelectedIndexes with Page.Loaded event
        void AdvancedSettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Set SelecnedIndex of PickItemsSortOrder ListPickers after loading the page
            this.PickItemsSortOrderSettings.SelectedIndex = (int)App.Settings.SortOrderPickItemsSetting;

            //Changing flag to 'true' -> the page is loaded
            this.bLoaded = true;
        }

        //When the Pick Items Sort Order ListPicker selection changed - it's need to update application settings
        private void PickItemsSortOrderSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //For preventing changing SelectedIndex of PickItemsSortOrder ListPicker with creating of control
            //(when the is not already loaded) using 'bLoaded' flag
            if (PickItemsSortOrderSettings != null && this.bLoaded)
                //Set Application Settings with corresponding values regards to
                //ListPicker SelectedIndex
                App.Settings.SortOrderPickItemsSetting = this.PickItemsSortOrderSettings.SelectedIndex;
        }

        //Button for reseting all settings to default
        private void ResetAllSettings_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to RESET ALL settings? This can not be undone.",
                "Reset all settings", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                App.Settings.SetAllSettingsToDefault();

                //Update UI after set Application Settings to default
                this.HideCheckedItems.IsChecked = App.Settings.HideCheckedItemsSettings;
                this.ShakeToCleanUp.IsChecked = App.Settings.ShakeToCleanUpSettings;
                this.TrackPerStorePrices.IsChecked = App.Settings.TrackPerStorePricesSettings;
                this.QuickEditMode.IsChecked = App.Settings.QuickEditModeSettings;
                this.Filters.IsChecked = App.Settings.FiltersSettings;
                this.ResetQuality.IsChecked = App.Settings.ResetQuantitySettings;

                this.ShowPrice.IsChecked = App.Settings.ShowPriceSettings;
                this.ShowTags.IsChecked = App.Settings.ShowTagsSettings;
                this.ShowUnits.IsChecked = App.Settings.ShowUnitsSettings;
                this.ShowQuantity.IsChecked = App.Settings.ShowQuantitySettings;
                this.ShowPriority.IsChecked = App.Settings.ShowPrioritySettings;

                this.SameSortOrder.IsChecked = App.Settings.AlwaysSameSortOrderSetting;
                this.PickItemsSortOrderSettings.SelectedIndex = App.Settings.SortOrderPickItemsSetting;
                this.PickItemsDirectly.IsChecked = App.Settings.PickItemsDirectlyInListSetting;

                MessageBox.Show("All settings nave been reset", "Done!", MessageBoxButton.OK);
            }
        }
    }
}
