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
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SettingsPage_Loaded);
        }

        void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            switch (App.AppSettings.FontSize)
            {
                case (int)Settings.FontSizeSettings.Default:
                    FontSizeSettings.SelectedIndex = 2;
                    break;
                case (int)Settings.FontSizeSettings.Large:
                    FontSizeSettings.SelectedIndex = 3;
                    break;
                case (int)Settings.FontSizeSettings.Small:
                    FontSizeSettings.SelectedIndex = 1;
                    break;
                case (int)Settings.FontSizeSettings.Tiny:
                    FontSizeSettings.SelectedIndex = 0;
                    break;
            }

            SortOrderSettings.SelectedIndex = (int)App.AppSettings.SortOrder;
            
        }

        private void AdvancedSettings_Click(object sender, ManipulationCompletedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdvancedSettingsPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButtonOk_Click(object sender, EventArgs e)
        {
            App.AppSettings.SortOrder = SortOrderSettings.SelectedIndex;

            switch (FontSizeSettings.SelectedIndex)
            {
                case 0:
                    App.AppSettings.FontSize = (int)Settings.FontSizeSettings.Tiny;
                    break;
                case 1:
                    App.AppSettings.FontSize = (int)Settings.FontSizeSettings.Small;
                    break;
                case 2:
                    App.AppSettings.FontSize = (int)Settings.FontSizeSettings.Default;
                    break;
                case 3:
                    App.AppSettings.FontSize = (int)Settings.FontSizeSettings.Large;
                    break;
            }

            MessageBox.Show("Application Settings were successfully saved", "Information", MessageBoxButton.OK);
            NavigationService.GoBack();
        }

        private void ApplicationBarIconButtonCancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}