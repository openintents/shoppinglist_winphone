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

namespace OIShoppingListWinPhone.CustomLayout
{
    public partial class EditNameDialog : UserControl
    {
        /// <summary>
        /// Current dialog displaying mode
        /// </summary>
        public int DialogMode;

        /// <summary>
        /// Enumeration for selecting the dialog's displaying mode
        /// (Actually, the dialog's label depends on this field)
        /// </summary>
        public enum EditNameDialogMode
        {
            AddingNewList,
            RenamingList,
            AddingNewStore,
            RenamingStore,
            RenamingSkyDriveFile
        }

        public EditNameDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Displaying (or 'activating') the control on the screen.
        /// </summary>
        /// <param name="mode">EditNameDialog mode.</param>
        public void Activate(EditNameDialogMode mode, string dialogData = "")
        {
            //Switching mode and displaying valid dialog's data
            switch (mode)
            {
                case EditNameDialogMode.AddingNewList:
                    {
                        this.DialogMode = (int)mode;
                        this.DialogLabel.Text = "Enter name of new shopping list";
                        this.DialogData.Text = "";
                    }
                    break;
                case EditNameDialogMode.RenamingList:
                    {
                        this.DialogMode = (int)mode;
                        this.DialogLabel.Text = "Enter new name of the shopping list";
                        this.DialogData.Text = dialogData;
                    }
                    break;
                case EditNameDialogMode.AddingNewStore:
                    {
                        this.DialogMode = (int)mode;
                        this.DialogLabel.Text = "Enter new store name";
                        this.DialogData.Text = "";
                    }
                    break;
                case EditNameDialogMode.RenamingStore:
                    {
                        this.DialogMode = (int)mode;
                        this.DialogLabel.Text = "Enter new name of the store";
                        this.DialogData.Text = dialogData;
                    }
                    break;
                case EditNameDialogMode.RenamingSkyDriveFile:
                    {
                        this.DialogMode = (int)mode;
                        this.DialogLabel.Text = "Enter new file name";
                        this.DialogData.Text = dialogData;
                    }
                    break;
                default:
                    break;
            }

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
