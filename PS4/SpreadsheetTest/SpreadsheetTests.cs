using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
//Developed by Hunter Schmidt

namespace SpreadsheetTest
{
    [TestClass]
    public class UnitTest1
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
            sheet.SetCellContents("A1", "Text");
            sheet.SetCellContents("A2", 1.5);
            sheet.SetCellContents("A3", new Formula("3 + 5", x => x, x => true));
            sheet.SetCellContents("A4", "");
            List<string> cells = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(3, cells.Count);
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
            sheet.SetCellContents("123A", new Formula("17 + 3"));
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
            sheet.SetCellContents("A1", s);
        }
        /// <summary>
        /// Tests setting of cell with null Formula and valid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void testInvalidNullSetWithFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula f = null;
            sheet.SetCellContents("A1", f);
        }
        /// <summary>
        /// Tests Get and Set Methods for cells, and that setting an existing cell overwrites contents
        /// </summary>
        [TestMethod]
        public void testSetAndGet()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 1.5);
            Assert.AreEqual(1.5, sheet.GetCellContents("A1"));

            sheet.SetCellContents("A1", "Test");
            Assert.AreEqual("Test", sheet.GetCellContents("A1"));

            sheet.SetCellContents("A1", new Formula("1 + 1"));
            Assert.AreEqual(new Formula("1+1"), sheet.GetCellContents("A1"));
        }
        /// <summary>
        /// Tests deleting a cell by setting it to ""
        /// </summary>
        [TestMethod]
        public void testDeleteCell()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", "Test");
            sheet.SetCellContents("A1", "");
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
            sheet.SetCellContents("A3", new Formula("3 + 5", x => x, x => true));
            List<string> cells = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(1, cells.Count);
            Assert.AreEqual(new Formula("3+5",x=>x,x=>true), sheet.GetCellContents("A3"));
            sheet.SetCellContents("A7", new Formula("3/ 0", x => x, x => true));
            sheet.SetCellContents("B1", new Formula("A1+1", x => x, x => true));
            sheet.SetCellContents("B2", new Formula("A1+2", x => x, x => true));
            sheet.SetCellContents("B3", new Formula("A1+3", x => x, x => true));
            List<string> debug = new List<string>(sheet.SetCellContents("A1", new Formula("3/ 0", x => x, x => true)));
            foreach(string s in debug)
            {
                Console.WriteLine(s);
            }
            Assert.AreEqual(4, sheet.SetCellContents("A1", new Formula("3/ 0", x => x, x => true)).Count);
        }
        /// <summary>
        /// Tests setting cell with double
        /// </summary>
        [TestMethod]
        public void testSetDouble()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 0.0);
            List<string> cells = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(1, cells.Count);
            Assert.AreEqual(0.0, sheet.GetCellContents("A1"));
            sheet.SetCellContents("A7", new Formula("3/ 0", x => x, x => true));
            sheet.SetCellContents("B1", new Formula("A1+1", x => x, x => true));
            sheet.SetCellContents("B2", new Formula("A1+2", x => x, x => true));
            sheet.SetCellContents("B3", new Formula("A1+3", x => x, x => true));
            List<string> debug = new List<string>(sheet.SetCellContents("A1", 1.0));
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
            sheet.SetCellContents("A1", "Test");
            List<string> cells = new List<string>(sheet.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(1, cells.Count);
            Assert.AreEqual("Test", sheet.GetCellContents("A1"));
            sheet.SetCellContents("A7", new Formula("3/ 0", x => x, x => true));
            sheet.SetCellContents("B1", new Formula("A1+1", x => x, x => true));
            sheet.SetCellContents("B2", new Formula("A1+2", x => x, x => true));
            sheet.SetCellContents("B3", new Formula("A1+3", x => x, x => true));
            List<string> debug = new List<string>(sheet.SetCellContents("A1", "Test2"));
            foreach (string s in debug)
            {
                Console.WriteLine(s);
            }
            Assert.AreEqual(4, debug.Count);
        }
        /// <summary>
        /// Tests setting of cell with Formula that creates circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void testCircularFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", new Formula("17 + B1"));
            sheet.SetCellContents("B1", "");
            sheet.SetCellContents("B1", new Formula("3 + A1"));
            Assert.AreNotEqual(new Formula("3+A1"), sheet.GetCellContents("B1"));
            Assert.AreEqual("",sheet.GetCellContents("B1"));
            Assert.AreEqual(new Formula("17+B1"), sheet.GetCellContents("B1"));
        }
        //TODO Write test that invalid formula throws exception
    }
}
