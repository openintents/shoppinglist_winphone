﻿#pragma checksum "C:\Users\Alex\Documents\GitHub\shoppinglist_winphone\OIShoppingListWinPhone\EditItemPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2EF924E5A5CA2FD701870DF3BF89A7EC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace OIShoppingListWinPhone {
    
    
    public partial class AddNewListPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox itemName;
        
        internal System.Windows.Controls.TextBox itemQuantity;
        
        internal System.Windows.Controls.TextBox itemUnits;
        
        internal System.Windows.Controls.TextBlock colonTextBlock;
        
        internal System.Windows.Controls.TextBlock totalItemsPrice;
        
        internal System.Windows.Controls.TextBox itemPrice;
        
        internal System.Windows.Controls.TextBox itemTag;
        
        internal System.Windows.Controls.TextBox itemPriority;
        
        internal System.Windows.Controls.Button ButtonNote;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/OIShoppingListWinPhone;component/EditItemPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.itemName = ((System.Windows.Controls.TextBox)(this.FindName("itemName")));
            this.itemQuantity = ((System.Windows.Controls.TextBox)(this.FindName("itemQuantity")));
            this.itemUnits = ((System.Windows.Controls.TextBox)(this.FindName("itemUnits")));
            this.colonTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("colonTextBlock")));
            this.totalItemsPrice = ((System.Windows.Controls.TextBlock)(this.FindName("totalItemsPrice")));
            this.itemPrice = ((System.Windows.Controls.TextBox)(this.FindName("itemPrice")));
            this.itemTag = ((System.Windows.Controls.TextBox)(this.FindName("itemTag")));
            this.itemPriority = ((System.Windows.Controls.TextBox)(this.FindName("itemPriority")));
            this.ButtonNote = ((System.Windows.Controls.Button)(this.FindName("ButtonNote")));
        }
    }
}

