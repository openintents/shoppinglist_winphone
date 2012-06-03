using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace OIShoppingListWinPhone
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 1", Tag = "Tag item 1", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 2", Tag = "Tag item 2", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 3", Tag = "Tag item 3", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 4", Tag = "Tag item 4", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 5", Tag = "Tag item 5", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 6", Tag = "Tag item 6", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 7", Tag = "Tag item 7", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 8", Tag = "Tag item 8", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 9", Tag = "Tag item 9", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 10", Tag = "Tag item 10", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 11", Tag = "Tag item 11", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 12", Tag = "Tag item 12", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 13", Tag = "Tag item 13", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 14", Tag = "Tag item 14", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 15", Tag = "Tag item 15", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            this.Items.Add(new ItemViewModel() { Check = true, ItemName = "List item 16", Tag = "Tag item 16", Quantity = 1, Units = 2, Priority = "-1-", Price = 5.45F }); //"Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });

            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}