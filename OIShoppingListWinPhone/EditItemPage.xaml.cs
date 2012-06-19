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

namespace OIShoppingListWinPhone
{
    public partial class AddNewListPage : PhoneApplicationPage
    {
        private int itemID;

        public AddNewListPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("ID"))
            {
                this.itemID = Convert.ToInt32(NavigationContext.QueryString["ID"].ToString());
                this.itemName.Text = NavigationContext.QueryString["Name"].ToString();
                if (NavigationContext.QueryString["Quantity"].ToString() != "-1")
                    this.itemQuantity.Text = NavigationContext.QueryString["Quantity"].ToString();
                if (NavigationContext.QueryString["Units"].ToString() != "-1")
                    this.itemUnits.Text = NavigationContext.QueryString["Units"].ToString();
                this.itemPrice.Text = NavigationContext.QueryString["Price"].ToString();
                this.itemTag.Text = NavigationContext.QueryString["Tag"].ToString();
                if (NavigationContext.QueryString["Priority"].ToString() != "-1")
                    this.itemPriority.Text = NavigationContext.QueryString["Priority"].ToString();
                this.itemName.Focus();
            }
        }

        private void Button_Click_OK(object sender, RoutedEventArgs e)
        {
            string queryBody = "?ID=" + this.itemID.ToString()
                + "&Name=" + this.itemName.Text
                + "&Quantity=" + this.itemQuantity.Text
                + "&Units=" + this.itemUnits.Text
                + "&Price=" + this.itemPrice.Text
                + "&Tag=" + this.itemTag.Text
                + "&Priority=" + this.itemPriority.Text
                + "&Note=";
            NavigationService.RemoveBackEntry();
            NavigationService.Navigate(new Uri("/MainPage.xaml" + queryBody, UriKind.Relative));
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}