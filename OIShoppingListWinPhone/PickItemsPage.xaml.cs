﻿using System;
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

namespace OIShoppingListWinPhone
{
    public partial class PickItemsPage : PhoneApplicationPage
    {
        public PickItemsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("ListId"))
            {
                int id = Convert.ToInt32(NavigationContext.QueryString["ListId"]);
                this.DataContext = App.ViewModel.ShoppingLists.Where(l => l.ListID == id).FirstOrDefault();
            }
        }
    }
}