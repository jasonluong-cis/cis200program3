using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibraryItems
{
    public partial class LibraryItemEdit : Form
    {
        private List<LibraryItem> _items; //Item List

        //Preconditions: None
        //Postconditions: Opens the form and loads the item list
        public LibraryItemEdit(List<LibraryItem> itemList)
        {
            InitializeComponent();
            _items = itemList;
        }

        //Preconditions: Must have items in the items list
        //Postconditions: Adds items to the combobox
        private void LibraryItemEdit_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < _items.Count; ++i)
            {
                if (!_items[i].IsCheckedOut()) // Not checked out, so OK to include
                {
                    itemCombo.Items.Add(_items[i].Title + ", " + _items[i].CallNumber);
                }
            }
        }

        internal int ItemIndex //returns the selected item index to be edited
        {
            // Precondition:  None
            // Postcondition: The index of form's selected patron combo box has been returned
            get
            {
                return itemCombo.SelectedIndex;
            }
        }

        //Preconditions: Clicked on Edit button
        //Postconditions: Returns the ok dialog result and opens another form to edit that item
        private void editButton_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        //Preconditions: Clicked on Cancel Button
        //Postconditions: Cancels the form
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void itemCombo_Validating(object sender, CancelEventArgs e)
        {
            if (itemCombo.SelectedIndex == -1) // Nothing selected
            {
                e.Cancel = true;
                errorProvider1.SetError(itemCombo, "Must select Item");
            }
        }

        private void itemCombo_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(itemCombo, "");
        }
    }
}
