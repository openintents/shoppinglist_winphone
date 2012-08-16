using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls;

using OIShoppingListWinPhone.DataModel;

namespace OIShoppingListWinPhone.CustomLayout
{
    public partial class CustomFilterListControl : UserControl
    {
        //Bool flag for displaying whether control is loaded or not
        private bool bLoaded = false;

        public CustomFilterListControl()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(PivotItemControl_Loaded);
        }

        void PivotItemControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Set ListPickers selections according to all settings
            if (ListSelector.Items.Count > App.Settings.SelectedListIndexSetting)
            {
                //Set ListSelector selection according to SelectedListIndexSetting
                ListSelector.SelectedIndex = App.Settings.SelectedListIndexSetting;               
                ShoppingList curList = ListSelector.SelectedItem as ShoppingList;

                TagsSelector.ItemsSource = curList.Tags;
                TagsSelector.SelectedItem = curList.FilterTag;

                StoreSelector.ItemsSource = curList.ListStoreLabels;
                StoreSelector.SelectedItem = curList.FilterStore;
            }
            //Changing flag to 'true' -> the page is loaded
            this.bLoaded = true;
        }

        private void ListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Changing app settings with ListSelector.SelectedIndex changing
            if (ListSelector != null && this.bLoaded)
            {
                App.Settings.SelectedListIndexSetting = ListSelector.SelectedIndex;

                ShoppingList curList = ListSelector.SelectedItem as ShoppingList;

                TagsSelector.ItemsSource = curList.Tags;
                TagsSelector.SelectedItem = curList.FilterTag;

                StoreSelector.ItemsSource = curList.ListStoreLabels;
                StoreSelector.SelectedItem = curList.FilterStore;
            }
        }

        private void TagsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Changing list filter tag with TagsSelector.SelectedIndex changing
            if (TagsSelector != null && this.bLoaded)
                App.ViewModel.UpdateListFilterTag(ListSelector.SelectedItem as ShoppingList,
                    (string)TagsSelector.SelectedItem);
        }

        private void StoreSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Changing list filter store with StoreSelector.SelectedIndex changing
            if (StoreSelector != null && this.bLoaded)
                App.ViewModel.UpdateListFilterStore(ListSelector.SelectedItem as ShoppingList,
                    (string)StoreSelector.SelectedItem);
        }
    }
}
