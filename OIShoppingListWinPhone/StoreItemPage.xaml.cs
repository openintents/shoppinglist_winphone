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

namespace OIShoppingListWinPhone
{
    public partial class StoreItemPage : PhoneApplicationPage
    {
        private int itemId;
        private ShoppingList list;
        private EditNameDialog dlgName;
        ItemsStoresCollection collection = new ItemsStoresCollection();

        public StoreItemPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("ListId"))
            {
                int listId = Convert.ToInt32(NavigationContext.QueryString["ListId"]);
                list = App.ViewModel.ShoppingLists.Where(c => c.ListID == listId).FirstOrDefault();

                itemId = Convert.ToInt32(NavigationContext.QueryString["ItemId"]);

                IEnumerable<ShoppingListStore> StoresPerItem = App.ViewModel.LoadStoresPerItem(itemId);
                foreach (ShoppingListStore store in list.ListStores)
                {
                    bool isCheck = false;
                    if (StoresPerItem != null)
                    {
                        foreach (ShoppingListStore store2 in StoresPerItem)
                        {
                            if (store2.StoreName == store.StoreName)
                                isCheck = true;
                        }
                    }
                    if (isCheck)
                    {
                        collection.Add(new ItemsStoresStructure(store.StoreName, true));
                        isCheck = false;
                    }
                    else
                        collection.Add(new ItemsStoresStructure(store.StoreName, false));
                }                                                
                StoreList.ItemsSource = collection;
                
                TextBlock pageTitle = this.PageTitle as TextBlock;
                pageTitle.Text = NavigationContext.QueryString["ItemName"] + pageTitle.Text;

                dlgName =new EditNameDialog();
                dlgName.ButtonOK.Click += new RoutedEventHandler(ButtonOK_Click);
            }
        }

        void ButtonOK_Click(object sender, RoutedEventArgs e)
        {   
            if (dlgName.DialogData.Text.Trim() != String.Empty)
            {
                ShoppingListStore newStore = new ShoppingListStore()
                {
                    List = list,
                    StoreName = dlgName.DialogData.Text
                };
                App.ViewModel.AddNewStore(list, newStore);
                collection.Add(new ItemsStoresStructure(dlgName.DialogData.Text, true));

                dlgName.Deactivate();                
            }
            else
                MessageBox.Show("Please, enter a name" + "\n\n" +
                                "*Note:" + "\n" + "- New name must not be empty", "Information", MessageBoxButton.OK);
        }
        
        private void ApplicationBarIconButtonOK_Click(object sender, EventArgs e)
        {
            foreach (ItemsStoresStructure item in StoreList.ItemsSource)
            {
                App.ViewModel.UpdateRelationship(itemId, item.Name, true);
            }
            NavigationService.GoBack();
        }

        private void ApplicationBarIconButtonCancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ApplicationBarIconButtonAdd_Click(object sender, EventArgs e)
        {
            dlgName.Activate(EditNameDialog.EditNameDialogMode.AddingNewStore);
        }
    }

    public class ItemsStoresStructure
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }

        public ItemsStoresStructure(string name, bool isChecked)
        {
            this.Name = name;
            this.IsChecked = isChecked;
        }
    }

    public class ItemsStoresCollection : ObservableCollection<ItemsStoresStructure>
    {
        public ItemsStoresCollection()
        {
            //Add(new ItemsStoresStructure("Michael", true));
            //Add(new ItemsStoresStructure("Chris", true));
            //Add(new ItemsStoresStructure("Cassie", true));
            //Add(new ItemsStoresStructure("Guido", true));
            //Add(new ItemsStoresStructure("Guido", true));
        }
    }
}