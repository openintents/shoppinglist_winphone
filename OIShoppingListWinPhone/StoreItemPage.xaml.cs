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
using System.Collections.ObjectModel;

using OIShoppingListWinPhone.DataModel;
using OIShoppingListWinPhone.ViewModel;
using OIShoppingListWinPhone.CustomLayout;
using System.Windows.Data;

namespace OIShoppingListWinPhone
{
    public partial class StoreItemPage : PhoneApplicationPage
    {
        //List ID
        private int listId;

        //Item ID
        private int itemId;

        //List instance
        private ShoppingList list;

        //Item instance
        private ShoppingListItem item;

        //Items Store Structure for rename
        private ItemsStoresStructure itemToRename;

        //Collection for UI data binding
        private ItemsStoresCollection collection;

        //Edit Name Dialog Control
        private EditNameDialog dlgName;

        public StoreItemPage()
        {
            InitializeComponent();            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("ListId"))
            {
                listId = Convert.ToInt32(NavigationContext.QueryString["ListId"]);
                list = App.ViewModel.ShoppingLists.Where(c => c.ListID == listId).FirstOrDefault();

                itemId = Convert.ToInt32(NavigationContext.QueryString["ItemId"]);
                item = list.ListItems.Where(i => i.ItemID == itemId).FirstOrDefault();

                PageTitle.Text = item.ItemName + PageTitle.Text;
                this.collection = new ItemsStoresCollection();

                IEnumerable<ShoppingListItemsStores> ItemsStores = App.ViewModel.LoadStoresPerItem(itemId);
                var itemsStoresIDs = from item_store in ItemsStores
                                     select item_store.StoreID;

                foreach (ShoppingListStore storePerItem in list.ListStores)
                {
                    if (itemsStoresIDs.Contains(storePerItem.StoreID))
                    {
                        ShoppingListItemsStores item_store = 
                            ItemsStores.Where(i_s => i_s.StoreID == storePerItem.StoreID).FirstOrDefault();
                        collection.Add(new ItemsStoresStructure(storePerItem.StoreName, true,
                            item_store.Aisle, item_store.StorePrice));
                    }
                    else
                        collection.Add(new ItemsStoresStructure(storePerItem.StoreName, false,
                            "", 0.00F));
                }

                StoreList.ItemsSource = collection;
            }
        }

