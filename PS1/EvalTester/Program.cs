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
            Console.WriteLine(Evaluator.Evaluate("20/5", NoVarsTestLookup));
            System.Console.Read();
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
    }
}
