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
using Microsoft.Phone.Tasks;

using OIShoppingListWinPhone.DataModel;
using OIShoppingListWinPhone.ViewModel;
using OIShoppingListWinPhone.CustomControls;

namespace OIShoppingListWinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        private DialogNameControl dlgName;
        private SendModeChooser chooser;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
                        
            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            dlgName = DialogName as DialogNameControl;
            //chooser = SendChooser as SendModeChooser;
            dlgName.ButtonOK.Click += new RoutedEventHandler(ButtonOK_Click);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            if (LayoutRoot.Children.Count == 3)
            {
                Control c = LayoutRoot.Children[0] as Control;
                LayoutRoot.Children.RemoveAt(0);
            }

            if (App.AppSettings.Filters)
            {
                Control elem = new PivotItemControl();
                LayoutRoot.Children.Insert(0, elem);
            }
            else
            {
                Control elem = new CustomPivotControl();
                LayoutRoot.Children.Insert(0, elem);
            }
        }
        
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            if (newListItemName.Text.Trim() != String.Empty)
            {
                DependencyObject listContainer = LayoutRoot.Children[0];
                Grid grid = VisualTreeHelper.GetChild(listContainer, 0) as Grid;
                UIElement obj = grid.Children[0] as ItemsControl;
                ShoppingList currentList = new ShoppingList();
                if (obj is Pivot)
                    currentList = (obj as Pivot).SelectedItem as ShoppingList;
                else if (obj is ListPicker)
                    currentList = (obj as ListPicker).SelectedItem as ShoppingList;
                else
                    MessageBox.Show("Unrecognized control", "Exception", MessageBoxButton.OK);
                                
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
                    Note = string.Empty,
                };
                
                App.ViewModel.AddNewListItem(currentList, newListItem);
                newListItemName.Text = "";
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
                /*Pivot pivot = this.PivotControl as Pivot;
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
                }*/
            }
            else if (dlgName.DialogLabel.Text == "Enter name of new shopping list")
            {
                if (dlgName.DialogData.Text.Trim() != String.Empty)
                {
                    ShoppingList newList = new ShoppingList();
                    newList.ListName = dlgName.DialogData.Text;

                    App.ViewModel.AddNewList(newList);
                    
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
            /*ShoppingList renameList = PivotControl.SelectedItem as ShoppingList;
            
            dlgName.Activate("Enter new name of the shopping list", renameList.ListName);*/
        }

        private void ApplicationBarIconButtonSendList_Click(object sender, EventArgs e)
        {
            //Send shopping list via SMS or E-mail
            chooser = new SendModeChooser();
            LayoutRoot.Children.Add(chooser);
            chooser.buttonSMS.Click += new RoutedEventHandler(buttonSMS_Click);
            chooser.buttonEmail.Click += new RoutedEventHandler(buttonEmail_Click);
            chooser.Activate();
        }

        void buttonSMS_Click(object sender, RoutedEventArgs e)
        {            
            SmsComposeTask smsTask = new SmsComposeTask();
            smsTask.Body = CreateMessageBody();
            smsTask.Show();            
            chooser.Deactivate();
            LayoutRoot.Children.Remove(chooser);
        }

        void buttonEmail_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailTask = new EmailComposeTask();
            emailTask.Body = CreateMessageBody();
            emailTask.Show();
            chooser.Deactivate();
            LayoutRoot.Children.Remove(chooser);
        }

        private void ApplicationBarIconButtonDeleteList_Click(object sender, EventArgs e)
        {            
            /*ShoppingList delList = PivotControl.SelectedItem as ShoppingList;
            if (MessageBox.Show("Are you sure you want to delete " + delList.ListName + " shopping list?", "Warning", MessageBoxButton.OKCancel)
                == MessageBoxResult.OK)
                App.ViewModel.DeleteList(delList);*/
        }

        #endregion

        private string CreateMessageBody()
        {
            DependencyObject listContainer = LayoutRoot.Children[0];
            Grid grid = VisualTreeHelper.GetChild(listContainer, 0) as Grid;
            UIElement obj = grid.Children[0] as ItemsControl;
            ShoppingList currentList = new ShoppingList();
            if (obj is Pivot)
                currentList = (obj as Pivot).SelectedItem as ShoppingList;
            else if (obj is ListPicker)
                currentList = (obj as ListPicker).SelectedItem as ShoppingList;
            else
                MessageBox.Show("Unrecognized control", "Exception", MessageBoxButton.OK);

            String body = String.Empty;

            if (currentList != null)
            {
                foreach (ShoppingListItem item in currentList.ListItems)
                {
                    if (item.Status <= 1)
                    {
                        body += String.Format("[{0}] ", item.Status == 0 ? " " : "X");
                        body += item.Quantity == null ? "" : item.Quantity + " ";
                        body += item.Units == null ? "" : item.Units + " ";
                        body += String.Format("{0} ", item.ItemName);

                        if (item.Tag != string.Empty || item.Price != 0.00F)
                        {
                            body += "()";
                            body = body.Insert(body.Length - 1, item.Tag == string.Empty ? "" : item.Tag + " ");
                            body = body.Insert(body.Length - 1, item.Price == 0.00F ? "" : String.Format("{0:F2}", item.Price));
                        }
                        body += "\n";
                    }
                }
            }

            return body;
        }

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
        
        

        
    }
}