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
        public ListItemContainer()
        {
            InitializeComponent();
        }

        private void ItemCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            Grid elementRoot = box.Parent as Grid;
            object element = elementRoot.Parent;

            DependencyObject obj = sender as DependencyObject;
            while (obj != null && !(obj is ListBoxItem))
                obj = VisualTreeHelper.GetParent(obj);
            ShoppingListItem item = (obj as ListBoxItem).DataContext as ShoppingListItem;
        }

        private void ItemCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void ItemParameterRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            DependencyObject obj = sender as DependencyObject;
            while (obj != null && !(obj is ListBoxItem))
                obj = VisualTreeHelper.GetParent(obj as DependencyObject);

            ShoppingListItem listItem = obj.GetValue(ListBoxItem.DataContextProperty) as ShoppingListItem;
            
            string queryBody = "?ID=" + listItem.ItemID.ToString()
                + "&Name=" + listItem.ItemName
                + "&Price=" + String.Format("{0:F2}", listItem.Price)
                + "&Tag=" + listItem.Tag
                + "&Note=" + listItem.Note;

            if (listItem.Priority != null)
                queryBody += "&Priority=" + listItem.Priority.ToString();

            if (listItem.Quantity != null)
                queryBody += "&Quantity=" + listItem.Quantity.ToString();

            if (listItem.Units != null)
                queryBody += "&Units=" + listItem.Units.ToString();

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
