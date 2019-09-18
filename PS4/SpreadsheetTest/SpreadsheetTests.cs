using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;

namespace SpreadsheetTest
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Tests creation of empty spreadsheet
        /// </summary>
        [TestMethod]
        public void testEmpty()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            List<string> cells = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(0, cells.Count);
        }

        /// <summary>
        /// Tests setting of cell with text and invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testInvalidSetWithText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("123A","Test");
        }

        /// <summary>
        /// Tests setting of cell with double and invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testInvalidSetWithDouble()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("123A", 1.123);
        }

        /// <summary>
        /// Tests setting of cell with Formula and invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testInvalidSetWithFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("123A", new SpreadsheetUtilities.Formula("17 + 3"));
        }
    }
}
