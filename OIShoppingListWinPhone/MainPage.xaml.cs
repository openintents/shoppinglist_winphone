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
using OIShoppingListWinPhone.CustomLayout;

namespace OIShoppingListWinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        //Private control for input new list's name and edeting name of existing list
        private EditNameDialog editNameDlg;
        //Private control for choosing list sending mode (SMS or E-mail)
        private ListSendingModeChooser sendModeChooser;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            //Set the MainPage.Loaded handler
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            //Checking loading data from database
            if (!App.ViewModel.IsDataLoaded)
            {
                //If data is not loaded from database than - Load
                App.ViewModel.LoadData();
            }
            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            //If there is an empty database (first start of application?)?
            if (App.ViewModel.ShoppingLists.Count == 0)
                //Than add new empty list with "my new list" name
                App.ViewModel.AddNewList(new ShoppingList() { ListName = "my new list" });

            //Choosing what layout to display on the MainPage of application - Pivot or Filtered list.
            //For Pivot layout using CustomPivotControl, for Filtered list - CustomFilteredListControl.
            //All this controls have specific layout to display Lists and list items in corresponding way
            //
            //With the start of application 'LaytouRoot' grid has one child
            // - it's a grid, that contain 'Add' button and TaxtBox to input new item's name.
            //Thus, for display the shopping lists from database it's necessary to add the control
            //with corresponding layout.
            if ((bool)App.Settings.FiltersSettings)
            {
                //Using Pivot layout for the MainPage
                Control elem = new CustomFilterListControl();
                LayoutRoot.Children.Insert(0, elem);
            }
            else
            {
                //Using Filtered list layout
                Control elem = new CustomPivotControl();
                LayoutRoot.Children.Insert(0, elem);
            }
        }
        
        //This function handles MainPage.Loaded event.
        //This event fires, when navigate to MainPage with NavigationService.GoBake()
        //or NavigationService.Navigate(). Such navigation appear when the user navigates
        //from EditItemPage, SettingsPage, SkyDrive etc.
        //The most important navigation is - MainPage <- SettingsPage, because there user select
        //Pivot display mode or Filter list mode.
        //In this case it's necessary to update layout if it is not valid.
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {            
            //To choose what layout to display (Pivot control or Filtered list)
            //at first check application settings and check current layout.
            //If current layout is invalid - change layout with deleting old
            //displaying control and inserting new control for corresponding settings.
            if (LayoutRoot.Children.Count == 1)
            {
                if ((bool)App.Settings.FiltersSettings)
                {
                    //Using Pivot layout for the MainPage
                    Control elem = new CustomFilterListControl();
                    LayoutRoot.Children.Insert(0, elem);
                }
                else
                {
                    //Using Filtered list layout
                    Control elem = new CustomPivotControl();
                    LayoutRoot.Children.Insert(0, elem);
                }
            }
            else if (LayoutRoot.Children.Count == 2)
            {
                Control currentControl = LayoutRoot.Children[0] as Control;
                Type type = currentControl.GetType();

                //Displaying Pivot control
                if ((bool)App.Settings.FiltersSettings && type == typeof(CustomPivotControl))
                {
                    LayoutRoot.Children.RemoveAt(0);
                    Control newControl = new CustomFilterListControl();
                    LayoutRoot.Children.Insert(0, newControl);
                }
                //Displaying Filtered list
                else if (!(bool)App.Settings.FiltersSettings && type == typeof(CustomFilterListControl))
                {
                    LayoutRoot.Children.RemoveAt(0);
                    Control newControl = new CustomPivotControl();
                    LayoutRoot.Children.Insert(0, newControl);
                }
                else
                {
                    //Nothing to do, because MainPage layout is valid
                }
            }
            //If 'LayoutRoot' grid contain more than 2 children, than we have an exception
            //with current layout
            else
            {
                throw new Exception("There is an exception in the page's layout");
            }
        }       
        
        #region ApplicationBarIconButton's Click Events

        //Activating the dialog for inputing the name of new shopping list
        private void ApplicationBarIconButtonNewList_Click(object sender, EventArgs e)
        {
            editNameDlg = new EditNameDialog();

            //Handlers for processing dialog's events
            editNameDlg.ButtonOK.Click += new RoutedEventHandler(ButtonOK_Click);
            editNameDlg.ButtonCancel.Click += new RoutedEventHandler(ButtonCancel_Click);
            editNameDlg.CollapsedVisualState.Storyboard.Completed += new EventHandler(Storyboard_Completed);
            
            //Actually activating the dialog
            LayoutRoot.Children.Add(editNameDlg);
            editNameDlg.Activate(EditNameDialog.EditNameDialogMode.AddingNewList);
            ApplicationBar.IsVisible = false;
        }            

        //Activating the dialog for inputing new name of the current shopping list
        private void ApplicationBarIconButtonRenameList_Click(object sender, EventArgs e)
        {
            ShoppingList renameList = this.GetCurrentShoppingList();
            if (renameList != null)
            {
                editNameDlg = new EditNameDialog();

                //Handlers for processing dialog's events
                editNameDlg.ButtonOK.Click += new RoutedEventHandler(ButtonOK_Click);
                editNameDlg.ButtonCancel.Click += new RoutedEventHandler(ButtonCancel_Click);
                editNameDlg.CollapsedVisualState.Storyboard.Completed += new EventHandler(Storyboard_Completed);

                //Actually activating the dialog
                LayoutRoot.Children.Add(editNameDlg);
                editNameDlg.Activate(EditNameDialog.EditNameDialogMode.RenamingList, renameList.ListName);
                ApplicationBar.IsVisible = false;                
            }
        }

        //Activating the dialog for selecting sending list mode
        private void ApplicationBarIconButtonSendList_Click(object sender, EventArgs e)
        {
            //Getting the selected current shopping list
            ShoppingList currentList = this.GetCurrentShoppingList();

            if (currentList != null)
            {
                //Send shopping list via SMS or E-mail
                ApplicationBar.IsVisible = false;
                sendModeChooser = new ListSendingModeChooser();

                //Handlers for processing dialog's events
                sendModeChooser.buttonSMS.Click += new RoutedEventHandler(buttonSMS_Click);
                sendModeChooser.buttonEmail.Click += new RoutedEventHandler(buttonEmail_Click);
                sendModeChooser.CollapsedVisualState.Storyboard.Completed += 
                    new EventHandler(ListSendingModeChooserStoryboard_Completed);
                //Activating the dialog
                LayoutRoot.Children.Add(sendModeChooser);
                sendModeChooser.Activate();
                ApplicationBar.IsVisible = false;
            }
        }       

        //Deleting selected shopping list
        private void ApplicationBarIconButtonDeleteList_Click(object sender, EventArgs e)
        {
            //Selecting the list for deleting            
            ShoppingList delList = this.GetCurrentShoppingList();

            if (delList != null)
            {
                if (MessageBox.Show("Are you sure you want to delete '" + delList.ListName + "' shopping list?", "Deleting list", MessageBoxButton.OKCancel)
                    == MessageBoxResult.OK)
                    App.ViewModel.DeleteList(delList);
            }

            //If there is an empty database (all lists were deleted?)?
            if (App.ViewModel.ShoppingLists.Count == 0)
                //Than add new empty list with "my new list" name
                App.ViewModel.AddNewList(new ShoppingList() { ListName = "my new list" });
        }

        #endregion

        #region EditNameDialog Events

        //With EditNameGialog 'ok' button click will check 'new list name' TextBox
        void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            //Is 'new list name' TextBox.Text Empty?
            if (editNameDlg.DialogData.Text.Trim() != String.Empty)
            {
                //If 'new list name' TextBox.Text is NOT empty,
                //than create the collection of exist shopping list's names.
                //This collection will be checked for containing the 'new list name'.
                var listsNames = from ShoppingList list in App.ViewModel.ShoppingLists
                                 select list.ListName;
                string newListName = editNameDlg.DialogData.Text.Trim();

                //If 'new list name' already exist in this collection,
                //than it will be selected (currently displaying) the list with this name.
                if (listsNames.Contains(newListName))
                {
                    ShoppingList list = App.ViewModel.ShoppingLists.FirstOrDefault(x => x.ListName == newListName);
                    this.SetSelectedList(list);
                    editNameDlg.Deactivate();
                }
                //If 'new list name' DOES NOT exist in this collection,
                //than it will be renaming or adding new shopping list (accordingly to the EditNameDialogMode)
                else
                {
                    //Renaming exeisting shopping list and deactivating the control
                    if (editNameDlg.DialogMode == (int)EditNameDialog.EditNameDialogMode.RenamingList)
                    {
                        ShoppingList renameList = this.GetCurrentShoppingList();
                        App.ViewModel.RenameList(renameList, newListName);
                        editNameDlg.Deactivate();                        
                    }
                    //Adding new shopping list and deactivating the control
                    else if (editNameDlg.DialogMode == (int)EditNameDialog.EditNameDialogMode.AddingNewList)
                    {
                        ShoppingList newList = new ShoppingList() { ListName = newListName };
                        App.ViewModel.AddNewList(newList);
                        this.SetSelectedList(newList);
                        editNameDlg.Deactivate();
                    }
                }
            }
            //If 'new list name' TextBox.Text IS empty -> MessageBox for informating user
            //to input correct data.
            else
                MessageBox.Show("Please, input correct list's name" + "\n\n" +
                                "*Note: Name must not be empty", "Information", MessageBoxButton.OK);
        }

        //With 'cancel' button click - deactivating the dialog
        void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            editNameDlg.Deactivate();
        }

        //Removing the dialog from the 'LayoutRoot' after the completing 'CollapsedVisualState'
        //storyboard animation
        void Storyboard_Completed(object sender, EventArgs e)
        {
            LayoutRoot.Children.Remove(editNameDlg);
            ApplicationBar.IsVisible = true;
        }    

        #endregion

        # region ListSendingModeChooser button's click events

        //Handler for SMS mode button's click
        void buttonSMS_Click(object sender, RoutedEventArgs e)
        {
            //Creating and showing SMS composer task
            SmsComposeTask smsTask = new SmsComposeTask();
            smsTask.Body = CreateMessageBody(this.GetCurrentShoppingList());
            smsTask.Show();
        }

        //Handler for E-mail mode button's click
        void buttonEmail_Click(object sender, RoutedEventArgs e)
        {
            //Creating and showing E-mail composer task
            EmailComposeTask emailTask = new EmailComposeTask();
            emailTask.Body = CreateMessageBody(this.GetCurrentShoppingList());
            emailTask.Show();
        }

        //Removing the dialog from the 'LayoutRoot' after the completing 'CollapsedVisualState'
        //storyboard animation
        void ListSendingModeChooserStoryboard_Completed(object sender, EventArgs e)
        {
            LayoutRoot.Children.Remove(sendModeChooser);
            ApplicationBar.IsVisible = true;
        }    

        #endregion

        /// <summary>
        /// Creating message body string from current list's entries
        /// </summary>
        /// <param name="currentList">Instance of the selected current shopping list</param>
        /// <returns>String body of the selected current shopping list</returns>
        private string CreateMessageBody(ShoppingList currentList)
        {
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

        //Button 'add' click event handler
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            //Check is the new item name string empty.
            //If 'no' - creating new list item with this name and adding it
            //to the current shopping list
            if (newListItemName.Text.Trim() != String.Empty)
            {
                ShoppingList currentList = this.GetCurrentShoppingList();
                if (currentList != null)
                {
                    //Actually creating new list item
                    ShoppingListItem newListItem = new ShoppingListItem()
                    {
                        ItemName = newListItemName.Text.Trim(),
                        List = currentList,
                        Priority = null,
                        Price = 0F,
                        Units = null,
                        Quantity = null,
                        Tag = string.Empty,
                        Status = (int)ShoppingListItem.StatusEnumerator.Unchecked,
                        Note = string.Empty,
                    };
                    //Adding new list item to the database and updating current list's entries
                    App.ViewModel.AddNewListItem(currentList, newListItem);
                }
                //Erasing 'new item's name' TextBox
                newListItemName.Text = "";
            }
            else
                MessageBox.Show("Please, input correct list's item name" + "\n\n" +
                                "*Note: Name must not be empty", "Information", MessageBoxButton.OK);
        }

        /// <summary>
        /// Function for getting current selected shopping list for corresponding MainPage's layout
        /// </summary>
        /// <returns>Instance of the selected current shopping list</returns>
        private ShoppingList GetCurrentShoppingList()
        {
            DependencyObject listContainer = LayoutRoot.Children[0];
            Grid grid = VisualTreeHelper.GetChild(listContainer, 0) as Grid;
            UIElement obj = grid.Children[0] as ItemsControl;
            ShoppingList currentList = new ShoppingList();
            if (obj is Pivot)
                return currentList = (obj as Pivot).SelectedItem as ShoppingList;
            else if (obj is ListPicker)
                return currentList = (obj as ListPicker).SelectedItem as ShoppingList;
            else
                return null;
        }

        /// <summary>
        /// Set shopping list selected - currently displaying on the MainPage of application
        /// </summary>
        /// <param name="list">ShoppingList instance, which have to be selceted
        ///  (currently displaying on the screen).</param>
        private void SetSelectedList(ShoppingList list)
        {
            DependencyObject listContainer = LayoutRoot.Children[0];
            Grid grid = VisualTreeHelper.GetChild(listContainer, 0) as Grid;
            UIElement obj = grid.Children[0] as ItemsControl;
            if (obj is Pivot)
                (obj as Pivot).SelectedItem = list;
            else if (obj is ListPicker)
                (obj as ListPicker).SelectedItem = list;
            else
                throw new Exception("Unrecognized control in the page's layout");
        }

        #region ApplicationBarMenu's Click Events

        private void ApplicationBarMenuAbout_Click(object sender, EventArgs e)
        {
            //Navigate to About page
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuSettings_Click(object sender, EventArgs e)
        {
            //Navigate to Settings page
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuSaveToSkyDrive_Click(object sender, EventArgs e)
        {            
            //Creating query body for navigation to SkyDrive page
            ShoppingList list = this.GetCurrentShoppingList();
            string queryBody = "?ListID=" + list.ListID.ToString();
            //Navigate to SkyDrive page
            NavigationService.Navigate(new Uri("/SkyDrivePage.xaml" + queryBody, UriKind.Relative));
        }

        private void ApplicationBarMenuMarkAllItems_Click(object sender, EventArgs e)
        {

        }

        private void ApplicationBarMenuCleanUpList_Click(object sender, EventArgs e)
        {
            
        }

        private void ApplicationBarMenuPickItems_Click(object sender, EventArgs e)
        {
            
        }

        #endregion

        //OnBackKeyPress using for checking - is such control as EditNameDialog, ListSendingModeChooser etc. in
        //active state. If it's active - than deactivate this control and supress GoBack() navigation.
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            //Deactivate EditNameDialog and supress GoBack() navigation
            if (LayoutRoot.Children.Contains(editNameDlg))
            {
                editNameDlg.Deactivate();
                e.Cancel = true;
            }
            //Deactivate ListSendingModeChooser and supress GoBack() navigation
            if (LayoutRoot.Children.Contains(sendModeChooser))
            {
                sendModeChooser.CollapsedVisualState.Storyboard.Begin();
                e.Cancel = true;
            }

            base.OnBackKeyPress(e);
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
    }
}