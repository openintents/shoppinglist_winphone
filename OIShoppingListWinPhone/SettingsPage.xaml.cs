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
        public SettingsPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SettingsPage_Loaded);
        }

        void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            switch ((int)App.Settings.FontSize)
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

            SortOrderSettings.SelectedIndex = App.Settings.SortOrder;
            
        }
                
        private void ApplicationBarIconButtonOk_Click(object sender, EventArgs e)
        {
            App.Settings.SortOrder = SortOrderSettings.SelectedIndex;

            switch (FontSizeSettings.SelectedIndex)
            {
                case 0:
                    App.Settings.FontSize = (int)ApplicationSettings.FontSizeSettings.Tiny;
                    break;
                case 1:
                    App.Settings.FontSize = (int)ApplicationSettings.FontSizeSettings.Small;
                    break;
                case 2:
                    App.Settings.FontSize = (int)ApplicationSettings.FontSizeSettings.Default;
                    break;
                case 3:
                    App.Settings.FontSize = (int)ApplicationSettings.FontSizeSettings.Large;
                    break;
            }

            MessageBox.Show("Application Settings were successfully saved", "Information", MessageBoxButton.OK);
            NavigationService.GoBack();
        }

        private void ApplicationBarIconButtonCancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdvancedSettingsPage.xaml", UriKind.Relative));
        }
    }
}