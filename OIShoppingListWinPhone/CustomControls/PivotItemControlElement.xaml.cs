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
        /*#region Variables for Control's displaying

        public string ItemName { get; set; }

        public Boolean Status { get; set; }

        public Decimal Price { get; set; }

        public int Quantity { get; set; }

        public int Units { get; set; }

        public int Priority { get; set; }

        public new string Tag { get; set; }

        public string Note {get; set; }

        #endregion*/

        public event RoutedEventHandler OnMenuItemClick;

        public PivotItemControlElement(OIShoppingListWinPhone.DataModel.ShoppingListItem listItem)
        {
            InitializeComponent();

            itemCheck.IsChecked = listItem.Status;
            itemName.Text = listItem.ItemName;
            itemTag.Text = listItem.Tag;
            itemPriority.Text = String.Format("-{0}-", listItem.Priority);
            itemQuantity.Text = listItem.Quantity.ToString();
            itemUnits.Text = listItem.Units.ToString();
            itemPrice.Text = String.Format("{0:F2}", listItem.Price);

            ContextMenu pItemControlElementMenu = new ContextMenu();
            MenuItem menuItem = new MenuItem();
            menuItem.Height = 65;
            menuItem.Header = "edit item";
            menuItem.Tag = this;
            menuItem.Click += new RoutedEventHandler(menuItem_Click);
            pItemControlElementMenu.Items.Add(menuItem);
            menuItem = new MenuItem();
            menuItem.Height = 65;
            menuItem.Header = "mark item";
            menuItem.Tag = this;
            menuItem.Click += new RoutedEventHandler(menuItem_Click);
            pItemControlElementMenu.Items.Add(menuItem);
            menuItem = new MenuItem();
            menuItem.Height = 65;
            menuItem.Header = "stores...";
            menuItem.Tag = this;
            menuItem.Click += new RoutedEventHandler(menuItem_Click);
            pItemControlElementMenu.Items.Add(menuItem);
            menuItem = new MenuItem();
            menuItem.Height = 65;
            menuItem.Header = "remove item from list";
            menuItem.Tag = this;
            menuItem.Click += new RoutedEventHandler(menuItem_Click);
            pItemControlElementMenu.Items.Add(menuItem);
            menuItem = new MenuItem();
            menuItem.Height = 65;
            menuItem.Header = "copy item";
            menuItem.Tag = this;
            menuItem.Click += new RoutedEventHandler(menuItem_Click);
            pItemControlElementMenu.Items.Add(menuItem);
            menuItem = new MenuItem();
            menuItem.Height = 65;
            menuItem.Header = "delete item permanently";
            menuItem.Tag = this;
            menuItem.Click += new RoutedEventHandler(menuItem_Click);
            pItemControlElementMenu.Items.Add(menuItem);
            menuItem = new MenuItem();
            menuItem.Height = 65;
            menuItem.Header = "move item to other list";
            menuItem.Tag = this;
            menuItem.Click += new RoutedEventHandler(menuItem_Click);
            pItemControlElementMenu.Items.Add(menuItem);
            ContextMenuService.SetContextMenu(this.LayoutRoot, pItemControlElementMenu);
        }

        void menuItem_Click(object sender, RoutedEventArgs e)
        {
            OnMenuItemClick(sender, e);
        }
    }
}
