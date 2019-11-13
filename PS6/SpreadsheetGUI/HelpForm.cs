using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            helpItemList.SelectedIndex = 0;
        }
        /// <summary>
        /// This shows help documentation depending on which item is selected from the 0-based index of helpItemList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpItemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (helpItemList.SelectedIndex)
            {
                case 0://If selection is "Save"
                    HelpContent.Text = "The Save functionality allows the user to save " +
                        "their spreadsheet to continue editing later. When choosing \"Save\" from " +
                        "the context menu under \"File\" if the file has not previously been saved, the user " +
                        "will be prompted to select a destination and specify a filename, otherwise, the spreadsheet " +
                        "will overwrite the last saved file.";
                    break;
                case 1://If selection is "Open"
                    HelpContent.Text = "The Open functionality allows the user to open a previously" +
                        "existing spreadsheet to continue to edit or view it. It prompts the user to select a file. If " +
                        "a file with unrecognized data or formatting is detected, the program informs the user and asks if they " +
                        "would like to select a different file.";
                    break;
                case 2://If selection is "Formula"
                    HelpContent.Text = "Formulas may be entered in cells by prepending an equal sign to the desired formula. Formulas" +
                        "may consist of addition, subtraction, multiplication, and division operations involving other cells or" +
                        "constant values. If a referenced cell is empty, its value is defaulted to 0. if it contains text" +
                        "the current cell with display \"#UNDEFINED\". If the provided formula results in an arithmetic error," +
                        "the cell will display \"#INVALID\"";
                    break;
                case 3: //If Selection is "Shortcuts"
                    HelpContent.Text = "Ctrl+N creates a new window\nCtrl+O opens a file\nCtrl+S saves the current file";
                    break;
                case 4://If Selection is "Autosave"
                    HelpContent.Text = "When enabled, this feature automatically saves the spreadsheet after every cell change" +
                        " provided the spreadsheet has already been saved initially or was opened from an existing sheet. WARNING: " +
                        "MAY CAUSE PERFORMANCE ISSUES ON LARGE SHEETS";
                    break;
                case 5://If Selection is "Editing a Cell's Contents"
                    HelpContent.Text = "Hitting a key other than an arrow key while a cell is selected puts the applications focus on" +
                        " the cell contents text box. Hitting enter while in this box will update the cells contents to whatever is in" + 
                        " the text box. Clicking on another cell also updates the cells contents." ;
                    break;
                case 6:
                    HelpContent.Text = "Selecting cells is done by clicking on them with the mouse. You can use the arrow keys to move one" + 
                        " cell in any direction." ;
                    break;
            }
        }
    }
}
