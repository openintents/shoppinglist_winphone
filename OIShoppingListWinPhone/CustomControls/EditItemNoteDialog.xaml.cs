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
	public partial class EditItemNoteDialog : UserControl
	{
		public EditItemNoteDialog()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        /// <summary>
        /// Displaying (or 'activating') the dialog on the screen.
        /// </summary>
        public void Activate()
        {            
            //Displaying dialog on the screen
            this.Visibility = System.Windows.Visibility.Visible;
            VisualStateManager.GoToState(this, "VisibleVisualState", true);
            this.DialogData.Focus();
        }

        /// <summary>
        /// Deactivating (or 'hide') the dialog from the screen.
        /// </summary>
        public void Deactivate()
        {
            VisualStateManager.GoToState(this, "CollapsedVisualState", true);
            //Erasing DialogData TextBox.Text field
            this.DialogData.Text = "";
        }
	}
}