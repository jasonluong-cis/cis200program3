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
    public partial class PatronEdit : Form
    {
        private List<LibraryPatron> _patrons; //Patron List

        //Preconditions: None
        //Postconditions: Loads up the form and loads the patron list
        public PatronEdit(List<LibraryPatron> patronList)
        {
            InitializeComponent();
            _patrons = patronList;
        }

        //Preconditions:None
        //Postconditions: Adds the patron list to the combo box
        private void PatronEdit_Load(object sender, EventArgs e)
        {
            foreach (LibraryPatron patron in _patrons)
                patronCombo.Items.Add(patron.PatronName + ", " + patron.PatronID);
        }

        internal int PatronIndex //returns the selected patron index
        {
            // Precondition:  None
            // Postcondition: The index of form's selected patron combo box has been returned
            get
            {
                return patronCombo.SelectedIndex;
            }
        }
        //Preconditions: Clicked on edit button
        //Postconditions: Returns the dialog ok result that opens another form to edit that item
        private void editButton_Click(object sender, EventArgs e)
        {
            if(ValidateChildren())
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        //Preconditions: Clicked on Cancel button
        //Postconditions: Closes the current form
        private void cancelButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.DialogResult = DialogResult.Cancel;
        }

        private void patronCombo_Validating(object sender, CancelEventArgs e)
        {
            if (patronCombo.SelectedIndex == -1) // Nothing selected
            {
                e.Cancel = true;
                errorProvider1.SetError(patronCombo, "Must select Item");
            }
        }

        private void patronCombo_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(patronCombo, "");
        }
    }
}
