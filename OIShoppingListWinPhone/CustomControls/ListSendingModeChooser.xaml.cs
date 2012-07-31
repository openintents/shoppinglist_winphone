using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace OIShoppingListWinPhone.CustomLayout
{
	public partial class ListSendingModeChooser : UserControl
	{
		public ListSendingModeChooser()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        /// <summary>
        /// Displaying (or 'activating') the control on the screen
        /// </summary>
        public void Activate()
        {
            this.Visibility = System.Windows.Visibility.Visible;
            VisualStateManager.GoToState(this, "VisibleVisualState", true);
        }
	}
}