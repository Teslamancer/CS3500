using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;

namespace EvalTester
{
    /// <summary>
    /// This class contains methods to test the functionality and implementation of the Evaluator class
    /// </summary>
    class Program
    {
        /// <summary>
        /// This method runs all of the tests on the Evaluator class
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            testVars();


            System.Console.Read();
        }
        /// <summary>
        /// Tests valid and invalid use cases for variables
        /// </summary>
        public static void testVars()
        {
            Console.WriteLine(Evaluator.Evaluate("1 + A1", NoVarsTestLookup));
        }



        /// <summary>
        /// Lookup Method that expects no variables, throws an ArgumentException if used
        /// </summary>
        /// <param name="s">Variable to Lookup</param>
        /// <returns>Variable value</returns>
        public static int NoVarsTestLookup(String s)
        {
            throw new ArgumentException("No Variables expected, but Variables found!");
        }
        /// <summary>
        /// Returns the sum of the decimal values of the characters in the variable name
        /// </summary>
        /// <param name="s">Variable to look up value for</param>
        /// <returns>value of variable</returns>
        public static int BasicVarsLookup(String s)
        {
            int toReturn = 0;
            foreach(char c in s.ToCharArray())
            {
                toReturn += (int)c;
            }
            return toReturn;
        }
    }
}
