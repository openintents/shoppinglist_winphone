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

using OIShoppingListWinPhone.DataModel;
using OIShoppingListWinPhone.ViewModel;

namespace OIShoppingListWinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            PivotItem newItem = new PivotItem();
            newItem.Header = "list1";
            newItem.Margin = new Thickness(0, 0, 0, 0);

            newItem.Style = (Style)App.Current.Resources["PivotItemCustomControl"];
            pivotControl.Items.Add(newItem);

            newItem = new PivotItem();
            newItem.Header = "list2";
            newItem.Margin = new Thickness(0, 0, 0, 0);

            newItem.Style = (Style)App.Current.Resources["PivotItemCustomControl"];
            pivotControl.Items.Add(newItem);

            totalItemsPrice.Text = "Total:" + "\n" + "87.20";
            checkItemsCount.Text = "#16";
            checkItemsPrice.Text = "Checked:" + "\n" + "87.20";
        }

        private void ApplicationBarMenuAbout_Click(object sender, EventArgs e)
        {

        }

        private void ApplicationBarMenuSettings_Click(object sender, EventArgs e)
        {

        }

        private void ApplicationBarMenuMarkAllItems_Click(object sender, EventArgs e)
        {

        }

        private void ApplicationBarMenuCleanUpList_Click(object sender, EventArgs e)
        {

        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            ShoppingListViewModel vm = new ShoppingListViewModel(@"isostore:/OIShoppingListDB.sdf");
            vm.LoadData();
        }
    }
}