        void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (dlgName.DialogData.Text.Trim() != String.Empty)
            {
                if (dlgName.DialogMode == (int)EditNameDialog.EditNameDialogMode.AddingNewStore)
                {
                    var storeNames = from item in collection
                                     select item.Name;
                    if (!storeNames.Contains(dlgName.DialogData.Text))
                    {
                        ShoppingListStore newStore = new ShoppingListStore()
                        {
                            List = list,
                            StoreName = dlgName.DialogData.Text
                        };
                        App.ViewModel.AddNewStore(list, newStore);
                        collection.Add(new ItemsStoresStructure(dlgName.DialogData.Text, false, "", 0.00F));
                    }
                }
                else if (dlgName.DialogMode == (int)EditNameDialog.EditNameDialogMode.RenamingStore)
                {
                    var storeNames = from item in collection
                                     select item.Name;
                    if (!storeNames.Contains(dlgName.DialogData.Text))
                    {                        
                        App.ViewModel.RenameStore(itemToRename.Name, dlgName.DialogData.Text);

                        int index = collection.IndexOf(itemToRename);
                        collection.Remove(itemToRename);
                        itemToRename.Name = dlgName.DialogData.Text;
                        collection.Insert(index, itemToRename);
                    }
                }

                dlgName.Deactivate();
            }
            else
                MessageBox.Show("Please, enter a name" + "\n\n" +
                                "*Note:" + "\n" + "- New name must not be empty", "Information", MessageBoxButton.OK);
        }

        private void ApplicationBarIconButtonSave_Click(object sender, EventArgs e)
        {
            foreach (ItemsStoresStructure item in StoreList.ItemsSource)
            {
                App.ViewModel.UpdateRelationship(itemId, item.Name, item.IsChecked, item.Aisle, item.StorePrice);
            }

            NavigationService.GoBack();
        }

        private void ApplicationBarIconButtonAdd_Click(object sender, EventArgs e)
        {
            dlgName = new EditNameDialog();
            dlgName.DialogMode = (int)EditNameDialog.EditNameDialogMode.AddingNewStore;
            dlgName.ButtonOK.Click += new RoutedEventHandler(ButtonOK_Click);
            dlgName.ButtonCancel.Click += new RoutedEventHandler(ButtonCancel_Click);
            dlgName.CollapsedVisualState.Storyboard.Completed += new EventHandler(Storyboard_Completed);

            //Actually activating the dialog
            LayoutRoot.Children.Add(dlgName);
            dlgName.Activate(EditNameDialog.EditNameDialogMode.AddingNewStore);
            ApplicationBar.IsVisible = false;
        }

        void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            dlgName.Deactivate();
        }

        void Storyboard_Completed(object sender, EventArgs e)
        {
            dlgName.Deactivate();
            LayoutRoot.Children.Remove(dlgName);
            ApplicationBar.IsVisible = true;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            //Deactivate EditNameDialog and supress GoBack() navigation
            if (LayoutRoot.Children.Contains(dlgName))
            {
                dlgName.Deactivate();
                e.Cancel = true;
            }

            base.OnBackKeyPress(e);
        }

        private void PerStoreItemPrice_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            //Deleting all ',' symbols from the string
            if (txt.Text.Contains(','))
            {
                txt.Text = txt.Text.Replace(",", "");
                txt.SelectionStart = txt.Text.Length;
            }
            //Deleting all '-' symbols from the string
            else if (txt.Text.Contains('-'))
            {
                txt.Text = txt.Text.Replace("-", "");
                txt.SelectionStart = txt.Text.Length;
            }

            int pos = txt.SelectionStart;
            float f = 0.00F;

            //Prevent inputting more than 3 digits after '.'
            if (txt.Text.Length > 4)
            {
                if (txt.Text.ElementAt(txt.Text.Length - 4) == '.')
                    txt.Text = txt.Text.Substring(0, txt.Text.Length - 1);
            }
            //Parse input string to float variable
            float.TryParse(txt.Text, out f);

            //If TextBox string does not contain '.' (it means that user deleted symbol '.')
            if (!txt.Text.Contains('.'))
                //Reset '.' symbol within the string
                txt.Text = txt.Text.Insert(txt.Text.Length - 2, ".");
            else
                //Format TextBox string of float with 2 digits after '.'
                txt.Text = String.Format("{0:F2}", f);
            //Set cursor in corresponding position
            txt.SelectionStart = pos;
                        
            BindingExpression exp = txt.GetBindingExpression(TextBox.TextProperty);
            exp.UpdateSource();
        }

        private void StoreAisle_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            BindingExpression exp = txt.GetBindingExpression(TextBox.TextProperty);
            exp.UpdateSource();
        }

        private void RenameStore_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = sender as MenuItem;

            if (menu != null)
            {
                this.itemToRename = menu.DataContext as ItemsStoresStructure;

                dlgName = new EditNameDialog();
                dlgName.ButtonOK.Click += new RoutedEventHandler(ButtonOK_Click);
                dlgName.ButtonCancel.Click += new RoutedEventHandler(ButtonCancel_Click);
                dlgName.CollapsedVisualState.Storyboard.Completed += new EventHandler(Storyboard_Completed);

                //Actually activating the dialog
                LayoutRoot.Children.Add(dlgName);
                dlgName.Activate(EditNameDialog.EditNameDialogMode.RenamingStore, itemToRename.Name);
                ApplicationBar.IsVisible = false;
            }
        }

        private void DeleteStore_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete this store?", "Delete store", MessageBoxButton.OKCancel)
                == MessageBoxResult.OK)
            {
                MenuItem menu = sender as MenuItem;
                ItemsStoresStructure str = menu.DataContext as ItemsStoresStructure;
                if (menu != null)
                {
                    App.ViewModel.DeleteStore(str.Name);
                    this.collection.Remove(str);
                }
            }
        }
    }

    public class ItemsStoresStructure
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public string Aisle { get; set; }
        public float StorePrice { get; set; }

        public ItemsStoresStructure(string name, bool isChecked, string aisle, float storePrice)
        {
            this.Name = name;
            this.IsChecked = isChecked;
            this.Aisle = aisle;
            this.StorePrice = storePrice;
        }
    }

    public class ItemsStoresCollection : ObservableCollection<ItemsStoresStructure>
    {
        public ItemsStoresCollection() { }
    }
}