using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
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
            threeArgConstructor.Save("4ArgTestSheet.xml");
            AbstractSpreadsheet fourArgConstructor = new Spreadsheet("4ArgTestSheet.xml",s => true, s => s.ToUpper(), "4ArgTestSheet");
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
            AbstractSpreadsheet loadsheet = new Spreadsheet("changedTestSheet.xml", x=>true,x=>x,"default");
            Assert.IsFalse(loadsheet.Changed);
            loadsheet.SetContentsOfCell("A1","=1+1");
            Assert.IsTrue(loadsheet.Changed);
            loadsheet.Save("loadedsheet.xml");
            Assert.IsFalse(loadsheet.Changed);
            loadsheet.SetContentsOfCell("A1","=1+1");
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
        /// Delegate to use for testing properties
        /// </summary>
        /// <param name="s">Cell name</param>
        /// <returns>false</returns>
        public bool isValidPropertiesTest(string s)
        {
            return false;
        }
        /// <summary>
        /// Delegate to use for testing properties
        /// </summary>
        /// <param name="s">Cell Name</param>
        /// <returns>Normalized Cell Name</returns>
        public string normalizePropertiesTest(string s)
        {
            return s.ToUpper();
        }
        /// <summary>
        /// Tests that getting and setting spreadsheet properties works
        /// </summary>
        [TestMethod]
        public void testProperties()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("default", sheet.Version);            

            sheet = new Spreadsheet( isValidPropertiesTest, normalizePropertiesTest, "Test");
            Assert.AreEqual("Test", sheet.Version);
            Assert.AreEqual(isValidPropertiesTest, sheet.IsValid);
            Assert.AreEqual(normalizePropertiesTest, sheet.Normalize);
            sheet.Save("PropertiesTest.XML");

            AbstractSpreadsheet testSheet = new Spreadsheet("PropertiesTest.XML",isValidPropertiesTest, normalizePropertiesTest, "Test");
            Assert.AreEqual("Test", testSheet.Version);
            Assert.AreEqual(isValidPropertiesTest, testSheet.IsValid);
            Assert.AreEqual(normalizePropertiesTest, testSheet.Normalize);
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
            s.Save("as?_-@#$%^&*()df!\'!\"asdf.xml");
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
            s.GetSavedVersion("this/path/does/not/exist.xml");
        }
        /// <summary>
        /// Tests getting saved version with invalid file
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testGetSavedVersionInvalidFile()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            using(XmlWriter w = XmlWriter.Create("unreal.xml",settings))
            {
                w.WriteStartDocument();
                w.WriteStartElement("Garbage");
                w.WriteEndElement();
                w.WriteEndDocument();
            }
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetSavedVersion("unreal.xml");
        }
        /// <summary>
        /// Tests that trying to get value of null cell throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testNullGetValue()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1","Test");
            s.GetCellValue(null);
        }
        /// <summary>
        /// Tests that trying to get value of cell with basic name invalid throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testInvalidBaseGetValue()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Test");
            s.GetCellValue("12A");
        }
        /// <summary>
        /// Tests that trying to get value of cell with name invalid due to isValid delegate throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testInvalidDelIsValidGetValue()
        {
            AbstractSpreadsheet s = new Spreadsheet(x=>false,x=>x,"test");
            s.SetContentsOfCell("A1", "Test");
            s.GetCellValue("A1");
        }
        /// <summary>
        /// Tests that trying to get value of cell with name invalid due to normalize delegate throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testInvalidDelNormalizeGetValue()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => { if (x == x.ToUpper()) return true; else return false; }, x => x.ToLower(), "test");
            s.SetContentsOfCell("A1", "Test");
            s.GetCellValue("A1");
        }
        /// <summary>
        /// Tests that creating spreadsheet from nonmatching version throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testNonmatchingVersionConstructor()
        {
            AbstractSpreadsheet s = new Spreadsheet(x=>true,x=>x,"Test");
            s.Save("Test.xml");
            AbstractSpreadsheet readInSheet = new Spreadsheet("Test.xml",x=>true,x=>x,"NotTest");
        }

        /// <summary>
        /// Tests that creating spreadsheet from file with null filepath throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testNullFilepathConstructor()
        {
            AbstractSpreadsheet readInSheet = new Spreadsheet(null, x => true, x => x, "NotTest");
        }
        /// <summary>
        /// Tests that creating spreadsheet from file with null isValid delegate throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void testNullIsValidDelegateConstructor()
        {
            AbstractSpreadsheet readInSheet = new Spreadsheet(null, x => x, "NotTest");
        }
        /// <summary>
        /// Tests that creating spreadsheet from file with null normalize delegate throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void testNullNormalizeDelegateConstructor()
        {
            AbstractSpreadsheet readInSheet = new Spreadsheet(x=>true, null, "NotTest");
        }
        /// <summary>
        /// Tests that creating spreadsheet from file with null version throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void testNullVersionConstructor()
        {
            AbstractSpreadsheet readInSheet = new Spreadsheet(x => true, x=>x, null);
        }

        /// <summary>
        /// Tests that creating spreadsheet from file with invalid filepath throws exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testInvalidFilepathConstructor()
        {
            AbstractSpreadsheet readInSheet = new Spreadsheet("Nonexistant.xml", x => true, x => x, "NotTest");
        }
        /// <summary>
        /// Tests that cell values recalculate after each set operation
        /// </summary>
        [TestMethod]
        public void testRecalculatingValues()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.0");
            s.SetContentsOfCell("A2","1.0");
            s.SetContentsOfCell("A3","=A1 + A2");
            Assert.AreEqual((2.0).ToString(), s.GetCellValue("A3").ToString());
            s.SetContentsOfCell("A1","0");
            Assert.AreEqual((1.0).ToString(), s.GetCellValue("A3").ToString());
        }

        /// <summary>
        /// Tests that cell values recalculate after each set operation
        /// </summary>
        [TestMethod]
        public void testFormulaErrorFromCells()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.0");
            s.SetContentsOfCell("A2", "1.0");
            s.SetContentsOfCell("A3", "=A1 + A2");
            Assert.AreEqual((2.0).ToString(), s.GetCellValue("A3").ToString());
            s.SetContentsOfCell("A1", "TEST");
            Assert.AreEqual(typeof(FormulaError), s.GetCellValue("A3").GetType());
            s.SetContentsOfCell("A1", "3.0");
            Assert.AreEqual((4.0).ToString(), s.GetCellValue("A3").ToString());
            s.SetContentsOfCell("A1", "=A2");
            Assert.AreEqual((2.0).ToString(), s.GetCellValue("A3").ToString());
        }
        /// <summary>
        /// Tests getting of a saved version from file
        /// </summary>
        [TestMethod]
        public void testGetSavedVersion()
        {
            AbstractSpreadsheet s = new Spreadsheet(x=>true,x=>x,"TestVersion");
            s.Save("TestGetVersion.xml");
            Assert.AreEqual("TestVersion", s.GetSavedVersion("TestGetVersion.xml"));
        }
        /// <summary>
        /// Stress test for Spreadsheet
        /// </summary>
        [TestMethod]
        public void stressSaveAndLoadTest()
        {
            Dictionary<string, double> checker = new Dictionary<string, double>();
            AbstractSpreadsheet s = new Spreadsheet(x=>true,x=>x,"Stress");
            for(int i = 0; i < 1000; i++)
            {
                s.SetContentsOfCell("A"+i,i.ToString());
                checker.Add("A"+i,i);
                s.Save("StressTest.xml");
            }
            foreach(string cell in s.GetNamesOfAllNonemptyCells())
            {
                Assert.AreEqual(checker[cell], s.GetCellContents(cell));
            }
            AbstractSpreadsheet retrieved = new Spreadsheet("StressTest.xml", x => true, x => x, "Stress");
            foreach (string cell in retrieved.GetNamesOfAllNonemptyCells())
            {
                Assert.AreEqual(checker[cell], retrieved.GetCellContents(cell));
            }
        }
        /// <summary>
        /// Stress test for Spreadsheet
        /// </summary>
        [TestMethod]
        public void stressSaveAndLoadTest2()
        {
            stressSaveAndLoadTest();
        }
        /// <summary>
        /// Stress test for Spreadsheet
        /// </summary>
        [TestMethod]
        public void stressSaveAndLoadTest3()
        {
            stressSaveAndLoadTest();
        }
        /// <summary>
        /// Stress test of value recalculation after loading from file
        /// </summary>
        [TestMethod]
        public void stressLoadRecalculateValues()
        {
            Dictionary<string, double> checker = new Dictionary<string, double>();
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x, "Stress");
            for (int i = 0; i < 1000; i++)
            {
                s.SetContentsOfCell("A" + i, i.ToString());
                checker.Add("A" + i, i);
                s.Save("StressTest.xml");
            }
            foreach (string cell in s.GetNamesOfAllNonemptyCells())
            {
                Assert.AreEqual(checker[cell], s.GetCellContents(cell));
            }
            AbstractSpreadsheet retrieved = new Spreadsheet("StressTest.xml", x => true, x => x, "Stress");
            foreach (string cell in retrieved.GetNamesOfAllNonemptyCells())
            {
                Assert.AreEqual(checker[cell], retrieved.GetCellValue(cell));
            }
        }
        /// <summary>
        /// Stress test of value recalculation
        /// </summary>
        [TestMethod]
        public void stressRecalculateValues()
        {
            Dictionary<string, double> checker = new Dictionary<string, double>();
            AbstractSpreadsheet s = new Spreadsheet(x => true, x => x, "Stress");
            s.SetContentsOfCell("A0","1");
            for (int i = 1; i < 1000; i++)
            {
                s.SetContentsOfCell("A" + i, "=A"+(i-1));               
                
            }
            foreach (string cell in s.GetNamesOfAllNonemptyCells())
            {
                Assert.AreEqual(1d, s.GetCellValue(cell));
            }
            s.SetContentsOfCell("A0","2");
            foreach (string cell in s.GetNamesOfAllNonemptyCells())
            {
                Assert.AreEqual(2d, s.GetCellValue(cell));
            }
        }
        // EMPTY SPREADSHEETS
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestEmptyGetNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestEmptyGetContents()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("1AA");
        }

        [TestMethod(), Timeout(5000)]
        public void TestGetEmptyContents()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("A2"));
        }

        // SETTING CELL TO A DOUBLE
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetNullDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "1.5");
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetInvalidNameDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1A1A", "1.5");
        }

        [TestMethod(), Timeout(5000)]
        public void TestSimpleSetDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "1.5");
            Assert.AreEqual(1.5, (double)s.GetCellContents("Z7"), 1e-9);
        }

        // SETTING CELL TO A STRING
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetNullStringVal()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A8", (string)null);
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetNullStringName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "hello");
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetSimpleString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1AZ", "hello");
        }

        [TestMethod(), Timeout(5000)]
        public void TestSetGetSimpleString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "hello");
            Assert.AreEqual("hello", s.GetCellContents("Z7"));
        }

        //// SETTING CELL TO A FORMULA - Commented Out as this is not possible with the new API
        //[TestMethod(), Timeout(5000)]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void TestSetNullFormVal()
        //{
        //    Spreadsheet s = new Spreadsheet();
        //    s.SetContentsOfCell("A8", "=" + (string)null);
        //}

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetNullFormName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "=2");
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetSimpleForm()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1AZ", "=2");
        }

        [TestMethod(), Timeout(5000)]
        public void TestSetGetForm()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "=3");
            Formula f = (Formula)s.GetCellContents("Z7");
            Assert.AreEqual(new Formula("3"), f);
            Assert.AreNotEqual(new Formula("2"), f);
        }

        // CIRCULAR FORMULA DETECTION
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(CircularException))]
        public void TestSimpleCircular()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2");
            s.SetContentsOfCell("A2", "=A1");
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(CircularException))]
        public void TestComplexCircular()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A3", "=A4+A5");
            s.SetContentsOfCell("A5", "=A6+A7");
            s.SetContentsOfCell("A7", "=A1+A1");
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(CircularException))]
        public void TestUndoCircular()
        {
            Spreadsheet s = new Spreadsheet();
            try
            {
                s.SetContentsOfCell("A1", "=A2+A3");
                s.SetContentsOfCell("A2", "15");
                s.SetContentsOfCell("A3", "30");
                s.SetContentsOfCell("A2", "=A3*A1");
            }
            catch (CircularException e)
            {
                Assert.AreEqual(15, (double)s.GetCellContents("A2"), 1e-9);
                throw e;
            }
        }

        // NONEMPTY CELLS
        [TestMethod(), Timeout(5000)]
        public void TestEmptyNames()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        [TestMethod(), Timeout(5000)]
        public void TestExplicitEmptySet()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "");
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        [TestMethod(), Timeout(5000)]
        public void TestSimpleNamesString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "hello");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }

        [TestMethod(), Timeout(5000)]
        public void TestSimpleNamesDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "52.25");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }

        [TestMethod(), Timeout(5000)]
        public void TestSimpleNamesFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "=3.5");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }

        [TestMethod(), Timeout(5000)]
        public void TestMixedNames()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "17.2");
            s.SetContentsOfCell("C1", "hello");
            s.SetContentsOfCell("B1", "=3.5");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "A1", "B1", "C1" }));
        }

        // RETURN VALUE OF SET CELL CONTENTS
        [TestMethod(), Timeout(5000)]
        public void TestSetSingletonDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "hello");
            s.SetContentsOfCell("C1", "=5");
            Assert.IsTrue(s.SetContentsOfCell("A1", "17.2").SequenceEqual(new List<string>() { "A1" }));
        }

        [TestMethod(), Timeout(5000)]
        public void TestSetSingletonString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "17.2");
            s.SetContentsOfCell("C1", "=5");
            Assert.IsTrue(s.SetContentsOfCell("B1", "hello").SequenceEqual(new List<string>() { "B1" }));
        }

        [TestMethod(), Timeout(5000)]
        public void TestSetSingletonFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "17.2");
            s.SetContentsOfCell("B1", "hello");
            Assert.IsTrue(s.SetContentsOfCell("C1", "=5").SequenceEqual(new List<string>() { "C1" }));
        }

        [TestMethod(), Timeout(5000)]
        public void TestSetChain()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A2", "6");
            s.SetContentsOfCell("A3", "=A2+A4");
            s.SetContentsOfCell("A4", "=A2+A5");
            Assert.IsTrue(s.SetContentsOfCell("A5", "82.5").SequenceEqual(new List<string>() { "A5", "A4", "A3", "A1" }));
        }

        // CHANGING CELLS
        [TestMethod(), Timeout(5000)]
        public void TestChangeFtoD()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A1", "2.5");
            Assert.AreEqual(2.5, (double)s.GetCellContents("A1"), 1e-9);
        }

        [TestMethod(), Timeout(5000)]
        public void TestChangeFtoS()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A1", "Hello");
            Assert.AreEqual("Hello", (string)s.GetCellContents("A1"));
        }

        [TestMethod(), Timeout(5000)]
        public void TestChangeStoF()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Hello");
            s.SetContentsOfCell("A1", "=23");
            Assert.AreEqual(new Formula("23"), (Formula)s.GetCellContents("A1"));
            Assert.AreNotEqual(new Formula("24"), (Formula)s.GetCellContents("A1"));
        }

        // STRESS TESTS
        [TestMethod(), Timeout(5000)]
        public void TestStress1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1+B2");
            s.SetContentsOfCell("B1", "=C1-C2");
            s.SetContentsOfCell("B2", "=C3*C4");
            s.SetContentsOfCell("C1", "=D1*D2");
            s.SetContentsOfCell("C2", "=D3*D4");
            s.SetContentsOfCell("C3", "=D5*D6");
            s.SetContentsOfCell("C4", "=D7*D8");
            s.SetContentsOfCell("D1", "=E1");
            s.SetContentsOfCell("D2", "=E1");
            s.SetContentsOfCell("D3", "=E1");
            s.SetContentsOfCell("D4", "=E1");
            s.SetContentsOfCell("D5", "=E1");
            s.SetContentsOfCell("D6", "=E1");
            s.SetContentsOfCell("D7", "=E1");
            s.SetContentsOfCell("D8", "=E1");
            IList<String> cells = s.SetContentsOfCell("E1", "0");
            Assert.IsTrue(new HashSet<string>() { "A1", "B1", "B2", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "E1" }.SetEquals(cells));
        }

        // Repeated for extra weight
        [TestMethod(), Timeout(5000)]
        public void TestStress1a()
        {
            TestStress1();
        }
        [TestMethod(), Timeout(5000)]
        public void TestStress1b()
        {
            TestStress1();
        }
        [TestMethod(), Timeout(5000)]
        public void TestStress1c()
        {
            TestStress1();
        }

        [TestMethod(), Timeout(5000)]
        public void TestStress2()
        {
            Spreadsheet s = new Spreadsheet();
            ISet<String> cells = new HashSet<string>();
            for (int i = 1; i < 200; i++)
            {
                cells.Add("A" + i);
                Assert.IsTrue(cells.SetEquals(s.SetContentsOfCell("A" + i, "=A" + (i + 1))));
            }
        }
        [TestMethod(), Timeout(5000)]
        public void TestStress2a()
        {
            TestStress2();
        }
        [TestMethod(), Timeout(5000)]
        public void TestStress2b()
        {
            TestStress2();
        }
        [TestMethod(), Timeout(5000)]
        public void TestStress2c()
        {
            TestStress2();
        }

        [TestMethod(), Timeout(5000)]
        public void TestStress3()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 1; i < 200; i++)
            {
                s.SetContentsOfCell("A" + i, "=A" + (i + 1));
            }
            try
            {
                s.SetContentsOfCell("A150", "=A50");
                Assert.Fail();
            }
            catch (CircularException)
            {
            }
        }

        [TestMethod(), Timeout(5000)]
        public void TestStress3a()
        {
            TestStress3();
        }
        [TestMethod(), Timeout(5000)]
        public void TestStress3b()
        {
            TestStress3();
        }
        [TestMethod(), Timeout(5000)]
        public void TestStress3c()
        {
            TestStress3();
        }

        [TestMethod(), Timeout(5000)]
        public void TestStress4()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 0; i < 500; i++)
            {
                s.SetContentsOfCell("A1" + i, "=A1" + (i + 1));
            }
            LinkedList<string> firstCells = new LinkedList<string>();
            LinkedList<string> lastCells = new LinkedList<string>();
            for (int i = 0; i < 250; i++)
            {
                firstCells.AddFirst("A1" + i);
                lastCells.AddFirst("A1" + (i + 250));
            }
            //foreach (string cell in firstCells)
            //{
            //    Console.WriteLine(cell);
            //}
            //Console.Write("***************************************************************************************");
            //foreach (string cell in s.SetCellContents("A1249", 25.0))
            //{
            //    Console.WriteLine(cell);
            //}
            Assert.IsTrue(s.SetContentsOfCell("A1249", "25.0").SequenceEqual(firstCells));
            //foreach (string cell in lastCells)
            //{
            //    Console.WriteLine(cell);
            //}
            //Console.Write("***************************************************************************************\n");
            //foreach (string cell in s.GetNamesOfAllNonemptyCells())
            //{
            //    Console.WriteLine(cell);
            //}
            //Console.WriteLine(s.GetCellContents("A1249"));
            Assert.IsTrue(s.SetContentsOfCell("A1499", "0").SequenceEqual(lastCells));
        }
        [TestMethod(), Timeout(5000)]
        public void TestStress4a()
        {
            TestStress4();
        }
        [TestMethod(), Timeout(5000)]
        public void TestStress4b()
        {
            TestStress4();
        }
        [TestMethod(), Timeout(5000)]
        public void TestStress4c()
        {
            TestStress4();
        }

        [TestMethod(), Timeout(5000)]
        public void TestStress5()
        {
            RunRandomizedTest(47, 2519);
        }

        [TestMethod(), Timeout(5000)]
        public void TestStress6()
        {
            RunRandomizedTest(48, 2521);
        }

        [TestMethod(), Timeout(5000)]
        public void TestStress7()
        {
            RunRandomizedTest(49, 2526);
        }

        [TestMethod(), Timeout(5000)]
        public void TestStress8()
        {
            RunRandomizedTest(50, 2521);
        }

        /// <summary>
        /// Sets random contents for a random cell 10000 times
        /// </summary>
        /// <param name="seed">Random seed</param>
        /// <param name="size">The known resulting spreadsheet size, given the seed</param>
        public void RunRandomizedTest(int seed, int size)
        {
            Spreadsheet s = new Spreadsheet();
            Random rand = new Random(seed);
            for (int i = 0; i < 10000; i++)
            {
                try
                {
                    switch (rand.Next(3))
                    {
                        case 0:
                            s.SetContentsOfCell(randomName(rand), "3.14");
                            break;
                        case 1:
                            s.SetContentsOfCell(randomName(rand), "hello");
                            break;
                        case 2:
                            s.SetContentsOfCell(randomName(rand), randomFormula(rand));
                            break;
                    }
                }
                catch (CircularException)
                {
                }
            }
            ISet<string> set = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(size, set.Count);
        }

        /// <summary>
        /// Generates a random cell name with a capital letter and number between 1 - 99
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        private String randomName(Random rand)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(rand.Next(26), 1) + (rand.Next(99) + 1);
        }

        /// <summary>
        /// Generates a random Formula
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        private String randomFormula(Random rand)
        {
            String f = randomName(rand);
            for (int i = 0; i < 10; i++)
            {
                switch (rand.Next(4))
                {
                    case 0:
                        f += "+";
                        break;
                    case 1:
                        f += "-";
                        break;
                    case 2:
                        f += "*";
                        break;
                    case 3:
                        f += "/";
                        break;
                }
                switch (rand.Next(2))
                {
                    case 0:
                        f += 7.2;
                        break;
                    case 1:
                        f += randomName(rand);
                        break;
                }
            }
            return f;
        }
    }
}
