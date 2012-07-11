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
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using OIShoppingListWinPhone.DataModel;
using OIShoppingListWinPhone.ViewModel;

namespace OIShoppingListWinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        private DialogNameControl dlgName;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            InitializeUI();

            dlgName = DialogName as DialogNameControl;
            dlgName.ButtonOK.Click += new RoutedEventHandler(ButtonOK_Click);
        }
        
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            if (newListItemName.Text.Trim() != String.Empty)
            {
                Pivot pivot = this.PivotControl as Pivot;
                PivotItem currentPivotItem = pivot.SelectedItem as PivotItem;
                PivotItemControl curItemControl = currentPivotItem.Content as PivotItemControl;
                ShoppingList currentList = curItemControl.Tag as ShoppingList;

                ShoppingListItem newListItem = new ShoppingListItem()
                {
                    ItemName = newListItemName.Text,
                    List = currentList,
                    Priority = null,
                    Price = 0F,
                    Units = null,
                    Quantity = null,
                    Tag = string.Empty,
                    Status = (int)ShoppingListItem.StatusEnumerator.Unchecked,
                    Note = string.Empty
                };
                
                App.ViewModel.AddNewListItem(currentList, newListItem);
                newListItemName.Text = "";
                PivotItemControlElement pControl = new PivotItemControlElement(newListItem);
                pControl.Tag = newListItem;
                //curItemControl.ContentPanel.Children.Insert(1, pControl);
                curItemControl.ContentPanel.Children.Add(pControl);
                AttachMenu(pControl);
                //For future development////////////////////////////////////////////////////////////////////////////////////////
                RedrawUI(currentPivotItem);
            }
            else
                MessageBox.Show("Please, input correct list's item name" + "\n\n" + 
                                "*Note: Name must not be empty", "Information", MessageBoxButton.OK);
        }

        #region DialogNameControl ButtonOK Click Event
                
        void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (dlgName.DialogLabel.Text == "Enter new name of the shopping list")
            {
                Pivot pivot = this.PivotControl as Pivot;
                PivotItem currentPivotItem = pivot.SelectedItem as PivotItem;
                PivotItemControl pControl = currentPivotItem.Content as PivotItemControl;
                ShoppingList renameList =  pControl.Tag as ShoppingList;

                if (dlgName.DialogData.Text.Trim() != String.Empty &&
                    dlgName.DialogData.Text != renameList.ListName)
                {
                    App.ViewModel.RenameList(renameList, dlgName.DialogData.Text);
                    currentPivotItem.Header = dlgName.DialogData.Text;

                    dlgName.Deactivate();
                }
                else
                {
                    MessageBox.Show("Please, input correct list's name" + "\n\n" +
                                    "*Note:" + "\n" + "- New name must not be empty" + "\n" +
                                    "- New name must not be the same", "Information", MessageBoxButton.OK);
                    dlgName.Activate("Enter new name of the shopping list", renameList.ListName);
                }
            }
            else if (dlgName.DialogLabel.Text == "Enter name of new shopping list")
            {
                if (dlgName.DialogData.Text.Trim() != String.Empty)
                {
                    ShoppingList newList = new ShoppingList();
                    newList.ListName = dlgName.DialogData.Text;

                    App.ViewModel.AddNewList(newList);

                    Pivot pivot = this.PivotControl as Pivot;
                    PivotItem pItem = new PivotItem();
                    pItem.Header = newList.ListName;
                    pItem.Margin = new Thickness(12, 0, 12, 0);

                    PivotItemControl pItemControl = new PivotItemControl();
                    pItemControl.Tag = newList;
                    pItem.Content = pItemControl;
                    pivot.Items.Add(pItem);
                    pivot.SelectedIndex = pivot.Items.Count - 1;

                    dlgName.Deactivate();
                }
                else
                {
                    MessageBox.Show("Please, input correct list's name" + "\n\n" +
                                    "*Note: Name must not be empty", "Information", MessageBoxButton.OK);
                    dlgName.Activate("Enter name of new shopping list", "");
                }
            }            
        }
        
        #endregion

        #region ApplicationBarIconButton's Click Events

        private void ApplicationBarIconButtonNewList_Click(object sender, EventArgs e)
        {            
            dlgName.Activate("Enter name of new shopping list", "");            
        }        

        private void ApplicationBarIconButtonRenameList_Click(object sender, EventArgs e)
        {
            Pivot pivot = this.PivotControl as Pivot;
            PivotItem currentPivotItem = pivot.SelectedItem as PivotItem;
            PivotItemControl pControl = currentPivotItem.Content as PivotItemControl;
            ShoppingList renameList = pControl.Tag as ShoppingList;
            
            dlgName.Activate("Enter new name of the shopping list", renameList.ListName);
        }

        private void ApplicationBarIconButtonSendList_Click(object sender, EventArgs e)
        {
            //Send shopping list via SMS or E-mail
        }

        private void ApplicationBarIconButtonDeleteList_Click(object sender, EventArgs e)
        {
            Pivot pivot = this.PivotControl as Pivot;
            PivotItem currentPivotItem = pivot.SelectedItem as PivotItem;
            PivotItemControl pControl = currentPivotItem.Content as PivotItemControl;
            ShoppingList delList = pControl.Tag as ShoppingList;
            if (MessageBox.Show("Are you sure you want to delete " + delList.ListName + " shopping list?", "Warning", MessageBoxButton.OKCancel)
                == MessageBoxResult.OK)
            {                        
                App.ViewModel.DeleteList(delList);
                pivot.Items.RemoveAt(pivot.SelectedIndex);

                if (App.ViewModel.ShoppingLists.Count == 0)
                {
                    pivot.Items.Clear();
                    ShoppingList newList = new ShoppingList();
                    newList.ListName = "MyNewList";

                    App.ViewModel.AddNewList(newList);

                    PivotItem pItem = new PivotItem();
                    pItem.Header = newList.ListName;
                    pItem.Margin = new Thickness(12, 0, 12, 0);

                    PivotItemControl pItemControl = new PivotItemControl();
                    pItemControl.Tag = newList;
                    pItem.Content = pItemControl;
                    pivot.Items.Add(pItem);

                    totalItemsPrice.Text = "Total:" + "\n" + "0";
                    checkItemsCount.Text = "#0";
                    checkItemsPrice.Text = "Checked:" + "\n" + "0";
                }                
                pivot.SelectedIndex = pivot.Items.Count - 1;
            }
        }

        #endregion

        #region ApplicationBarMenu's Click Events

        private void ApplicationBarMenuAbout_Click(object sender, EventArgs e)
        {

        }

        private void ApplicationBarMenuSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuMarkAllItems_Click(object sender, EventArgs e)
        {

        }

        private void ApplicationBarMenuCleanUpList_Click(object sender, EventArgs e)
        {

        }

        #endregion

        private void InitializeUI()
        {
            int chItemsCount = 0;
            float tItemsPrice = 0;
            float chItemsPrice = 0;

            Pivot pivot = this.PivotControl as Pivot;
            pivot.Items.Clear();
            if (App.ViewModel.ShoppingLists.Count != 0)
            {
                foreach (ShoppingList sList in App.ViewModel.ShoppingLists)
                {
                    PivotItem pItem = new PivotItem();
                    pItem.Header = sList.ListName;
                    pItem.Margin = new Thickness(12, 0, 12, 0);

                    PivotItemControl pItemControl = new PivotItemControl();
                    pItemControl.Tag = sList;

                    List<ShoppingListItem> listItems = new List<ShoppingListItem>();
                    foreach (ShoppingListItem listItem in sList.ListItems)
                    {
                        listItems.Add(listItem);
                    }
                    IEnumerable<ShoppingListItem> sortListItems = listItems;
                    switch (App.AppSettings.SortOrder)
                    {
                        case (int)Settings.SortOrderSettings.UncheckedFirst_Alphabetical:
                            listItems.Sort(new UncheckedFirst_Alphabetical());
                            sortListItems = listItems;
                            break;
                        case (int)Settings.SortOrderSettings.Alphabetical:
                            sortListItems = listItems.OrderBy(x => x.ItemName);
                            break;
                        case (int)Settings.SortOrderSettings.NewestFirst:
                            listItems.Sort(new NewestFirst());
                            sortListItems = listItems;
                            break;
                        case (int)Settings.SortOrderSettings.OldestFirst:
                            sortListItems = listItems.OrderBy(x => x.CreatedDate);
                            break;
                        case (int)Settings.SortOrderSettings.TagsAlphabetical:
                            sortListItems = listItems.OrderBy(x => x.Tag);
                            break;
                        case (int)Settings.SortOrderSettings.Priority_TagsAlphabetical:
                            listItems.Sort(new Priority_TagsAlphabetical());
                            sortListItems = listItems;
                            break;
                        case (int)Settings.SortOrderSettings.MostExpensiveFirst:
                            listItems.Sort(new MostExpensiveFirst());
                            sortListItems = listItems;
                            break;
                        case (int)Settings.SortOrderSettings.UncheckedFirst_TagsAlphabetical:
                            listItems.Sort(new UncheckedFirst_TagsAlphabetical());
                            sortListItems = listItems;
                            break;
                        case (int)Settings.SortOrderSettings.UncheckedFirst_Priority_Alphabetical:
                            listItems.Sort(new UncheckedFirst_Priority_Alphabetical());
                            sortListItems = listItems;
                            break;
                        case (int)Settings.SortOrderSettings.UnckeckedFirst_Priority_TagsAlphabetical:
                            listItems.Sort(new UnckeckedFirst_Priority_TagsAlphabetical());
                            sortListItems = listItems;
                            break;
                    }

                    foreach (ShoppingListItem listItem in sortListItems)
                    {
                        PivotItemControlElement pControl = new PivotItemControlElement(listItem);
                        AttachMenu(pControl);
                        pControl.Tag = listItem;
                        pItemControl.ContentPanel.Children.Add(pControl);

                        chItemsCount++;
                        tItemsPrice += listItem.Price;
                        if (listItem.Status == (int)ShoppingListItem.StatusEnumerator.Checked)
                            chItemsPrice += listItem.Price;
                    }
                    pItem.Content = pItemControl;
                    pivot.Items.Add(pItem);
                }

                totalItemsPrice.Text = "Total:" + "\n" + tItemsPrice.ToString();
                checkItemsCount.Text = "#" + chItemsCount.ToString();
                checkItemsPrice.Text = "Checked:" + "\n" + chItemsPrice.ToString();
            }
            else
            {
                ShoppingList newList = new ShoppingList();
                newList.ListName = "MyNewList";

                App.ViewModel.AddNewList(newList);

                PivotItem pItem = new PivotItem();
                pItem.Header = newList.ListName;
                pItem.Margin = new Thickness(12, 0, 12, 0);

                PivotItemControl pItemControl = new PivotItemControl();
                pItemControl.Tag = newList;
                pItem.Content = pItemControl;
                pivot.Items.Add(pItem);

                totalItemsPrice.Text = "Total:" + "\n" + "0";
                checkItemsCount.Text = "#0";
                checkItemsPrice.Text = "Checked:" + "\n" + "0";
            }
        }       

        private void AttachMenu(PivotItemControlElement element)
        {
            element.LayoutRootManipulationStarted += new EventHandler<System.Windows.Input.GestureEventArgs>(element_LayoutRootManipulationStarted);

            element.OnMenuEditItemClick += new EventHandler<RoutedEventArgs>(element_OnMenuEditItemClick);
            element.OnMenuMarkItemClick += new EventHandler<RoutedEventArgs>(element_OnMenuMarkItemClick);
            element.OnMenuStoresClick += new EventHandler<RoutedEventArgs>(element_OnMenuStoresClick);
            element.OnMenuRemoveItemClick += new EventHandler<RoutedEventArgs>(element_OnMenuRemoveItemClick);
            element.OnMenuCopyItemClick += new EventHandler<RoutedEventArgs>(element_OnMenuCopyItemClick);
            element.OnMenuDeleteItemClick += new EventHandler<RoutedEventArgs>(element_OnMenuDeleteItemClick);
            element.OnMenuMoveItemClick += new EventHandler<RoutedEventArgs>(element_OnMenuMoveItemClick);
        }

        private void RedrawUI(PivotItem item)
        {
        }
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("Name"))
            {
                NavigationService.RemoveBackEntry();
                if (NavigationContext.QueryString.ContainsKey("ID"))
                {
                    string newName = NavigationContext.QueryString["Name"].ToString();
                    float newPrice = (float)Convert.ToDouble(NavigationContext.QueryString["Price"].ToString());
                    string newTag = NavigationContext.QueryString["Tag"].ToString();
                    string newNote = NavigationContext.QueryString["Note"].ToString();

                    int? newQuantity;
                    if (NavigationContext.QueryString["Quantity"].ToString() != "")
                        newQuantity = Convert.ToInt32(NavigationContext.QueryString["Quantity"].ToString());
                    else
                        newQuantity = null;

                    int? newUnits;
                    if (NavigationContext.QueryString["Units"].ToString() != "")
                        newUnits = Convert.ToInt32(NavigationContext.QueryString["Units"].ToString());
                    else
                        newUnits = null;
                    
                    int? newPriority;
                    if (NavigationContext.QueryString["Priority"].ToString() != "")
                        newPriority = Convert.ToInt32(NavigationContext.QueryString["Priority"].ToString());
                    else
                        newPriority = null;
                    
                    App.ViewModel.UpdateListItem(Convert.ToInt32(NavigationContext.QueryString["ID"].ToString()),
                        newName, newQuantity, newUnits, newPrice, newTag, newPriority, newNote);
                }
            }
        }

        void element_LayoutRootManipulationStarted(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShoppingListItem listItem = (sender as PivotItemControlElement).Tag as ShoppingListItem;

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

            NavigationService.Navigate(new Uri("/EditItemPage.xaml" + queryBody, UriKind.Relative));
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
            
        }

        void element_OnMenuMarkItemClick(object sender, RoutedEventArgs e)
        {
            
        }

        void element_OnMenuStoresClick(object sender, RoutedEventArgs e)
        {
            ShoppingListItem listItem = (sender as PivotItemControlElement).Tag as ShoppingListItem;
            string queryBody = "?ListId=" + listItem.ListID
                + "&ItemId=" + listItem.ItemID
                + "&ItemName=" + listItem.ItemName;
            NavigationService.Navigate(new Uri("/StoreItemPage.xaml" + queryBody, UriKind.Relative));
        }

        void element_OnMenuRemoveItemClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}