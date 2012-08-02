using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace OIShoppingListWinPhone.CustomLayout
{
    public partial class CustomFilterListControl : UserControl
    {
        //IEnumerable<ShoppingListItem> FilteredItemsCollection;

        public CustomFilterListControl()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(PivotItemControl_Loaded);
        }

        void PivotItemControl_Loaded(object sender, RoutedEventArgs e)
        {
            /*if (ListSelector.Items.Count != 0)
                ListSelector.SelectedIndex = 0;
             */
            /*ShoppingList currentList = ListSelector.SelectedItem as ShoppingList;
            if (currentList != null)
            {
                this.FilteredItemsCollection = currentList.ListItems;
                //ItemContainer.ItemsSource = this.FilteredItemsCollection;
            }*/
        }

        private void ListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*this.FilteredItemsCollection = new EntitySet<ShoppingListItem>();
            ShoppingList currentList = ListSelector.SelectedItem as ShoppingList;
            if (currentList != null)
                this.FilteredItemsCollection = currentList.ListItems;

            if (TagsSelector.SelectedIndex > 0)
                this.FilteredItemsCollection = this.FilteredItemsCollection.Where(x => x.Tags.Contains(TagsSelector.SelectedItem as string));
            if (StoreSelector.SelectedIndex > 0)
            {
                EntitySet<ShoppingListItem> newCollection = new EntitySet<ShoppingListItem>();
                foreach (ShoppingListItem item in this.FilteredItemsCollection)
                {
                    foreach (ShoppingListItemsStores item_store in item.ItemsStores)
                    {
                        if(item_store.Store == StoreSelector.SelectedItem as ShoppingListStore);
                        newCollection.Add(item);
                    }
                }
                this.FilteredItemsCollection = newCollection;
            }
            ItemContainer.ItemsSource = this.FilteredItemsCollection;*/
        }
    }
}
