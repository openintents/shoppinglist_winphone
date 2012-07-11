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

using OIShoppingListWinPhone.DataModel;
using OIShoppingListWinPhone.ViewModel;

namespace OIShoppingListWinPhone
{
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
