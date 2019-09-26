using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.Xml;
//Developed by Hunter Schmidt

namespace SpreadsheetTest
{
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Tests getting of cell with invalid base name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testBaseInvalidGetWithName()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("123A");
        }
        /// <summary>
        /// Tests getting of cell with invalid delegate name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testDelInvalidGetWithName()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => { if (s == s.ToUpper()) return true; else return false; },s=>s,"Invalid");
            sheet.GetCellContents("a1");
        }
        /// <summary>
        /// Tests getting of cell with invalid delegate name due to normalizer
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testDelNormInvalidGetWithName()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => { if (s == s.ToUpper()) return true; else return false; }, s => s.ToLower(), "Invalid");
            sheet.GetCellContents("A1");
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
        /// <summary>
        /// Creating Cell with space before equals sign sets to text, not Formula
        /// </summary>
        [TestMethod]
        public void testSetWhitespaceFirstFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", " =17 + B1");
            sheet.SetContentsOfCell("B1", "\t=2.1");
            Assert.AreEqual(" =17 + B1", sheet.GetCellContents("A1"));
            Assert.AreEqual("\t=2.1", sheet.GetCellContents("B1"));
        }
        /// <summary>
        /// Tests that all three constructors work and don't throw
        /// </summary>
        [TestMethod]
        public void testConstructors()
        {
            AbstractSpreadsheet emptyConstructor = new Spreadsheet();
            AbstractSpreadsheet threeArgConstructor = new Spreadsheet(s => true, s => s.ToUpper(), "4ArgTestSheet");
            AbstractSpreadsheet fourArgConstructor = new Spreadsheet("4ArgTestSheet",s => true, s => s.ToUpper(), "Test4Args");
        }
        /// <summary>
        /// Tests the Changed property of the spreadsheet
        /// </summary>
        [TestMethod]
        public void testChanged()
        {
            //COMMENTED PORTION AWAITING CLARIFICATION
            //AbstractSpreadsheet original = new Spreadsheet(x => true, x => x, "Default");
            //original.SetContentsOfCell("A1", "Test");
            //original.SetContentsOfCell("A2", "Test");
            //original.SetContentsOfCell("A3", "Test");
            //original.SetContentsOfCell("A4", "Test");
            //original.Save("OriginalSpreadsheet.XML");
            //Assert.IsFalse(original.Changed);
            //AbstractSpreadsheet copyOriginal = new Spreadsheet("OriginalSpreadsheet.XML", x => true, x=>x,"Default");
            //Assert.IsFalse(copyOriginal.Changed);
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.IsFalse(s.Changed);
            s.SetContentsOfCell("A1","");
            Assert.IsFalse(s.Changed);
            s.SetContentsOfCell("A1", "Test");
            Assert.IsTrue(s.Changed);
            s.Save("changedTestSheet.xml");
            Assert.IsFalse(s.Changed);
            s.SetContentsOfCell("A1", "Test");
            Assert.IsFalse(s.Changed);
            s.SetContentsOfCell("A1", "changed");
            Assert.IsTrue(s.Changed);
            AbstractSpreadsheet loadsheet = new Spreadsheet("changedTestSheet.xml", x=>true,x=>x,"test");
            Assert.IsFalse(loadsheet.Changed);
        }
        /// <summary>
        /// Tests that loading file with null filename throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testNullGetSaved()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.Save("nullLoad.XML");
            s.GetSavedVersion(null);
        }
        /// <summary>
        /// Tests that loading file with nonexistant filename throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testNonExistantGetSaved()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.Save("nullLoad.XML");
            s.GetSavedVersion("ThisFileIsNotReal.XML");
        }
        /// <summary>
        /// Tests that trying to get value of cell with null name throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testNullNameGetValue()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetCellValue(null);
        }
        /// <summary>
        /// Tests that trying to get value of cell with invalid name throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testInvalidNameGetValue()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetCellValue("1A");
        }
        /// <summary>
        /// Tests getting cell value
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testGetValue()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "String");
            s.SetContentsOfCell("A2", "1.0");
            s.SetContentsOfCell("A3", "=A2 +2");
            Assert.AreEqual("String", s.GetCellValue("A1"));
            Assert.AreEqual(1.0, s.GetCellValue("A2"));
            Assert.AreEqual(3.0, s.GetCellValue("A3"));
            s.SetContentsOfCell("A3", "=3/0");
            Assert.AreEqual(typeof(FormulaError), s.GetCellValue("A3").GetType());
        }
        /// <summary>
        /// Tests that getting and setting spreadsheet properties works
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testProperties()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("default", sheet.Version);
            Assert.AreEqual(s => true, sheet.IsValid);
            Assert.AreEqual(s => s, sheet.Normalize);

            sheet = new Spreadsheet( x => false, x => x.ToUpper(), "Test");
            Assert.AreEqual("Test", sheet.Version);
            Assert.AreEqual(x => false, sheet.IsValid);
            Assert.AreEqual(x => x.ToUpper(), sheet.Normalize);
            sheet.Save("PropertiesTest.XML");

            AbstractSpreadsheet testSheet = new Spreadsheet("PropertiesTest.XML", x => false, x => x.ToUpper(), "Test");
            Assert.AreEqual("Test", testSheet.Version);
            Assert.AreEqual(x => false, testSheet.IsValid);
            Assert.AreEqual(x => x.ToUpper(), testSheet.Normalize);

            AbstractSpreadsheet exceptor = new Spreadsheet("PropertiesTest.XML", x => false, x => x.ToUpper(), "Different");
        }
        /// <summary>
        /// Tests saving spreadsheet
        /// </summary>
        [TestMethod]
        public void testSave()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.0");
            s.SetContentsOfCell("A2", "Test");
            s.SetContentsOfCell("A3", "=A1 + 10");
            s.SetContentsOfCell("A2", "Replaced!");
            s.Save("FINDME.xml");

            using(XmlReader r = XmlReader.Create("FINDME.xml"))
            {
                r.Read();
                Assert.IsTrue(r.IsStartElement());
                Assert.AreEqual("spreadsheet",r.Name);
                Assert.AreEqual("default", r["version"]);
                readAndTestCellsFromXml(r, s);
            }
        }
        /// <summary>
        /// Helper method to test Cells saved properly into xml
        /// </summary>
        /// <param name="r">XmlReader</param>
        /// <param name="s">Spreadsheet</param>
        private void readAndTestCellsFromXml(XmlReader r, AbstractSpreadsheet s)
        {
            foreach (string cell in s.GetNamesOfAllNonemptyCells())
            {
                string toAddToContents = "";
                r.Read();
                Assert.IsTrue(r.IsStartElement());
                Assert.AreEqual("cell", r.Name);
                r.Read();
                Assert.IsTrue(r.IsStartElement());
                Assert.AreEqual("name", r.Name);
                r.Read();
                Assert.AreEqual(cell, r.Value);
                r.Read();
                r.Read();
                Assert.IsTrue(r.IsStartElement());
                Assert.AreEqual("contents", r.Name);
                r.Read();
                if (s.GetCellContents(cell).GetType() == typeof(Formula))
                    toAddToContents = "=";
                Assert.AreEqual(toAddToContents + s.GetCellContents(cell), r.Value);
                r.Read();
                r.Read();
                r.Read();
            }
        }
        /// <summary>
        /// Tests saving to an invalid (nonexistant) directory
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testSaveInvalidPath()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.0");
            s.SetContentsOfCell("A2", "Test");
            s.SetContentsOfCell("A3", "=A1 + 10");
            s.SetContentsOfCell("A2", "Replaced!");
            s.Save("/This/Path/Doesn't/Exist/42/FINDME.xml");
        }
        /// <summary>
        /// Tests saving to a nonexistant drive
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testSaveInvalidDrivePath()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.0");
            s.SetContentsOfCell("A2", "Test");
            s.SetContentsOfCell("A3", "=A1 + 10");
            s.SetContentsOfCell("A2", "Replaced!");
            s.Save(@"Z:\FINDME.xml");
        }
        /// <summary>
        /// Tests Saving with an Invalid Filename
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testSaveNameInvalid()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.0");
            s.SetContentsOfCell("A2", "Test");
            s.SetContentsOfCell("A3", "=A1 + 10");
            s.SetContentsOfCell("A2", "Replaced!");
            s.Save(@"asdf!!""asdf.xml");
        }
        /// <summary>
        /// Tests Saving with a filename that is too long
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testSaveNameTooLong()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.0");
            s.SetContentsOfCell("A2", "Test");
            s.SetContentsOfCell("A3", "=A1 + 10");
            s.SetContentsOfCell("A2", "Replaced!");
            s.Save(@"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.xml");
        }
        /// <summary>
        /// Tests getting saved version with invalid directory
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testGetSavedVersionInvalidDirectory()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetSavedVersion("this/path/does/not/exist");
        }
        /// <summary>
        /// Tests getting saved version with invalid file
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testGetSavedVersionInvalidFile()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetSavedVersion("this/path/does/not/exist");
        }
    }
}
