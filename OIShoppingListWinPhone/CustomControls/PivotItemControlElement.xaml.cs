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

namespace OIShoppingListWinPhone
{
    public partial class PivotItemControlElement : UserControl
    {
        /*#region Variables for Control's displaying

        public string ItemName { get; set; }

        public Boolean Status { get; set; }

        public Decimal Price { get; set; }

        public int Quantity { get; set; }

        public int Units { get; set; }

        public int Priority { get; set; }

        public new string Tag { get; set; }

        public string Note {get; set; }

        #endregion*/

        public PivotItemControlElement(OIShoppingListWinPhone.DataModel.ShoppingListItem listItem)
        {
            InitializeComponent();

            itemCheck.IsChecked = listItem.Status;
            itemPriority.Text = String.Format("-{0}-", listItem.Priority);
            itemQuantity.Text = listItem.Quantity.ToString();
            itemUnits.Text = listItem.Units.ToString();
            itemName.Text = listItem.ItemName;
            itemPrice.Text = String.Format("{0:F2}", listItem.Price);
            itemTag.Text = listItem.Tag;
        }
    }
}
