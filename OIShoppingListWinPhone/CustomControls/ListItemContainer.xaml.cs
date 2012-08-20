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
    public partial class ListItemContainer : UserControl
    {
        //Bool flag for indicating whether the page is loaded
        bool bLoaded = false;

        public ListItemContainer()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ListItemContainer_Loaded);
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

            //Attaching menu to ItemPriority and ItemQuantity Quick Edit Mode
            //according to corresponding Application Settings

            //If QuickEditMode = false -> ContextMenus = null
            ContextMenuService.SetContextMenu(this.ItemPriority, null);
            ContextMenuService.SetContextMenu(this.ItemQuantity, null);

            //Else -> Create and attach context menus
            if (App.Settings.QuickEditModeSettings)
            {
                if ((this.DataContext as ShoppingListItem).Priority != null)
                {
                    ContextMenu cMenu = new ContextMenu();
                    cMenu.VerticalOffset = -20;

                    MenuItem item1 = new MenuItem();
                    item1.Header = "1";
                    item1.Click += new RoutedEventHandler(priority_Click);

                    MenuItem item2 = new MenuItem();
                    item2.Header = "2";
                    item2.Click += new RoutedEventHandler(priority_Click);

                    MenuItem item3 = new MenuItem();
                    item3.Header = "3";
                    item3.Click += new RoutedEventHandler(priority_Click);

                    MenuItem item4 = new MenuItem();
                    item4.Header = "4";
                    item4.Click += new RoutedEventHandler(priority_Click);

                    MenuItem item5 = new MenuItem();
                    item5.Header = "Other Priority...";
                    item5.Click += new RoutedEventHandler(priority_Click);

                    cMenu.Items.Add(item1);
                    cMenu.Items.Add(item2);
                    cMenu.Items.Add(item3);
                    cMenu.Items.Add(item4);
                    cMenu.Items.Add(item5);

                    ContextMenuService.SetContextMenu(this.ItemPriority, cMenu);
                }

                if ((this.DataContext as ShoppingListItem).Quantity != null)
                {
                    ContextMenu cMenu = new ContextMenu();
                    cMenu.VerticalOffset = -20;

                    MenuItem item1 = new MenuItem();
                    item1.Header = "1";
                    item1.Click += new RoutedEventHandler(quantity_Click);

                    MenuItem item2 = new MenuItem();
                    item2.Header = "2";
                    item2.Click += new RoutedEventHandler(quantity_Click);

                    MenuItem item3 = new MenuItem();
                    item3.Header = "3";
                    item3.Click += new RoutedEventHandler(quantity_Click);

                    MenuItem item4 = new MenuItem();
                    item4.Header = "4";
                    item4.Click += new RoutedEventHandler(quantity_Click);

                    MenuItem item5 = new MenuItem();
                    item5.Header = "Other Quantity...";
                    item5.Click += new RoutedEventHandler(quantity_Click);
                    
                    cMenu.Items.Add(item1);
                    cMenu.Items.Add(item2);
                    cMenu.Items.Add(item3);
                    cMenu.Items.Add(item4);
                    cMenu.Items.Add(item5);

                    ContextMenuService.SetContextMenu(this.ItemQuantity, cMenu);
                }
            }
            //Set bLoaded flag to true with Page.Loaded event
            bLoaded = true;
        }

        #region QuickEditMode context menus Click event handlers

        void priority_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                switch (menuItem.Header as string)
                {
                    case "1":
                        {
                            ShoppingListItem item = menuItem.DataContext as ShoppingListItem;
                            if (item != null)
                                App.ViewModel.UpdateItemPriority(item, 1);
                        }
                        break;
                    case "2":
                        {
                            ShoppingListItem item = menuItem.DataContext as ShoppingListItem;
                            if (item != null)
                                App.ViewModel.UpdateItemPriority(item, 2);
                        }
                        break;
                    case "3":
                        {
                            ShoppingListItem item = menuItem.DataContext as ShoppingListItem;
                            if (item != null)
                                App.ViewModel.UpdateItemPriority(item, 3);
                        }
                        break;
                    case "4":
                        {
                            ShoppingListItem item = menuItem.DataContext as ShoppingListItem;
                            if (item != null)
                                App.ViewModel.UpdateItemPriority(item, 4);
                        }
                        break;
                    case "Other Priority...":
                        {
                            //Get currently selected (or actually taped) list item
                            ShoppingListItem listItem = menuItem.DataContext as ShoppingListItem;
                            if (listItem != null)
                            {
                                //Creating navigating query string
                                string queryBody = "?ID=" + listItem.ItemID.ToString();
                                queryBody += "&ListID=" + listItem.ListID.ToString();
                                //And actually navigate to EditItemPage
                                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/EditItemPage.xaml" + queryBody,
                                    UriKind.Relative));
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        void quantity_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                switch (menuItem.Header as string)
                {
                    case "1":
                        {
                            ShoppingListItem item = menuItem.DataContext as ShoppingListItem;
                            if (item != null)
                                App.ViewModel.UpdateItemQuantity(item, 1);
                        }
                        break;
                    case "2":
                        {
                            ShoppingListItem item = menuItem.DataContext as ShoppingListItem;
                            if (item != null)
                                App.ViewModel.UpdateItemQuantity(item, 2);
                        }
                        break;
                    case "3":
                        {
                            ShoppingListItem item = menuItem.DataContext as ShoppingListItem;
                            if (item != null)
                                App.ViewModel.UpdateItemQuantity(item, 3);
                        }
                        break;
                    case "4":
                        {
                            ShoppingListItem item = menuItem.DataContext as ShoppingListItem;
                            if (item != null)
                                App.ViewModel.UpdateItemQuantity(item, 4);
                        }
                        break;
                    case "Other Quantity...":
                            {
                                //Get currently selected (or actually taped) list item
                                ShoppingListItem listItem = menuItem.DataContext as ShoppingListItem;
                                if (listItem != null)
                                {
                                    //Creating navigating query string
                                    string queryBody = "?ID=" + listItem.ItemID.ToString();
                                    queryBody += "&ListID=" + listItem.ListID.ToString();
                                    //And actually navigate to EditItemPage
                                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/EditItemPage.xaml" + queryBody,
                                        UriKind.Relative));
                                }
                            }
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        private void ItemCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (bLoaded)
            {
                //With checking the ItemStatus checkbox -> Save Item Status to local Database
                ShoppingListItem item = this.DataContext as ShoppingListItem;
                App.ViewModel.UpdateItemStatus(item, true);
            }
        }

        private void ItemCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (bLoaded)
            {
                //With unchecking the ItemStatus checkbox -> Save Item Status to local Database
                ShoppingListItem item = this.DataContext as ShoppingListItem;
                if (item.Status != 2)
                    App.ViewModel.UpdateItemStatus(item, false);
            }
        }

        //If user tap on the particular item -> Navigate to EditItemPage
        private void ItemParameterRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            DependencyObject obj = sender as DependencyObject;
            while (obj != null && !(obj is ListBoxItem))
                obj = VisualTreeHelper.GetParent(obj as DependencyObject);
            //Get currently selected (or actually taped) list item
            ShoppingListItem listItem = obj.GetValue(ListBoxItem.DataContextProperty) as ShoppingListItem;

            if (listItem != null)
            {
                //Creating navigating query string
                string queryBody = "?ID=" + listItem.ItemID.ToString();
                queryBody += "&ListID=" + listItem.ListID.ToString();
                //And actually navigate to EditItemPage
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/EditItemPage.xaml" + queryBody,
                    UriKind.Relative));
            }
        }

        void element_OnMenuMoveItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            //Get currently selected (or actually taped) list item
            ShoppingListItem listItem = menuItem.DataContext as ShoppingListItem;
            if (listItem != null)
            {                
                PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
                MainPage main = frame.Content as MainPage;
                main.ActivateListSelector(listItem.List, listItem);
            }
        }

        void element_OnMenuDeleteItemClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete permanently this item?", "Delete item permanently",
                MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                MenuItem menuItem = sender as MenuItem;
                //Get currently selected (or actually taped) list item
                ShoppingListItem listItem = menuItem.DataContext as ShoppingListItem;
                if (listItem != null)
                    App.ViewModel.DeleteListItem(listItem);
            }
        }

        void element_OnMenuCopyItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            //Get currently selected (or actually taped) list item
            ShoppingListItem listItem = menuItem.DataContext as ShoppingListItem;
            if (listItem != null)
            {
                //Creating navigating query string
                string queryBody = "?ID=" + listItem.ItemID.ToString();
                queryBody += "&ListID=" + listItem.ListID.ToString();
                queryBody += "&Mode=Copy";
                //And actually navigate to EditItemPage
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/EditItemPage.xaml" + queryBody,
                    UriKind.Relative));
            }
        }

        void element_OnMenuEditItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            //Get currently selected (or actually taped) list item
            ShoppingListItem listItem = menuItem.DataContext as ShoppingListItem;
            if (listItem != null)
            {
                //Creating navigating query string
                string queryBody = "?ID=" + listItem.ItemID.ToString();
                queryBody += "&ListID=" + listItem.ListID.ToString();
                //And actually navigate to EditItemPage
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/EditItemPage.xaml" + queryBody,
                    UriKind.Relative));
            }
        }

        void element_OnMenuMarkItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            //Get currently selected (or actually taped) list item
            ShoppingListItem listItem = menuItem.DataContext as ShoppingListItem;
            if (listItem != null)
                App.ViewModel.ChangeItemStatus(listItem.List, listItem);
        }

        void element_OnMenuStoresClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            //Get currently selected list item and Navigate to StoreItemPage
            ShoppingListItem listItem = menuItem.DataContext as ShoppingListItem;
            if (listItem != null)
            {
                string queryBody = "/StoreItemPage.xaml?ListId=" + listItem.ListID
                    + "&ItemId=" + listItem.ItemID;
                (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(queryBody, UriKind.Relative));
            }
        }

        void element_OnMenuRemoveItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            //Get currently selected list item and Navigate to StoreItemPage
            ShoppingListItem listItem = menuItem.DataContext as ShoppingListItem;
            if (listItem != null)
                App.ViewModel.PickItem(listItem);
        }
    }
}
