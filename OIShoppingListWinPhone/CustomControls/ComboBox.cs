﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace OIShoppingListWinPhone.CustomControls
{
    public class ComboBox : System.Windows.Controls.ComboBox
    {
        public ComboBox()
        {
            // Insert code required on object creation below this point.
            this.DefaultStyleKey = typeof(ComboBox);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
