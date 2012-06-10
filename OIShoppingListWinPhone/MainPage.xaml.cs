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
using Microsoft.Phone.Shell;

namespace OIShoppingListWinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
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
                    Priority = 1,
                    Price = 5,
                    Units = 2,
                    Quantity = 2,
                    Tag = "Tag",
                    Status = true
                };
                
                App.ViewModel.AddNewListItem(currentList, newListItem);

                newListItemName.Text = "";

                curItemControl.ContentPanel.Children.Add(new PivotItemControlElement(newListItem));
                //For future development
                RedrawPivotItem(currentPivotItem);
            }
            else
                MessageBox.Show("Please, input correct list's item name" + "\n\n" + 
                                "*Note: Name must not be empty", "Information", MessageBoxButton.OK);
        }

        #region ApplicationBarIconButton's Click Events

        private void ApplicationBarIconButtonNewList_Click(object sender, EventArgs e)
        {
            DialogLabel.Text = "Enter name of new shopping list";
            DisableRect.Visibility = Visibility.Visible;
            DialogRect.Visibility = Visibility.Visible;
        }

        private void ApplicationBarIconButtonRenameList_Click(object sender, EventArgs e)
        {
            DialogLabel.Text = "Enter new name of the shopping list";
            Pivot pivot = this.PivotControl as Pivot;
            PivotItem currentPivotItem = pivot.SelectedItem as PivotItem;
            PivotItemControl pControl = currentPivotItem.Content as PivotItemControl;
            ShoppingList renameList = pControl.Tag as ShoppingList;
            DialogData.Text = renameList.ListName;
            DisableRect.Visibility = Visibility.Visible;
            DialogRect.Visibility = Visibility.Visible;            
        }

        private void ApplicationBarIconButtonSendList_Click(object sender, EventArgs e)
        {

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
            Decimal tItemsPrice = 0;
            Decimal chItemsPrice = 0;

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

                    foreach (ShoppingListItem listItem in sList.ListItems)
                    {
                        PivotItemControlElement pControl = new PivotItemControlElement(listItem);
                        pControl.OnMenuItemClick += new RoutedEventHandler(pControl_OnMenuItemClick);
                        pControl.Tag = listItem;
                        pItemControl.ContentPanel.Children.Add(pControl);

                        chItemsCount++;
                        tItemsPrice += listItem.Price;
                        if (listItem.Status)
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

        private void RedrawPivotItem(PivotItem item)
        {
        }

        void pControl_OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            PivotItemControlElement element = menuItem.Tag as PivotItemControlElement;
            ShoppingListItem listItem = element.Tag as ShoppingListItem;
            switch (menuItem.Header.ToString())
            {
                case "edit item":
                    {
                    }
                    break;
                case "mark item":
                    {
                    }
                    break;
                case "stores...":
                    {
                    }
                    break;
                case "remove item from list":
                    {
                    }
                    break;
                case "copy item":
                    {
                    }
                    break;
                case "delete item permanently":
                    {
                    }
                    break;
                case "move item to other list":
                    {
                    }
                    break;
            }
        }

        private void ButtonDialogOK_Click(object sender, RoutedEventArgs e)
        {
            if (DialogLabel.Text == "Enter new name of the shopping list")
            {
                Pivot pivot = this.PivotControl as Pivot;
                PivotItem currentPivotItem = pivot.SelectedItem as PivotItem;
                PivotItemControl pControl = currentPivotItem.Content as PivotItemControl;
                ShoppingList renameList = pControl.Tag as ShoppingList;
                App.ViewModel.RenameList(renameList, DialogData.Text);
                currentPivotItem.Header = DialogData.Text;
            }
            else if (DialogLabel.Text == "Enter name of new shopping list")
            {
                ShoppingList newList = new ShoppingList();
                newList.ListName = DialogData.Text;

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
            }
            DialogData.Text = "";
            DisableRect.Visibility = Visibility.Collapsed;
            DialogRect.Visibility = Visibility.Collapsed;
        }

        private void ButtonDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogData.Text = "";
            DisableRect.Visibility = Visibility.Collapsed;
            DialogRect.Visibility = Visibility.Collapsed;
        }
    }
}