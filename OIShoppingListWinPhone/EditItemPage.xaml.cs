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
using OIShoppingListWinPhone.CustomLayout;

namespace OIShoppingListWinPhone
{
    public partial class AddNewListPage : PhoneApplicationPage
    {
        //Item ID of currently editing list item
        private int itemID;
        //List ID of currently editing list item
        private int listID;
        //Item Note of currently editing list item
        private string itemNote = string.Empty;
        //Edit item note dialog control
        private EditItemNoteDialog noteDialog;

        public AddNewListPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //If the navigation is MainPage -> EditItemPage, than QueryString
            //contain item's ID - ["ID"] key
            if (NavigationContext.QueryString.ContainsKey("ID"))
            {
                //If ["ID"] key exist - than get and displaying all next items parameters
                this.itemID = Convert.ToInt32(NavigationContext.QueryString["ID"].ToString());
                this.listID = Convert.ToInt32(NavigationContext.QueryString["ListID"].ToString());

                ShoppingList list = App.ViewModel.ShoppingLists.FirstOrDefault(l => l.ListID == listID);
                ShoppingListItem item = list.ListItems.FirstOrDefault(i => i.ItemID == itemID);

                this.itemNote = item.Note;
                this.itemName.Text = item.ItemName;                
                this.itemTag.Text = item.Tag;
                this.itemQuantity.Text = item.Quantity.ToString();
                this.itemPrice.Text = String.Format("{0:F2}", item.Price);
                if (item.Quantity != null)
                    this.itemPrice.Text = String.Format("{0:F2}", item.Price / item.Quantity);
                this.itemUnits.Text = item.Units;
                this.itemPriority.Text = item.Priority.ToString();                
            }
        }

        private void ApplicationBarIconButtonSave_Click(object sender, EventArgs e)
        {
            int? quantity = null;
            if (this.itemQuantity.Text != string.Empty)
                quantity = Convert.ToInt32(this.itemQuantity.Text);

            int? priority = null;
            if (this.itemPriority.Text != string.Empty)
                priority = Convert.ToInt32(this.itemPriority.Text);

            float price = 0.00F;
            price = (float)Convert.ToDouble(this.itemPrice.Text);
            //float.TryParse(this.itemPrice.Text, out price);
            if (totalItemsPrice.Visibility == System.Windows.Visibility.Visible)
                price = (float)Convert.ToDouble(this.totalItemsPrice.Text);
                //float.TryParse(this.totalItemsPrice.Text, out price);

            App.ViewModel.UpdateListItem(this.listID,
                this.itemID,
                this.itemName.Text,
                quantity,
                this.itemUnits.Text,
                price,
                this.itemTag.Text,
                priority,
                this.itemNote);

            MessageBox.Show("Data was successfully saved", "Information", MessageBoxButton.OK);
            NavigationService.GoBack();
        }

        //Make float input scope for Price TextBox
        private void itemPrice_KeyUp(object sender, KeyEventArgs e)
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
       
            //Update TextBlock 'TotalItemsPrice' text
            this.UpdatePriceDisplayingText();
        }

        //Item Price Text Changed event handler. Using for updating displaying items price
        private void itemPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Update TextBlock 'TotalItemsPrice' text
            this.UpdatePriceDisplayingText();
        }

        //Update TextBlock 'TotalItemsPrice' text 
        private void UpdatePriceDisplayingText()
        {
            //Checking inputed information and update price text block's visibility
            //and actually text
            float f = 0.00F;
            float.TryParse(itemPrice.Text, out f);
            int q = 0;
            Int32.TryParse(itemQuantity.Text, out q);
            if (q > 0 && f > 0.00F)
            {
                colonTextBlock.Visibility = System.Windows.Visibility.Visible;
                totalItemsPrice.Visibility = System.Windows.Visibility.Visible;
                totalItemsPrice.Text = String.Format("{0:F2}", f * Convert.ToInt32(itemQuantity.Text));
            }
            else
            {
                colonTextBlock.Visibility = System.Windows.Visibility.Collapsed;
                totalItemsPrice.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        //Make digits input scope for Quantity TextBox
        private void NumericKeyUp(object sender, KeyEventArgs e)
        {
            this.NumericInput(sender);
            //Update TextBlock 'TotalItemsPrice' text
            this.UpdatePriceDisplayingText();
        }

        //Item Quantity Text Changed event handler. Using for updating displaying items price
        private void itemQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Update TextBlock 'TotalItemsPrice' text
            this.UpdatePriceDisplayingText();
        }

        //Make digits input scope for Priority TextBox
        private void itemPriority_KeyUp(object sender, KeyEventArgs e)
        {
            this.NumericInput(sender);
        }

        //Make digits input for 'sender' TextBox
        private void NumericInput(object sender)
        {
            TextBox txt = (TextBox)sender;
            //Deleting all '.' symbols from the string
            if (txt.Text.Contains('.'))
            {
                txt.Text = txt.Text.Replace(".", "");
                txt.SelectionStart = txt.Text.Length;
            }
            //Deleting all ',' symbols from the string
            else if (txt.Text.Contains(','))
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
        }

        //Initialising Edit item note Dialog and activate it
        private void ButtonNote_Click(object sender, RoutedEventArgs e)
        {
            //Initialising all necessary event handlers of Edit item note dialog
            noteDialog = new EditItemNoteDialog();
            noteDialog.ButtonOK.Click += new RoutedEventHandler(ButtonOK_Click);
            noteDialog.ButtonCancel.Click += new RoutedEventHandler(ButtonCancel_Click);
            noteDialog.CollapsedVisualState.Storyboard.Completed += new EventHandler(Storyboard_Completed);

            //Actually activating of the dialog
            noteDialog.DialogData.Text = this.itemNote;
            ApplicationBar.IsVisible = false;
            LayoutRoot.Children.Add(noteDialog);
            noteDialog.Activate();
        }

        //Back Key Press handler for deactivating Edit item note dialog
        //with if it's active
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (LayoutRoot.Children.Contains(noteDialog))
            {
                //Deactivating dialog and supressing NavigationService.GoBack() navigation
                noteDialog.Deactivate();
                e.Cancel = true;
            }

            base.OnBackKeyPress(e);
        }
        
        #region Note Dialog event handlers

        //Dialog button 'ok' click event
        void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            //Saving item note in private field of this class and deactivating the dialog
            this.itemNote = noteDialog.DialogData.Text;
            noteDialog.CollapsedVisualState.Storyboard.Begin();
        }

        //Dialog button 'cancel' click event
        void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            //Just deactivating the dialog
            noteDialog.CollapsedVisualState.Storyboard.Begin();
        }

        //Completing storyboard event handler
        void Storyboard_Completed(object sender, EventArgs e)
        {
            //Removing dialog from the 'LayoutRoot' grid
            //and making ApplicationBar visible
            ApplicationBar.IsVisible = true;
            LayoutRoot.Children.Remove(noteDialog);
        }    

        #endregion
    }
}