using System;
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
	public partial class SendModeChooser : UserControl
	{
		public SendModeChooser()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        public void Activate()
        {
            this.Visibility = System.Windows.Visibility.Visible;

            VisualStateManager.GoToState(this, "VisibleVisualState", true);
        }

        public void Deactivate()
        {
            VisualStateManager.GoToState(this, "CollapsedVisualState", true);
        }
	}
}