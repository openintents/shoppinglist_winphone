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

using OIShoppingListWinPhone.DataModel;
using OIShoppingListWinPhone.ViewModel;
using Microsoft.Phone.Controls;

namespace OIShoppingListWinPhone.CustomControls
{
    public partial class CustomPivotControl : UserControl
    {
        public CustomPivotControl()
        {
            InitializeComponent();
        }

        private void ItemContainer_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ListBox box = sender as ListBox;
            ShoppingListItem item = box.SelectedItem as ShoppingListItem;
        }

        private void ItemContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = sender as ListBox;
            ShoppingListItem item = list.SelectedItem as ShoppingListItem;
        }
    }
}
