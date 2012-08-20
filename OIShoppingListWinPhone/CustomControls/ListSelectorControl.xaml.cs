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
using System.Collections.ObjectModel;

using OIShoppingListWinPhone.DataModel;
using OIShoppingListWinPhone.ViewModel;

namespace OIShoppingListWinPhone.CustomLayout
{
    public partial class ListSelectorControl : UserControl
    {
        //Item to be moved
        public ShoppingListItem item;
        //Item containing list
        public ShoppingList list;

        public ListSelectorControl()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ListSelectorControl_Loaded);
        }

        void ListSelectorControl_Loaded(object sender, RoutedEventArgs e)
        {
            IEnumerable<ShoppingList> collection = new ObservableCollection<ShoppingList>();
            collection = from ls in App.ViewModel.ShoppingLists
                         where ls.ListID != list.ListID
                         select ls;

            this.ListContainer.ItemsSource = collection;
        }
        
        /// <summary>
        /// Displaying (or 'activating') the dialog on the screen.
        /// </summary>
        public void Activate()
        {
            //Displaying dialog on the screen
            this.Visibility = System.Windows.Visibility.Visible;
            VisualStateManager.GoToState(this, "VisibleVisualState", true);
        }

        /// <summary>
        /// Deactivating (or 'hide') the dialog from the screen.
        /// </summary>
        public void Deactivate()
        {
            VisualStateManager.GoToState(this, "CollapsedVisualState", true);
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            //Actually creating new list item
            ShoppingListItem newListItem = new ShoppingListItem()
            {
                ItemName = item.ItemName,
                List = (sender as RadioButton).DataContext as ShoppingList,
                Priority = item.Priority,
                Price = item.Price,
                Quantity = item.Quantity,
                Units = item.Units,
                Tag = item.Tag,
                Status = item.Status,
                Note = item.Note,
            };

            App.ViewModel.MoveItemToAnotherList(list, (sender as RadioButton).DataContext as ShoppingList, item);
            VisualStateManager.GoToState(this, "CollapsedVisualState", true);
        }
    }
}
