// Program 3
// CIS 200-01
// Due: 4/5/2018
// By: Z9467

// File: Prog3Form.cs
// Serialize and deserialize the library and editing the content

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace LibraryItems
{
    public partial class Prog3Form : Form
    {
        private BinaryFormatter formatter = new BinaryFormatter(); //Serializes the object
        private FileStream output; //The file being saved
        private BinaryFormatter reader = new BinaryFormatter(); // Reads the Object
        private FileStream input; //The file that is being opened
        private Library _lib; // The library
        // Precondition:  None
        // Postcondition: The form's GUI is prepared for display. A few test items and patrons
        //                are added to the library
        public Prog3Form()
        {
            InitializeComponent();

            _lib = new Library(); // Create the library

            // Insert test data - Magic numbers allowed here
            
        }

        // Precondition:  File, About menu item activated
        // Postcondition: Information about author displayed in dialog box
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string NL = Environment.NewLine; // NewLine shortcut

            MessageBox.Show($"Program 3{NL}By: Z9467{NL}CIS 200-01{NL}Spring 2018",
                "About Program 3");
        }

        // Precondition:  File, Exit menu item activated
        // Postcondition: The application is exited
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Precondition:  Report, Patron List menu item activated
        // Postcondition: The list of patrons is displayed in the reportTxt
        //                text box
        private void patronListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder(); // Holds text as report being built
                                                        // StringBuilder more efficient than String
            List<LibraryPatron> patrons;                // List of patrons
            string NL = Environment.NewLine;            // NewLine shortcut

            patrons = _lib.GetPatronsList();

            result.Append($"Patron List - {patrons.Count} patrons{NL}{NL}");

            foreach (LibraryPatron p in patrons)
                result.Append($"{p}{NL}{NL}");

            reportTxt.Text = result.ToString();

            // Put cursor at start of report
            reportTxt.SelectionStart = 0;
        }

        // Precondition:  Report, Item List menu item activated
        // Postcondition: The list of items is displayed in the reportTxt
        //                text box
        private void itemListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder(); // Holds text as report being built
                                                        // StringBuilder more efficient than String
            List<LibraryItem> items;                    // List of library items
            string NL = Environment.NewLine;            // NewLine shortcut

            items = _lib.GetItemsList();

            result.Append($"Item List - {items.Count} items{NL}{NL}");

            foreach (LibraryItem item in items)
                result.Append($"{item}{NL}{NL}");

            reportTxt.Text = result.ToString();

            // Put cursor at start of report
            reportTxt.SelectionStart = 0;
        }

        // Precondition:  Report, Checked Out Items menu item activated
        // Postcondition: The list of checked out items is displayed in the
        //                reportTxt text box
        private void checkedOutItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder(); // Holds text as report being built
                                                        // StringBuilder more efficient than String
            List<LibraryItem> items;                    // List of library items
            string NL = Environment.NewLine;            // NewLine shortcut

            items = _lib.GetItemsList();

            // LINQ: selects checked out items
            var checkedOutItems =
                from item in items
                where item.IsCheckedOut()
                select item;

            result.Append($"Checked Out Items - {checkedOutItems.Count()} items{NL}{NL}");

            foreach (LibraryItem item in checkedOutItems)
                result.Append($"{item}{NL}{NL}");

            reportTxt.Text = result.ToString();

            // Put cursor at start of report
            reportTxt.SelectionStart = 0;
        }

        // Precondition:  Insert, Patron menu item activated
        // Postcondition: The Patron dialog box is displayed. If data entered
        //                are OK, a LibraryPatron is created and added to the library
        private void patronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatronForm patronForm = new PatronForm(); // The patron dialog box form

            DialogResult result = patronForm.ShowDialog(); // Show form as dialog and store result

            if (result == DialogResult.OK) // Only add if OK
            {
                // Use form's properties to get patron info to send to library
                _lib.AddPatron(patronForm.PatronName, patronForm.PatronID);
            }

            patronForm.Dispose(); // Good .NET practice - will get garbage collected anyway
        }

        // Precondition:  Insert, Book menu item activated
        // Postcondition: The Book dialog box is displayed. If data entered
        //                are OK, a LibraryBook is created and added to the library
        private void bookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookForm bookForm = new BookForm(); // The book dialog box form

            DialogResult result = bookForm.ShowDialog(); // Show form as dialog and store result

            if (result == DialogResult.OK) // Only add if OK
            {
                try
                {
                    // Use form's properties to get book info to send to library
                    _lib.AddLibraryBook(bookForm.ItemTitle, bookForm.ItemPublisher, int.Parse(bookForm.ItemCopyrightYear),
                        int.Parse(bookForm.ItemLoanPeriod), bookForm.ItemCallNumber, bookForm.BookAuthor);
                }

                catch (FormatException) // This should never happen if form validation works!
                {
                    MessageBox.Show("Problem with Book Validation!", "Validation Error");
                }
            }

            bookForm.Dispose(); // Good .NET practice - will get garbage collected anyway
        }

        // Precondition:  Item, Check Out menu item activated
        // Postcondition: The Checkout dialog box is displayed. If data entered
        //                are OK, an item is checked out from the library by a patron
        private void checkOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<LibraryItem> items;     // List of library items
            List<LibraryPatron> patrons; // List of patrons

            items = _lib.GetItemsList();
            patrons = _lib.GetPatronsList();

            if (((items.Count - _lib.GetCheckedOutCount()) == 0) || (patrons.Count() == 0)) // Must have items and patrons
                MessageBox.Show("Must have items and patrons to check out!", "Check Out Error");
            else
            {
                CheckoutForm checkoutForm = new CheckoutForm(items, patrons); // The check out dialog box form

                DialogResult result = checkoutForm.ShowDialog(); // Show form as dialog and store result

                if (result == DialogResult.OK) // Only add if OK
                {
                    _lib.CheckOut(checkoutForm.ItemIndex, checkoutForm.PatronIndex);
                }

                checkoutForm.Dispose(); // Good .NET practice - will get garbage collected anyway
            }
        }

        // Precondition:  Item, Return menu item activated
        // Postcondition: The Return dialog box is displayed. If data entered
        //                are OK, an item is returned to the library
        private void returnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<LibraryItem> items;     // List of library items

            items = _lib.GetItemsList();

            if ((_lib.GetCheckedOutCount() == 0) ) // Must have items to return
                MessageBox.Show("Must have items to return!", "Return Error");
            else
            {
                ReturnForm returnForm = new ReturnForm(items); // The return dialog box form

                DialogResult result = returnForm.ShowDialog(); // Show form as dialog and store result

                if (result == DialogResult.OK) // Only add if OK
                {
                    _lib.ReturnToShelf(returnForm.ItemIndex);
                }

                returnForm.Dispose(); // Good .NET practice - will get garbage collected anyway
            }
        }

        //Preconditions: File, Save menu item activated
        //Postconditions: Save dialog box is displayed. If data entered are ok, a file is saved to the folder
        private void saveLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result; 
            string fileName; //The file name

            using (SaveFileDialog fileChooser = new SaveFileDialog()) //Making the new file and name
            {
                fileChooser.CheckFileExists = false;
                result = fileChooser.ShowDialog();
                fileName = fileChooser.FileName;
            }
            
            if(result == DialogResult.OK)
            {
                if(string.IsNullOrEmpty(fileName)) // Must have a file name
                {
                    MessageBox.Show("Invalid File Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        output = new FileStream(fileName, FileMode.Create, FileAccess.Write); //Creates the file
                        formatter.Serialize(output, _lib); //Serializes it
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Error opening file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        output?.Close();
                    }

                }
            }
        }

        //Preconditions: File, Open menu item activated
        //Postconditions: Open Dialog box is displayed. If data entered are ok, a file is opened to the application
        private void openLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string fileName; // The file name

            using (OpenFileDialog fileChooser = new OpenFileDialog()) //choosing the file
            {
                result = fileChooser.ShowDialog();
                fileName = fileChooser.FileName;
            }

            if (result == DialogResult.OK) //Only opens if dialog result is ok
            {
                if (string.IsNullOrEmpty(fileName)) //Must have a file name
                {
                    MessageBox.Show("Invalid File Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    input = new FileStream(fileName, FileMode.Open, FileAccess.Read); //reading the file
                    try
                    {
                        Library records = (Library)reader.Deserialize(input); //Deserializes the file
                        _lib = records;
                    }
                    catch (SerializationException)
                    {
                        MessageBox.Show("Error loading file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        input?.Close();
                    }
                }
            }
        }

        //Preconditions: Edit, Patron menu item activated
        //Postconditions: Opens a dialog box to select a patron to edit, and another form to edit the patron
        private void patronToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<LibraryPatron> patrons; //list of patrons
            patrons = _lib.GetPatronsList();

            if (patrons.Count == 0)
            {
                MessageBox.Show("Must have a patron");
            }
            else
            {
                PatronEdit edit = new PatronEdit(patrons); 
                DialogResult result = edit.ShowDialog(); // Show form as dialog and store result

                if (result == DialogResult.OK) //Only allows selection if ok
                {
                    LibraryPatron libraryPatron = patrons[edit.PatronIndex];
                    PatronForm editor = new PatronForm(); //seperate form but same format from patron form
                    DialogResult edited = editor.ShowDialog(); //Opens the patron form to edit
                    if (edited == DialogResult.OK) //Only allows edit if ok
                    {
                        libraryPatron.PatronName = editor.PatronName;
                        libraryPatron.PatronID = editor.PatronID;
                    }
                    editor.Dispose();
                }
            }

        }

        //Preconditions: Edit, Book menu item activated
        //Postconditions: Opens a dialog box to select a book to edit, and another form to edit the book
        private void bookToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<LibraryItem> items; //list of items
            items = _lib.GetItemsList();

            if(items.Count == 0) // Must have items in list
            {
                MessageBox.Show("Must have an item in the Library");
            }
            else
            {
                LibraryItemEdit edit = new LibraryItemEdit(items);
                DialogResult result = edit.ShowDialog();

                if(result == DialogResult.OK) //Only allows selection if ok
                {
                    LibraryItem item = items[edit.ItemIndex];
                    BookForm editor = new BookForm(); //seperate form but same format from bookform
                    DialogResult edited = editor.ShowDialog(); //Opens the BookForm to edit
                    if(edited == DialogResult.OK) //Only edits if ok
                    {
                        item.Title = editor.ItemTitle;
                        item.Publisher = editor.ItemPublisher;
                        item.CopyrightYear = int.Parse(editor.ItemCopyrightYear);
                        item.LoanPeriod = int.Parse(editor.ItemLoanPeriod);
                        item.CallNumber = editor.ItemCallNumber;
                    }
                    editor.Dispose();
                }
            }

        }
    }
}
