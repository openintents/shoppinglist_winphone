﻿#pragma checksum "C:\Users\Alex\Documents\GitHub\shoppinglist_winphone\OIShoppingListWinPhone\PivotItemControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EACD325FDD026815F5FC3C7FC10C9775"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
    
    
    public partial class PivotItemControl : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.CheckBox itemCheck;
        
        internal System.Windows.Controls.TextBlock itemPriority;
        
        internal System.Windows.Controls.TextBlock itemQuantity;
        
        internal System.Windows.Controls.TextBlock itemUnits;
        
        internal System.Windows.Controls.TextBlock itemName;
        
        internal System.Windows.Controls.TextBlock itemTag;
        
        internal System.Windows.Controls.TextBlock itemPrice;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/OIShoppingListWinPhone;component/PivotItemControl.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.itemCheck = ((System.Windows.Controls.CheckBox)(this.FindName("itemCheck")));
            this.itemPriority = ((System.Windows.Controls.TextBlock)(this.FindName("itemPriority")));
            this.itemQuantity = ((System.Windows.Controls.TextBlock)(this.FindName("itemQuantity")));
            this.itemUnits = ((System.Windows.Controls.TextBlock)(this.FindName("itemUnits")));
            this.itemName = ((System.Windows.Controls.TextBlock)(this.FindName("itemName")));
            this.itemTag = ((System.Windows.Controls.TextBlock)(this.FindName("itemTag")));
            this.itemPrice = ((System.Windows.Controls.TextBlock)(this.FindName("itemPrice")));
        }
    }
}

