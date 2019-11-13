/// Authors: Michael Blum and Hunter Schmidt
/// Date: 10/4/19
///

using SpreadsheetGUI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SS
{

    /// <summary>
    /// Represents a spreadsheet window
    /// </summary>
    public partial class Window : Form
    {
        /// <summary>
        /// The spreadsheet backend.
        /// </summary>
        private Spreadsheet ss;

        /// <summary>
        /// The currently selected cell
        /// </summary>
        private string selectedCell;

        /// <summary>
        /// The filename of the currently open file
        /// </summary>
        private string filename;

        /// <summary>
        /// The path to the currently open file
        /// </summary>
        private string filepath;

        /// <summary>
        /// isValid delegate used to determine if a cell name is valid or not
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isValidCell(string s)
        {

            // Only allow cells in the spreadsheet range
            CellToCoords(s, out int col, out int row);
            if (col < 0 || col > 26 || row < 0 || row > 99) return false;

            return Regex.IsMatch(s, @"^[A-Z][0-9]+$");
        }

        /// <summary>
        /// Creates a new Window
        /// </summary>
        public Window()
        {
            InitializeComponent();

            //Initializes spreadsheet with an uppercase normalizer
            ss = new Spreadsheet(isValidCell, x => x.ToUpper(), "ps6");
            filename = "";
            filepath = "";
            statusLabel.Text = "Ready";

            // Handle selection events
            spreadsheetPanel1.SelectionChanged += updateSelectedCell;
            spreadsheetPanel1.SetSelection(0, 0);

            selectedCell = "A1";
            updateInspector();
        }

        /// <summary>
        /// Updates the inspectors values to the value and contents of selectedCell
        /// </summary>
        private void updateInspector()
        {
            cellTextBox.Text = selectedCell;

            object contents = ss.GetCellContents(selectedCell);

            if (!(contents is string) && !(contents is double))
                cellContentsTextBox.Text = "=" + ss.GetCellContents(selectedCell).ToString();
            else
                cellContentsTextBox.Text = ss.GetCellContents(selectedCell).ToString();            
        }

        /// <summary>
        /// Updates the value of selected Cell when the user clicks somewhere on the spreadsheetpanel
        /// </summary>
        /// <param name="sp"></param>
        private void updateSelectedCell(SpreadsheetPanel sp)
        {
            updateCellContents(); // Update the previously selected cell
            //backgroundWorker1.RunWorkerAsync(updateCellContents);
            int row, col;
            sp.GetSelection(out col, out row);

            selectedCell = CoordsToCell(col, row);

            updateInspector();
            spreadsheetPanel1.Focus();
        }

        /// <summary>
        /// Updates the contents of selectedCell and all cells that depend, directly or indirectly, on it.
        /// </summary>
        private void updateCellContents()
        {

            List<string> recalc;

            try
            {
                recalc = new List<string>(ss.SetContentsOfCell(selectedCell, cellContentsTextBox.Text));

                statusLabel.Text = "Updating cells...";
                foreach (string cell in recalc)
                    if(ss.GetCellValue(cell).ToString().Contains("FormulaError"))
                        updatePanelCell(cell, "#UNDEFINED");
                    else
                        updatePanelCell(cell, ss.GetCellValue(cell).ToString());

                statusLabel.Text = "Ready";
            }
            catch (CircularException)
            {
                MessageBox.Show("ERROR: Circular Dependency detected.");
            }
            catch (Exception)//Sets Cell to Display "#INVALID" if formula is invalid
            {
                MessageBox.Show("Invalid Formula Entered!","Error",MessageBoxButtons.OK);
                updatePanelCell(selectedCell, ss.GetCellValue(selectedCell).ToString());
            }

            if(AutoSaveToggle.Checked)//Autosaves if autosave turned on and file has already been saved
            {
                SaveToolStripMenuItem_Click(null, null);
                Text = filename;
            }
        }

        /// <summary>
        /// Updates the value the user sees in the cell
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        private void updatePanelCell(string cell, string value)
        {
            // Get the coordinates of the cell
            CellToCoords(cell, out int col, out int row);

            spreadsheetPanel1.SetValue(col, row, value);

            if (ss.GetCellValue(cell).ToString().Contains("FormulaError"))
                cellValue.Text = "#UNDEFINED";
            else
                cellValue.Text = ss.GetCellValue(cell).ToString();
            
        }

        /// <summary>
        /// Called when the user clicks New or presses Ctrl+N
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            SSApplicationContext.getAppContext().RunForm(new Window());
        }

        /// <summary>
        /// Called when the user clicks Save or presses Ctrl+S
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Saving...";
            if (filename == "" || filepath == "")
                SaveAsToolStripMenuItem_Click(sender, e);
            else
            {
                ss.Save(filepath);
                Text = filepath;
            }

            statusLabel.Text = "Ready";
        }

        /// <summary>
        /// Called when the user clicks Save As
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open a save dialog box            
            SaveFileDialog saveDialog = new SaveFileDialog();
            //set dialog to filter to spradsheet or all files
            saveDialog.Filter = "spreadsheet files (*.sprd)|*.sprd|All files (*.*)|*.*";
            //call dialog "Save as"
            saveDialog.Title = "Save as";
            saveDialog.FileName = "Spreadsheet1.sprd";
            //if everything goes well, set filename and filepath and save
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveDialog.FileName != "")
                {
                    filepath = saveDialog.FileName;
                    filename = System.IO.Path.GetFileNameWithoutExtension(filepath);
                    ss.Save(filepath);
                    Text = filename;
                }
            }

        }

        /// <summary>
        /// Called when the user clicks Open or presses Ctrl+O
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (ss.Changed)
            {
                switch (MessageBox.Show("Save changes?", "Save changes", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        SaveAsToolStripMenuItem_Click(sender, e);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }                        
            //Shows a new Open File Dialog
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Open";
            openDialog.Filter = "spreadsheet files (*.sprd)|*.sprd|All files (*.*)|*.*";
            List<string> cells = new List<string>(ss.GetNamesOfAllNonemptyCells());

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                while (true)//loops until the open window is closed, a valid file is selected, or the error message box is closed or "cancel" is selected
                {
                    if (openDialog.FileName != "")
                    {
                        filepath = openDialog.FileName;

                        if (System.IO.Path.GetExtension(filepath) == "") filepath += ".sprd";

                        try
                        {
                            ss = new Spreadsheet(filepath, isValidCell, s => s.ToUpper(), "ps6");
                            // Clear the old spreadsheet
                            foreach (string cell in cells)
                                updatePanelCell(cell, "");

                            // Update cells with new values
                            cells = new List<string>(ss.GetNamesOfAllNonemptyCells());
                            foreach (string cell in cells)
                                updatePanelCell(cell, ss.GetCellValue(cell).ToString());

                            filename = System.IO.Path.GetFileNameWithoutExtension(filepath);
                            Text = filename;

                            updateInspector();
                            break;
                        }
                        catch (SpreadsheetReadWriteException)
                        {//Shows messagebox if "Open" Failed
                            if (MessageBox.Show("File Not Valid!", "Invalid File!", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                                break;
                        }

                        // Show the open dialog again
                        openDialog.ShowDialog();
                    } // end if (filenaem != "")

                } // end While loop

            } // end if (openDialog.ShowDialog() == ok)
            
        }

        /// <summary>
        /// Called when the user presses enter in the cellContentsTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellContentsTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//updates cells
                updateCellContents();
        }

        /// <summary>
        /// Called when the user closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ss.Changed)
            {//Displays warning if user closes window with unsaved changes
                switch (MessageBox.Show("Save changes?", "Save changes", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        SaveAsToolStripMenuItem_Click(sender, e);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Called as the user types in the text box, provides live updates in the cell that's being edited
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellContentsTextBox_TextChanged(object sender, EventArgs e)
        {
            updatePanelCell(selectedCell, cellContentsTextBox.Text);            
        }
        
        /// <summary>
        /// Opens Help Window in new Window when "Help" is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var helpPopup = new HelpForm();//Opens help form in new window
            helpPopup.Show();
        }

        /// <summary>
        /// Handles keyboard shortcuts
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.N))
            {
                NewFileToolStripMenuItem_Click(null, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.O))
            {
                OpenToolStripMenuItem_Click(null, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.S))
            {
                SaveToolStripMenuItem_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Handles Arrowkeys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetPanel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            spreadsheetPanel1.GetSelection(out int currentCol, out int currentRow);

            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (currentRow < 99)
                    {
                        spreadsheetPanel1.SetSelection(currentCol, currentRow + 1);
                        selectedCell = CoordsToCell(currentCol, currentRow + 1);
                        updateInspector();
                    }
                    break;
                case Keys.Right:
                    if (currentCol < 26)
                    {
                        spreadsheetPanel1.SetSelection(currentCol + 1, currentRow);
                        selectedCell = CoordsToCell(currentCol + 1, currentRow);
                        updateInspector();
                    }
                    break;
                case Keys.Up:
                    if (currentCol > 1)
                    {
                        spreadsheetPanel1.SetSelection(currentCol, currentRow - 1);
                        selectedCell = CoordsToCell(currentCol, currentRow - 1);
                        updateInspector();
                    }
                    break;
                case Keys.Left:
                    if (currentRow > 1)
                    {
                        spreadsheetPanel1.SetSelection(currentCol - 1, currentRow);
                        selectedCell = CoordsToCell(currentCol - 1, currentRow);
                        updateInspector();
                    }
                    break;
                default:
                    cellContentsTextBox.SelectAll();
                    cellContentsTextBox.Focus();
                    return;
            }
        }

        /// <summary>
        /// Returns a cell reference for the given coordinates
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private string CoordsToCell(int col, int row)
        {
            return Convert.ToChar(col + 65).ToString() + (row + 1).ToString();
        }

        /// <summary>
        /// Converts a given cell object to spreadsheet coordinates
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void CellToCoords(string cell, out int col, out int row)
        {
            col = Convert.ToInt32(cell[0]) - 65;
            row = Convert.ToInt32(cell.Substring(1)) - 1;
        }

        /// <summary>
        /// Called when the user clicks the save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveStripButton_Click(object sender, EventArgs e)
        {
            SaveToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Called when the user clicks Close in the file menu. Closes the current form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ss.Changed)
            {//Displays warning if user closes window with unsaved changes
                switch (MessageBox.Show("Save changes?", "Save changes", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        SaveAsToolStripMenuItem_Click(sender, e);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }

            Close();
        }

        /// <summary>
        /// Toggles autosave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutosaveToggleButton_Click(object sender, EventArgs e)
        {
            AutoSaveToggle.Checked = !AutoSaveToggle.Checked;
        }
    }
}
