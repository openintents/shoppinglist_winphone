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
        bool bLoaded = false;

        public ListItemContainer()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ListItemContainer_Loaded);
        }

        void ListItemContainer_Loaded(object sender, RoutedEventArgs e)
        {
            bLoaded = true;
            ContextMenuService.SetContextMenu(this.ItemPriority, null);
            ContextMenuService.SetContextMenu(this.ItemQuantity, null);

            if (App.Settings.QuickEditModeSettings)
            {
                if ((this.DataContext as ShoppingListItem).Priority != null)
                {
                    ContextMenu cMenu = new ContextMenu();
                    cMenu.VerticalOffset = -20;

                    MenuItem item1 = new MenuItem();
                    item1.Header = "1";
                    item1.Click += new RoutedEventHandler(item_Click);

                    MenuItem item2 = new MenuItem();
                    item2.Header = "2";
                    item2.Click += new RoutedEventHandler(item_Click);

                    MenuItem item3 = new MenuItem();
                    item3.Header = "3";
                    item3.Click += new RoutedEventHandler(item_Click);

                    MenuItem item4 = new MenuItem();
                    item4.Header = "4";
                    item4.Click += new RoutedEventHandler(item_Click);

                    cMenu.Items.Add(item1);
                    cMenu.Items.Add(item2);
                    cMenu.Items.Add(item3);
                    cMenu.Items.Add(item4);

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

                    cMenu.Items.Add(item1);
                    cMenu.Items.Add(item2);
                    cMenu.Items.Add(item3);
                    cMenu.Items.Add(item4);

                    ContextMenuService.SetContextMenu(this.ItemQuantity, cMenu);
                }
            }
        }

        void item_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as MenuItem).Header as string)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                default:
                    break;
            }
        }

        void quantity_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as MenuItem).Header as string)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                default:
                    break;
            }
        }

        private void ItemCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (bLoaded)
            {
                ShoppingListItem item = this.DataContext as ShoppingListItem;
                App.ViewModel.SaveItemStatus(item, true);
            }
        }

        private void ItemCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (bLoaded)
            {
                ShoppingListItem item = this.DataContext as ShoppingListItem;
                App.ViewModel.SaveItemStatus(item, false);
            }
        }

        private void ItemParameterRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            DependencyObject obj = sender as DependencyObject;
            while (obj != null && !(obj is ListBoxItem))
                obj = VisualTreeHelper.GetParent(obj as DependencyObject);

            ShoppingListItem listItem = obj.GetValue(ListBoxItem.DataContextProperty) as ShoppingListItem;
            
            string queryBody = "?ID=" + listItem.ItemID.ToString();
            queryBody += "&ListID=" + listItem.ListID.ToString();

            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/EditItemPage.xaml"+ queryBody,
                UriKind.Relative));
        }
        
        void element_OnMenuMoveItemClick(object sender, RoutedEventArgs e)
        {

        }

        void element_OnMenuDeleteItemClick(object sender, RoutedEventArgs e)
        {

        }

        void element_OnMenuCopyItemClick(object sender, RoutedEventArgs e)
        {

        }

        void element_OnMenuEditItemClick(object sender, RoutedEventArgs e)
        {
            //object type = PivotControl.ItemTemplate.GetValue(ListBox.NameProperty);
        }

        void element_OnMenuMarkItemClick(object sender, RoutedEventArgs e)
        {

        }

        void element_OnMenuStoresClick(object sender, RoutedEventArgs e)
        {
            //ShoppingListItem listItem = (sender as PivotItemControlElement).Tag as ShoppingListItem;
            string queryBody = "/StoreItemPage.xaml";/*?ListId=" + listItem.ListID
                + "&ItemId=" + listItem.ItemID
                + "&ItemName=" + listItem.ItemName;*/
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(queryBody, UriKind.Relative));
            //NavigationService.Navigate(new Uri(queryBody, UriKind.Relative));
        }

        void element_OnMenuRemoveItemClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
