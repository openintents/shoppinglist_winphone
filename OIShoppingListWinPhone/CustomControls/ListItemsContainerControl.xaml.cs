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

namespace OIShoppingListWinPhone.CustomLayout
{
    public partial class ListItemsContainerControl : UserControl
    {
        public ListItemsContainerControl()
        {
            InitializeComponent();
        }

        private void ItemContainer_Loaded(object sender, RoutedEventArgs e)
        {
            //To displaying currently selected item in the center of page
            if (this.ItemContainer.SelectedItem != null)
                this.ItemContainer.ScrollIntoView(this.ItemContainer.SelectedItem);
        }
    }
}
