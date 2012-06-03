using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace OIShoppingListWinPhone
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _itemName;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string ItemName
        {
            get
            {
                return _itemName;
            }
            set
            {
                if (value != _itemName)
                {
                    _itemName = value;
                    NotifyPropertyChanged("LineOne");
                }
            }
        }

        private string _tag;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                if (value != _tag)
                {
                    _tag = value;
                    NotifyPropertyChanged("LineTwo");
                }
            }
        }

        private string _priority;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                if (value != _priority)
                {
                    _priority = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
        }

        private int _quantity;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (value != _quantity)
                {
                    _quantity = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
        }

        private int _units;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public int Units
        {
            get
            {
                return _units;
            }
            set
            {
                if (value != _units)
                {
                    _units = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
        }

        private float _price;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public float Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (value != _price)
                {
                    _price = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
        }

        private bool _check;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public bool Check
        {
            get
            {
                return _check;
            }
            set
            {
                if (value != _check)
                {
                    _check = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
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