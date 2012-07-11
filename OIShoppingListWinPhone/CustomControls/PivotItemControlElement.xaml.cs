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
    public partial class PivotItemControlElement : UserControl
    {
        public event EventHandler<System.Windows.Input.GestureEventArgs> LayoutRootManipulationStarted;

        public event EventHandler<RoutedEventArgs> ItemCheck;
        public event EventHandler<RoutedEventArgs> ItemUncheck;

        public event EventHandler<RoutedEventArgs> OnMenuEditItemClick;
        public event EventHandler<RoutedEventArgs> OnMenuMarkItemClick;
        public event EventHandler<RoutedEventArgs> OnMenuStoresClick;
        public event EventHandler<RoutedEventArgs> OnMenuRemoveItemClick;
        public event EventHandler<RoutedEventArgs> OnMenuCopyItemClick;
        public event EventHandler<RoutedEventArgs> OnMenuDeleteItemClick;
        public event EventHandler<RoutedEventArgs> OnMenuMoveItemClick;

        public PivotItemControlElement(OIShoppingListWinPhone.DataModel.ShoppingListItem listItem)
        {
            InitializeComponent();

            if (listItem.Status == 1)
                itemCheck.IsChecked = true;

            itemName.FontSize = (int)App.AppSettings.FontSize;
            itemName.Text = listItem.ItemName;

            if (listItem.Tag != null && App.AppSettings.ShowTags)
                itemTag.Text = listItem.Tag;

            if (listItem.Priority != null && App.AppSettings.ShowPriority)
                itemPriority.Text = String.Format("-{0}-", listItem.Priority);

            if (listItem.Quantity != null && App.AppSettings.ShowQuantity)
            {
                itemQuantity.FontSize = (int)App.AppSettings.FontSize;
                itemQuantity.Text = listItem.Quantity.ToString();
            }

            if (listItem.Units != null && App.AppSettings.ShowUnits)
            {
                itemUnits.FontSize = (int)App.AppSettings.FontSize;
                itemUnits.Text = listItem.Units.ToString();
            }

            if (listItem.Price != 0 && App.AppSettings.ShowPrice)
                itemPrice.Text = String.Format("{0:F2}", listItem.Price);
        }

        public void UpdateControl(string name,
            int quantity,
            int units,
            float price,
            string tags,
            int priority,
            string note)
        {
            itemName.Text = name;
            if (tags != null)
                itemTag.Text = tags;
            if (priority != -1)
                itemPriority.Text = String.Format("-{0}-", priority);
            if (quantity != -1)
                itemQuantity.Text = quantity.ToString();
            if (units != -1)
                itemUnits.Text = units.ToString();
            if (price != 0)
                itemPrice.Text = String.Format("{0:F2}", price);
        }
                
        private void Menu_EditItem_Click(object sender, RoutedEventArgs e)
        {
            OnMenuEditItemClick(this, e);
        }

        private void Menu_MarkItem_Click(object sender, RoutedEventArgs e)
        {
            OnMenuMarkItemClick(this, e);
        }

        private void Menu_Stores_Click(object sender, RoutedEventArgs e)
        {
            OnMenuStoresClick(this, e);
        }

        private void Menu_RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            OnMenuRemoveItemClick(this, e);
        }

        private void Menu_CopyItem_Click(object sender, RoutedEventArgs e)
        {
            OnMenuCopyItemClick(this, e);
        }

        private void Menu_DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            OnMenuDeleteItemClick(this, e);
        }

        private void Menu_MoveItem_Click(object sender, RoutedEventArgs e)
        {
            OnMenuMoveItemClick(this, e);
        }

        private void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LayoutRootManipulationStarted(this, e);
        }

        private void itemCheck_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {            
            e.Handled = true;
        }

        private void itemCheck_Checked(object sender, RoutedEventArgs e)
        {
            //ItemCheck(this, e);
        }

        private void itemCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            //ItemUncheck(this, e);
        }
    }
}
