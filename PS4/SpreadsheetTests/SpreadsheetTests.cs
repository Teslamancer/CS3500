using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
//Developed by Hunter Schmidt

namespace SpreadsheetTest
{
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Tests getting of cell with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testInvalidGetWithName()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("123A");
        }
        /// <summary>
        /// Tests getting of cell with null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testInvalidGetWithNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);
        }
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
        /// Tests getting all nonempty cells from spreadsheet
        /// </summary>
        [TestMethod]
        public void testGetNonEmpty()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "Text");
            sheet.SetContentsOfCell("A2", "1.5");
            sheet.SetContentsOfCell("A3", "=3 + 5");
            sheet.SetContentsOfCell("A4", "");
            List<string> cells = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(3, cells.Count);
            Assert.IsTrue(cells.Contains("A1"));
            Assert.IsTrue(cells.Contains("A2"));
            Assert.IsTrue(cells.Contains("A3"));
            Assert.IsFalse(cells.Contains("A4"));
        }

        /// <summary>
        /// Tests setting of cell with text and invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testInvalidSetWithText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("123A","Test");
        }
        /// <summary>
        /// Tests setting of cell with text and null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testNullSetWithText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "Test");
        }

        /// <summary>
        /// Tests setting of cell with double and invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testInvalidSetWithDouble()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("123A", "1.123");
        }
        /// <summary>
        /// Tests setting of cell with double and null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testNullSetWithDouble()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "1.123");
        }

        /// <summary>
        /// Tests setting of cell with Formula and invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testInvalidSetWithFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("123A", "=17 + 3");
        }
        /// <summary>
        /// Tests setting of cell with Formula and null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testNullSetWithFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "=17 + 3");
        }

        /// <summary>
        /// Tests setting of cell with null text and a valid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void testInvalidNullSetWithText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            string s = null;
            sheet.SetContentsOfCell("A1", s);
        }
        /// <summary>
        /// Tests setting of cell with null Formula and valid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void testInvalidNullSetWithFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            
            sheet.SetContentsOfCell("A1", null);
        }
        /// <summary>
        /// Tests Get and Set Methods for cells, and that setting an existing cell overwrites contents
        /// </summary>
        [TestMethod]
        public void testSetAndGet()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "1.5");
            Assert.AreEqual(1.5, sheet.GetCellContents("A1"));

            sheet.SetContentsOfCell("A1", "Test");
            Assert.AreEqual("Test", sheet.GetCellContents("A1"));

            sheet.SetContentsOfCell("A1", "=1 + 1");
            Assert.AreEqual(new Formula("1+1"), sheet.GetCellContents("A1"));
        }
        /// <summary>
        /// Tests deleting a cell by setting it to ""
        /// </summary>
        [TestMethod]
        public void testDeleteCell()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "Test");
            sheet.SetContentsOfCell("A1", "");
            List<string> cells = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(0, cells.Count);
            Assert.AreEqual("",sheet.GetCellContents("A1"));
        }
        /// <summary>
        /// Tests getting of Cell contents when Cell not set
        /// </summary>
        [TestMethod]
        public void testGetNonSetCell()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("",sheet.GetCellContents("A1"));
        }
        /// <summary>
        /// Tests setting cell with formula
        /// </summary>
        [TestMethod]
        public void testSetFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A3", "=3 + 5");
            List<string> cells = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(1, cells.Count);
            Assert.AreEqual(new Formula("3+5"), sheet.GetCellContents("A3"));
            sheet.SetContentsOfCell("A7", "=3/ 0");
            sheet.SetContentsOfCell("B1", "=A1+1");
            sheet.SetContentsOfCell("B2", "=A1+2");
            sheet.SetContentsOfCell("B3", "=A1+3");
            List<string> debug = new List<string>(sheet.SetContentsOfCell("A1", "=3/ 0"));
            foreach(string s in debug)
            {
                Console.WriteLine(s);
            }
            Assert.AreEqual(4, sheet.SetContentsOfCell("A1", "=3/ 0").Count);
        }
        /// <summary>
        /// Tests setting cell with double
        /// </summary>
        [TestMethod]
        public void testSetDouble()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "0.0");
            List<string> cells = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(1, cells.Count);
            Assert.AreEqual(0.0, sheet.GetCellContents("A1"));
            sheet.SetContentsOfCell("A7", "=3/ 0");
            sheet.SetContentsOfCell("B1", "=A1+1");
            sheet.SetContentsOfCell("B2", "=A1+2");
            sheet.SetContentsOfCell("B3", "=A1+3");
            List<string> debug = new List<string>(sheet.SetContentsOfCell("A1", "1.0"));
            foreach (string s in debug)
            {
                Console.WriteLine(s);
            }
            Assert.AreEqual(4, debug.Count);
        }

        /// <summary>
        /// Tests setting cell with text
        /// </summary>
        [TestMethod]
        public void testSetText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "Test");
            List<string> cells = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(1, cells.Count);
            Assert.AreEqual("Test", sheet.GetCellContents("A1"));
            sheet.SetContentsOfCell("A7", "=3/ 0");
            sheet.SetContentsOfCell("B1", "=A1+1");
            sheet.SetContentsOfCell("B2", "=A1+2");
            sheet.SetContentsOfCell("B3", "=A1+3");
            List<string> debug = new List<string>(sheet.SetContentsOfCell("A1", "Test2"));
            foreach (string s in debug)
            {
                Console.WriteLine(s);
            }
            Assert.AreEqual(4, debug.Count);
        }
        /// <summary>
        /// Tests setting of cell with Formula that creates circular dependency when previously the cell was empty
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void testCircularFormulaPrevEmpty()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=17 + B1");            
            try
            {
                sheet.SetContentsOfCell("B1", "=3 + A1");
            }
            catch (CircularException)
            {
                Assert.AreNotEqual(new Formula("3+A1"), sheet.GetCellContents("B1"));
                Assert.AreEqual("", sheet.GetCellContents("B1"));
                throw new CircularException();
            }           
        }
        /// <summary>
        /// Tests setting of cell with Formula that creates circular dependency when previously the cell contained text
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void testCircularFormulaPrevText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=17 + B1");
            sheet.SetContentsOfCell("B1", "Test");
            try
            {
                sheet.SetContentsOfCell("B1", "=3 + A1");
            }
            catch (CircularException)
            {
                Assert.AreNotEqual(new Formula("3+A1"), sheet.GetCellContents("B1"));
                Assert.AreEqual("Test", sheet.GetCellContents("B1"));
                throw new CircularException();
            }
        }
        /// <summary>
        /// Tests setting of cell with Formula that creates circular dependency when previously the cell contained a double
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void testCircularFormulaPrevDouble()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=17 + B1");
            sheet.SetContentsOfCell("B1","1.0");
            try
            {
                sheet.SetContentsOfCell("B1", "=3 + A1");
            }
            catch (CircularException)
            {
                Assert.AreNotEqual(new Formula("3+A1"), sheet.GetCellContents("B1"));
                Assert.AreEqual(1.0, sheet.GetCellContents("B1"));
                throw new CircularException();
            }
        }
        /// <summary>
        /// Tests setting of cell with Formula that creates circular dependency when previously the cell contained a Formula
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void testCircularFormulaPrevFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=17 + B1");
            sheet.SetContentsOfCell("B1", "=3+  C1");
            try
            {
                sheet.SetContentsOfCell("B1", "=3 + A1");
            }
            catch (CircularException)
            {
                Assert.AreNotEqual(new Formula("3+A1"), sheet.GetCellContents("B1"));
                Assert.AreEqual(new Formula("3+C1"), sheet.GetCellContents("B1"));
                List<string> debug = new List<string>(sheet.SetContentsOfCell("C1", "F"));
                foreach(string var in debug)
                {
                    Console.WriteLine(var);
                }
                Assert.AreEqual(3, debug.Count);
                throw new CircularException();
            }
        }
        /// <summary>
        /// Creating Cell with invalid Formula throws FormulaFormatException, Commented out as this just sets conents to text
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void testSetInvalidFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=17 + B1");
            sheet.SetContentsOfCell("B1", "=3+ + C1");

        }
        /// <summary>
        /// Creating Cell with Empty Formula throws FormulaFormatException, or does it just set to text?
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void testSetEmptyFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=17 + B1");
            sheet.SetContentsOfCell("B1", "=");

        }
    }
}
