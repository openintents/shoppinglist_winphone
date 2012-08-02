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
                
        private void ButtonAdvSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdvancedSettingsPage.xaml", UriKind.Relative));
        }
    }
}