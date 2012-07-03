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
    public sealed class AlphabeticComparer : IComparer<ShoppingListItem>
    {
        public int Compare(ShoppingListItem first, ShoppingListItem second)
        {
            return 0;
        }
    }

    public static class ShoppingUtils
    {
        
    }    
}
