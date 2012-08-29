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

namespace OIShoppingListWinPhone.CustomLayout
{
    public partial class PickItemContainer : UserControl
    {
        //Bool flag for indicating whether the page is loaded
        bool bLoaded = false;

        public PickItemContainer()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ListItemContainer_Loaded);
            this.Unloaded += new RoutedEventHandler(PickItemContainer_Unloaded);
        }

        void PickItemContainer_Unloaded(object sender, RoutedEventArgs e)
        {
            bLoaded = false;
        }

        void ListItemContainer_Loaded(object sender, RoutedEventArgs e)
        {
            //Set UI elements text font size according to Application Settings
            this.ItemQuantity.FontSize = App.Settings.FontSizeSetting;
            this.ItemName.FontSize = App.Settings.FontSizeSetting;
            this.ItemUnits.FontSize = App.Settings.FontSizeSetting;

            //Set UI elements visibility according to Application Settings
            this.ItemPrice.Visibility = App.Settings.ShowPriceSettings ? Visibility.Visible : Visibility.Collapsed;
            this.ItemPriority.Visibility = App.Settings.ShowPrioritySettings ? Visibility.Visible : Visibility.Collapsed;
            this.ItemUnits.Visibility = App.Settings.ShowUnitsSettings ? Visibility.Visible : Visibility.Collapsed;
            this.ItemQuantity.Visibility = App.Settings.ShowQuantitySettings ? Visibility.Visible : Visibility.Collapsed;
            this.ItemTag.Visibility = App.Settings.ShowTagsSettings ? Visibility.Visible : Visibility.Collapsed;

            //Set bLoaded flag to true with Page.Loaded event
            bLoaded = true;
        }
        
        private void ItemCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (bLoaded)
            {
                //With checking the ItemStatus checkbox -> Save Item Status to local Database
                ShoppingListItem item = this.DataContext as ShoppingListItem;
                (sender as CheckBox).IsChecked = false;
                App.ViewModel.PickItem(item);
            }
        }

        private void ItemCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (bLoaded)
            {
                //With unchecking the ItemStatus checkbox -> Save Item Status to local Database
                ShoppingListItem item = this.DataContext as ShoppingListItem;
                App.ViewModel.UpdateItemStatus(item, false);
            }
        }

        private void PickImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShoppingListItem item = this.DataContext as ShoppingListItem;
            App.ViewModel.UpdateItemStatus(item, false);
        }
    }
}
