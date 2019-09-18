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
        /// Tests for invalid character
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void testInvalidToken()
        {
            Formula f = new Formula("1 + ?");
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
        /// Tests toString Function
        /// </summary>
        [TestMethod()]
        public void testToString()
        {
            Formula f = new Formula("x2  +  4 *Y4", s => s.ToUpper(), s => true);
            Assert.AreEqual("X2+4*Y4", f.ToString());
        }

        /// <summary>
        /// Tests Positive Scientific notation
        /// </summary>
        [TestMethod()]
        public void testPosSciNotation()
        {
            Formula f = new Formula("11+1e2");
            Assert.AreEqual(111d, f.Evaluate(x => 0)); ;
        }

        /// <summary>
        /// Tests Negative Scientific notation
        /// </summary>
        [TestMethod()]
        public void testNegSciNotation()
        {
            Formula f = new Formula("11+1e-2");
            Assert.AreEqual(11.01d, f.Evaluate(x => 0)); ;
        }

        /// <summary>
        /// Tests Hashcode and Equals
        /// </summary>
        [TestMethod()]
        public void testEquals()
        {
            Formula f1 = new Formula("1 + 3");
            Formula f2 = new Formula("1 +3");
            Assert.IsTrue(f1.Equals(f2));

            Formula v1 = new Formula("x2+7", upperNormalize, s => true);
            Formula v2 = new Formula("X2 +  7");
            Assert.IsTrue(v1 == v2);

            Assert.IsTrue(v1 != f2);
        }
        /// <summary>
        /// Tests single number formula
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestSingleNumber()
        {
            Formula f = new Formula("5");
            Assert.AreEqual(5d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// Tests single variable formula
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestSingleVariable()
        {
            Formula f = new Formula("X5");
            Assert.AreEqual(13d, f.Evaluate(s => 13));
        }
        /// <summary>
        /// tests basic addition
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestAddition()
        {
            Formula f = new Formula("5+3");
            Assert.AreEqual(8d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// tests basic subtraction
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestSubtraction()
        {
            Formula f = new Formula("18-10");
            Assert.AreEqual(8d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// tests basic multiplication
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestMultiplication()
        {
            Formula f = new Formula("2*4");
            Assert.AreEqual(8d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// tests basic division
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestDivision()
        {
            Formula f = new Formula("16/2");
            Assert.AreEqual(8d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// tests addition with variable
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestArithmeticWithVariable()
        {
            Formula f = new Formula("2+X1");
            Assert.AreEqual(6d, f.Evaluate(s => 4));
        }
        
        /// <summary>
        /// tests left to right evaluation
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestLeftToRight()
        {
            Formula f = new Formula("2*6+3");
            Assert.AreEqual(15d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// tests that order of operations functions properly
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestOrderOperations()
        {
            Formula f = new Formula("2+6*3");
            Assert.AreEqual(20d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// tests parentheses times a number
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestParenthesesTimes()
        {
            Formula f = new Formula("(2+6)*3");
            Assert.AreEqual(24d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// tests a number times parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestTimesParentheses()
        {
            Formula f = new Formula("2*(3+5)");
            Assert.AreEqual(16d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// thests a number plus parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestPlusParentheses()
        {
            Formula f = new Formula("2+(3+5)");
            Assert.AreEqual(10d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// tests a number plus complex parenthetical expression
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestPlusComplex()
        {
            Formula f = new Formula("2+(3+5*9)");
            Assert.AreEqual(50d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// tests order of ops with an operator after parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestOperatorAfterParens()
        {
            Formula f = new Formula("(1*1)-2/2");
            Assert.AreEqual(0d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// tests a complex expression times parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestComplexTimesParentheses()
        {
            Formula f = new Formula("2+3*(3+5)");
            Assert.AreEqual(26d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// tests a complex expression on both sides of parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestComplexAndParentheses()
        {
            Formula f = new Formula("2+3*5+(3+4*8)*5+2");
            Assert.AreEqual(194d, f.Evaluate(s => 0));
        }
        /// <summary>
        /// tests divide by zero returning a formulaerror
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestDivideByZero()
        {
            Formula f = new Formula("5/0");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));            
        }
        /// <summary>
        /// tests divide by 0.0 returning a formulaerror
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestDivideByZeroDouble()
        {
            Formula f = new Formula("5/0.0");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }
        /// <summary>
        /// tests that single operator throws exception
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSingleOperator()
        {
            Formula f = new Formula("+");
        }
        /// <summary>
        /// tests that exception throws with extra operator
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraOperator()
        {
            Formula f = new Formula("2+5+");
        }
        /// <summary>
        /// tests that exception throws with extra right parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraRightParentheses()
        {
            Formula f = new Formula("2+5*7)");
        }
        /// <summary>
        /// tests that exception throws when starting expression with operator
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestStartingOperator()
        {
            Formula f = new Formula("+23 *1");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// tests that exception thrown when ending expression with operator
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEndingOperator()
        {
            Formula f = new Formula("1 +3 -");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// tests that exception thrown when operator follows operator
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOperatorFollowingRule()
        {
            Formula f = new Formula("2+-4");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// tests that exception throws when operator follows open parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOpenParenthesesFollowingRule()
        {            
            Formula f = new Formula("(*3+2)");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// tests that exception thrown when number following closing parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestCloseParenthesesFollowingNum()
        {
            Formula f = new Formula("(3+2)5");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// tests that variable cannot follow closing parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestCloseParenthesesFollowingVar()
        {
            Formula f = new Formula("(3+2)X");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// tests that exception thrown when there is an unmatched opening parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraLeftParentheses()
        {
            Formula f = new Formula("(2+(5*7)");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// tests the exception thrown when open parentheses follows number
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestParenthesesAfterNum()
        {
            Formula f = new Formula("3(4*2)");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// tests that exception thrown when variable preceeds opening parentheses
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestParenthesesAfterVar()
        {
            Formula f = new Formula("X(4*2)");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// tests that number + invalid variable throws exception
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestPlusInvalidVariable()
        {
            Formula f = new Formula("5+3X");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// tests that invalid variable throws exception
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInvalidVariable()
        {
            Formula f = new Formula("3X");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// tests that parentheses with no operator throws exception
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestParensNoOperator()
        {
            Formula f = new Formula("5+7+(5)8");
        }
        /// <summary>
        /// tests that empty formula throws exception
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmpty()
        {
            Formula f = new Formula("");
            f.Evaluate(s => 0);
        }
        /// <summary>
        /// tests that complex expression with multiple variables evaluates correctly
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestComplexMultiVar()
        {
            Formula f = new Formula("y1*3-8/2+4*(8-9*2)/5*x7");
            Assert.AreEqual(-33d, f.Evaluate(s => (s == "x7") ? 4 : 1));
        }
        /// <summary>
        /// tests complex nested parentheses with lots on the right
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestComplexNestedParensRight()
        {
            Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            Assert.AreEqual(6d, f.Evaluate(s => 1));
        }
        /// <summary>
        /// tests complex nested parentheses with ones on the left
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestComplexNestedParensLeft()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12d, f.Evaluate(s => 2));
        }
        /// <summary>
        /// tests a repeated variable
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestRepeatedVar()
        {
            Formula f = new Formula("a4-a4*a4/a4");
            Assert.AreEqual(0d, f.Evaluate(s => 3));
        }
        /// <summary>
        /// tests that valid Base variable that is invalidated by validation delegate throws exception
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void testInvalidValidationVar()
        {
            Formula f = new Formula("a3+3", s =>s, s => s.Equals("A3"));
        }

    }
}
