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

using OIShoppingListWinPhone.Settings;

namespace OIShoppingListWinPhone
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        //Bool flag for displaying whether page is loaded or not
        private bool bLoaded = false;

        public SettingsPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SettingsPage_Loaded);
        }

        //Start initialisation of LitPickers SelectedIndexes with Page.Loaded event
        void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Set SelecnedIndex of FontSize ListPickers after loading the page
            switch ((int)App.Settings.FontSizeSetting)
            {
                case (int)ApplicationSettings.FontSizeSettings.Default:
                    FontSizeSettings.SelectedIndex = 2;
                    break;
                case (int)ApplicationSettings.FontSizeSettings.Large:
                    FontSizeSettings.SelectedIndex = 3;
                    break;
                case (int)ApplicationSettings.FontSizeSettings.Small:
                    FontSizeSettings.SelectedIndex = 1;
                    break;
                case (int)ApplicationSettings.FontSizeSettings.Tiny:
                    FontSizeSettings.SelectedIndex = 0;
                    break;
            }
            //Set SelecnedIndex of SortOrder ListPickers after loading the page
            SortOrderSettings.SelectedIndex = App.Settings.SortOrderSetting;
            //Changing flag to 'true' -> the page is loaded
            this.bLoaded = true;
        }          

        //Navigate to AdvancedSettingsPage with AdvButton Click event        
        private void ButtonAdvSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdvancedSettingsPage.xaml", UriKind.Relative));
        }

        //When the FontSize ListPicker selection changed - it's need to update application settings
        private void FontSizeSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //For preventing changing SelectedIndex of FontSize ListPicker with creating of control
            //(when the is not already loaded) using 'bLoaded' flag
            if (FontSizeSettings != null && this.bLoaded)
            {
                //Set Application Settings with corresponding values regards to
                //ListPicker SelectedIndex
                switch (FontSizeSettings.SelectedIndex)
                {
                    case 2:
                        App.Settings.FontSizeSetting = (int)ApplicationSettings.FontSizeSettings.Default;
                        break;
                    case 3:
                        App.Settings.FontSizeSetting = (int)ApplicationSettings.FontSizeSettings.Large;
                        break;
                    case 1:
                        App.Settings.FontSizeSetting = (int)ApplicationSettings.FontSizeSettings.Small;
                        break;
                    case 0:
                        App.Settings.FontSizeSetting = (int)ApplicationSettings.FontSizeSettings.Tiny;
                        break;
                }
            }
        }

        //When the SortOrder ListPicker selection changed - it's need to update application settings
        private void SortOrderSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //For preventing changing SelectedIndex of SortOrder ListPicker with creating of control
            //(when the is not already loaded) using 'bLoaded' flag
            if (SortOrderSettings != null && this.bLoaded)
                //Set Application Settings with corresponding values regards to
                //ListPicker SelectedIndex
                App.Settings.SortOrderSetting = SortOrderSettings.SelectedIndex;
        }
    }
}