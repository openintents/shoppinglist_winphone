using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using System.Globalization;

using OIShoppingListWinPhone.DataModel;
using OIShoppingListWinPhone.ViewModel;

namespace OIShoppingListWinPhone.Utils
{
    public sealed class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
                return (int)value == 1 ? true : false;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return (bool)value ? 1 : 0;

            return 0;
        }
    }

    /// <summary>
    /// Float to Visibility converter.
    /// Using as Binding Converter for CheckedPrice and TotalPrice
    ///  - if item price = 0 => Visibility = Collapsed, else => Visibility = Visible
    /// </summary>
    public sealed class FloatToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float)
             return (float)value == 0.0F ? Visibility.Collapsed : Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// Float to Visibility converter.
    /// Using as Binding Converter for Grid.Height (to rebuild the layout)
    ///  - if TotalPrice = 0 => Grid.Height = 0, else => Grid.Height = 25
    /// </summary>
    public sealed class FloatToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float)
                return (float)value == 0.0F ? 0 : 25;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// Bool to Visibility 'positive' converter.
    /// When bool value = true => Visibility = Visible,
    /// else => Visibility = Collapsed
    /// </summary>
    public sealed class BoolToVisibilityPositiveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// Bool to Visibility 'negative' converter.
    /// When bool value = true => Visibility = Collapsed,
    /// else => Visibility = Visible
    /// </summary>
    public sealed class BoolToVisibilityNegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public sealed class UncheckedFirst_Alphabetical : IComparer<ShoppingListItem>
    {
        public int Compare(ShoppingListItem first, ShoppingListItem second)
        {
            if (first.Status > second.Status)
                return 1;
            else if (first.Status < second.Status)
                return -1;
            else
                return first.ItemName.CompareTo(second.ItemName);
        }
    }

    public sealed class NewestFirst : IComparer<ShoppingListItem>
    {
        public int Compare(ShoppingListItem first, ShoppingListItem second)
        {
            return 0 - first.CreatedDate.CompareTo(second.CreatedDate);
        }
    }
    
    public sealed class MostExpensiveFirst : IComparer<ShoppingListItem>
    {
        public int Compare(ShoppingListItem first, ShoppingListItem second)
        {
            return 0 - first.Price.CompareTo(second.Price);
        }
    }

    public sealed class Priority_TagsAlphabetical : IComparer<ShoppingListItem>
    {
        public int Compare(ShoppingListItem first, ShoppingListItem second)
        {
            if (first.Priority > second.Priority)
                return 1;
            else if (first.Priority < second.Priority)
                return -1;
            else
                return first.Tag.CompareTo(second.Tag);
        }
    }

    public sealed class UncheckedFirst_TagsAlphabetical : IComparer<ShoppingListItem>
    {
        public int Compare(ShoppingListItem first, ShoppingListItem second)
        {
            if (first.Status > second.Status)
                return 1;
            else if (first.Status < second.Status)
                return -1;
            else
                return first.Tag.CompareTo(second.Tag);
        }
    }

    public sealed class UncheckedFirst_Priority_Alphabetical : IComparer<ShoppingListItem>
    {
        public int Compare(ShoppingListItem first, ShoppingListItem second)
        {
            if (first.Status > second.Status)
                return 1;
            else if (first.Status < second.Status)
                return -1;
            else
            {
                if (first.Priority > second.Priority)
                    return 1;
                else if (first.Priority < second.Priority)
                    return -1;
                else
                    return first.ItemName.CompareTo(second.ItemName);
            }
        }
    }
        
    public sealed class UnckeckedFirst_Priority_TagsAlphabetical : IComparer<ShoppingListItem>
    {
        public int Compare(ShoppingListItem first, ShoppingListItem second)
        {
            if (first.Status > second.Status)
                return 1;
            else if (first.Status < second.Status)
                return -1;
            else
            {
                if (first.Priority > second.Priority)
                    return 1;
                else if (first.Priority < second.Priority)
                    return -1;
                else
                    return first.Tag.CompareTo(second.Tag);
            }
        }
    }

    public static class ShoppingUtils
    {
        
    }    
}
