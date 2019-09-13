using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaTest
{
    [TestClass]
    public class FormulaTest
    {
        /// <summary>
        /// Basic Normalizer for testing, converts all to uppercase
        /// </summary>
        /// <param name="s">string token to normalize</param>
        /// <returns>uppercase version of s</returns>
        public string upperNormalize(string s)
        {
            return s.ToUpper();
        }
        /// <summary>
        /// Checks if a variable is valid given that it must be one or more uppercase letters followed by 1 or more numbers
        /// </summary>
        /// <param name="s">variable</param>
        /// <returns>Whether variable is a valid one</returns>
        public bool onlyCellValid(string s)
        {
            return Regex.IsMatch(s, @"^[A-Z]+[0-9]+$");
        }

        /// <summary>
        /// Tests for invalid basic variables
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void testInvalidBaseVariables()
        {
            Formula f = new Formula("123_ + 2A");
        }

        /// <summary>
        /// Tests for when normalizer makes variables invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void testNormalizeMakesInvalidVariables()
        {
            Formula f = new Formula("a2 + 3", s => s, onlyCellValid);
        }

        /// <summary>
        /// Tests for when Variables are invalid but pass normalizer
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void testNormalizedInvalidVariables()
        {
            Formula f = new Formula("_a2 + _3", upperNormalize, onlyCellValid);
        }
        /// <summary>
        /// Tests the getVars Function for normalized variables
        /// </summary>
        [TestMethod()]
        public void testGetVars()
        {
            Formula f = new Formula("x2 + y1", s => s.ToUpper(), s => true);
            List<String> contains = new List<String>(f.GetVariables());
            Assert.IsTrue(contains.Contains("X2"));
            Assert.IsTrue(contains.Contains("Y1"));
            Assert.IsFalse(contains.Contains("y1"));
        }
        /// <summary>
        /// Tests toString Method
        /// </summary>
        [TestMethod()]
        public void testToString()
        {
            Formula f = new Formula("x2  +  4 *Y4", s => s.ToUpper(), s => true);
            Assert.AreEqual("X2+4*Y4", f.ToString());
        }

        [TestMethod(), Timeout(5000)]
        public void TestSingleNumber()
        {
            Formula f = new Formula("5");
            Assert.AreEqual(5d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestSingleVariable()
        {
            Formula f = new Formula("X5");
            Assert.AreEqual(13d, f.Evaluate(s => 13));
        }

        [TestMethod(), Timeout(5000)]
        public void TestAddition()
        {
            Formula f = new Formula("5+3");
            Assert.AreEqual(8d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestSubtraction()
        {
            Formula f = new Formula("18-10");
            Assert.AreEqual(8d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestMultiplication()
        {
            Formula f = new Formula("2*4");
            Assert.AreEqual(8d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestDivision()
        {
            Formula f = new Formula("16/2");
            Assert.AreEqual(8d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestArithmeticWithVariable()
        {
            Formula f = new Formula("2+X1");
            Assert.AreEqual(6d, f.Evaluate(s => 4));
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUnknownVariable()
        {
            Formula f = new Formula("2+X1");
            f.Evaluate( s => { throw new ArgumentException("Unknown variable"); });
        }

        [TestMethod(), Timeout(5000)]
        public void TestLeftToRight()
        {
            Formula f = new Formula("2*6+3");
            Assert.AreEqual(15d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestOrderOperations()
        {
            Formula f = new Formula("2+6*3");
            Assert.AreEqual(20d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestParenthesesTimes()
        {
            Formula f = new Formula("(2+6)*3");
            Assert.AreEqual(24d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestTimesParentheses()
        {
            Formula f = new Formula("2*(3+5)");
            Assert.AreEqual(16d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestPlusParentheses()
        {
            Formula f = new Formula("2+(3+5)");
            Assert.AreEqual(10d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestPlusComplex()
        {
            Formula f = new Formula("2+(3+5*9)");
            Assert.AreEqual(50d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestOperatorAfterParens()
        {
            Formula f = new Formula("(1*1)-2/2");
            Assert.AreEqual(0d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestComplexTimesParentheses()
        {
            Formula f = new Formula("2+3*(3+5)");
            Assert.AreEqual(26d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestComplexAndParentheses()
        {
            Formula f = new Formula("2+3*5+(3+4*8)*5+2");
            Assert.AreEqual(194d, f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        public void TestDivideByZero()
        {
            Formula f = new Formula("5/0");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }

        [TestMethod(), Timeout(5000)]
        public void TestSingleOperator()
        {
            Formula f = new Formula("5/0");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }

        [TestMethod(), Timeout(5000)]
        public void TestExtraOperator()
        {
            Formula f = new Formula("2+5+");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }

        [TestMethod(), Timeout(5000)]
        public void TestExtraParentheses()
        {
            Formula f = new Formula("2+5*7)");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestPlusInvalidVariable()
        {
            Formula f = new Formula("5+3X");
            f.Evaluate(s => 0);
        }

        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidVariable()
        {
            Formula f = new Formula("3X");
            f.Evaluate(s => 0);
        }

        [TestMethod(), Timeout(5000)]
        public void TestParensNoOperator()
        {
            Formula f = new Formula("5+7+(5)8");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }


        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmpty()
        {
            Formula f = new Formula("");
            f.Evaluate(s => 0);
        }

        [TestMethod(), Timeout(5000)]
        public void TestComplexMultiVar()
        {
            Formula f = new Formula("y1*3-8/2+4*(8-9*2)/5*x7");
            Assert.AreEqual(-33d, f.Evaluate(s => (s == "x7") ? 4 : 1));
        }

        [TestMethod(), Timeout(5000)]
        public void TestComplexNestedParensRight()
        {
            Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            Assert.AreEqual(6d, f.Evaluate(s => 1));
        }

        [TestMethod(), Timeout(5000)]
        public void TestComplexNestedParensLeft()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12d, f.Evaluate(s => 2));
        }

        [TestMethod(), Timeout(5000)]
        public void TestRepeatedVar()
        {
            Formula f = new Formula("a4-a4*a4/a4");
            Assert.AreEqual(0d, f.Evaluate(s => 3));
        }
    }
}
