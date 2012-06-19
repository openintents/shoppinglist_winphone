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
    public partial class DialogNameControl : UserControl
    {
        public DialogNameControl()
        {
            InitializeComponent();
        }

        public void Activate(string dialogLabel, string dialogData)
        {
            this.DialogLabel.Text = dialogLabel;
            this.DialogData.Text = dialogData;
            this.Visibility = System.Windows.Visibility.Visible;

            VisualStateManager.GoToState(this, "VisibleVisualState", true);
            this.DialogData.Focus();
        }

        public void Deactivate()
        {            
            VisualStateManager.GoToState(this, "CollapsedVisualState", true);
            this.DialogData.Text = "";
        }
    }
}
