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
            //Console.WriteLine(Evaluator.Evaluate("1 + A1", BasicVarsLookup));
            testBasicExp();
            testVars();


            System.Console.Read();
        }
        /// <summary>
        /// Tests valid and invalid use cases for variables
        /// </summary>
        public static void testVars()
        {
            try
            {
                Console.WriteLine(Evaluator.Evaluate("1 + A1", NoVarsTestLookup));
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Variable with no value exception caught.");
            }
            try
            {
                Console.WriteLine(Evaluator.Evaluate("1 + A1", BasicVarsLookup));
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: int + Var");
            }
            try
            {
                Console.WriteLine(Evaluator.Evaluate("1 - A1", BasicVarsLookup));
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: int - Var");
            }
            try
            {
                Console.WriteLine(Evaluator.Evaluate("1 * A1", BasicVarsLookup));
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: int * Var");
            }
            try
            {
                Console.WriteLine(Evaluator.Evaluate("1 / A1", BasicVarsLookup));
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: int / Var");
            }

            try
            {
                Console.WriteLine(Evaluator.Evaluate("A1 + 1", BasicVarsLookup));
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: Var + int");
            }

            try
            {
                Console.WriteLine(Evaluator.Evaluate("A1 - 1", BasicVarsLookup));
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: Var - int");
            }

            try
            {
                Console.WriteLine(Evaluator.Evaluate("A1 * 1", BasicVarsLookup));
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: Var * int");
            }

            try
            {
                Console.WriteLine(Evaluator.Evaluate("A1 / 1", BasicVarsLookup));
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: Var / int");
            }

            try
            {
                Console.WriteLine(Evaluator.Evaluate("A1 + B1", BasicVarsLookup));
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: Var + Var");
            }

            try
            {
                Console.WriteLine(Evaluator.Evaluate("A1 - B1", BasicVarsLookup));
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: Var - Var");
            }

            try
            {
                Console.WriteLine(Evaluator.Evaluate("A1 * B1", BasicVarsLookup));
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: Var * Var");
            }

            try
            {
                Console.WriteLine(Evaluator.Evaluate("A1 / B1", BasicVarsLookup));
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: Var / Var");
            }

        }
        /// <summary>
        /// Tests basic expression behaviors using integers
        /// </summary>
        public static void testBasicExp()
        {
            try
            {
                int exp=Evaluator.Evaluate("1 + 1", BasicVarsLookup);
                if(exp != 2)
                {
                    throw new Exception("Math not working");
                }
                Console.WriteLine(exp);
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: int + int");
            }

            try
            {
                int exp = Evaluator.Evaluate("1 - 1", BasicVarsLookup);
                if (exp != 0)
                {
                    throw new Exception("Math not working");
                }
                Console.WriteLine(exp);
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: int - int");
            }

            try
            {
                int exp = Evaluator.Evaluate("1 * 1", BasicVarsLookup);
                if (exp != 1)
                {
                    throw new Exception("Math not working");
                }
                Console.WriteLine(exp);
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: int * int");
            }

            try
            {
                int exp = Evaluator.Evaluate("1 / 1", BasicVarsLookup);
                if (exp != 1)
                {
                    throw new Exception("Math not working");
                }
                Console.WriteLine(exp);
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: int / int");
            }
        }
        /// <summary>
        /// Tests operations with parentheses
        /// </summary>
        public static void testParentheses()
        {
            try
            {
                int exp = Evaluator.Evaluate("(1 + 1) -2", BasicVarsLookup);
                if (exp != 0)
                {
                    throw new Exception("Math not working");
                }
                Console.WriteLine(exp);
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: (int + int) - int");
            }

            try
            {
                int exp = Evaluator.Evaluate("(1 + 1) * 3", BasicVarsLookup);
                if (exp != 6)
                {
                    throw new Exception("Math not working");
                }
                Console.WriteLine(exp);
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: (int + int) * int");
            }

            try
            {
                int exp = Evaluator.Evaluate("(1 + 1) / 2", BasicVarsLookup);
                if (exp != 1)
                {
                    throw new Exception("Math not working");
                }
                Console.WriteLine(exp);
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: (int + int) / int");
            }

            try
            {
                int exp = Evaluator.Evaluate("2 / (1 + 1)", BasicVarsLookup);
                if (exp != 1)
                {
                    throw new Exception("Math not working");
                }
                Console.WriteLine(exp);
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: int / (int + int)");
            }

            try
            {
                int exp = Evaluator.Evaluate("3 * (1 + 1)", BasicVarsLookup);
                if (exp != 6)
                {
                    throw new Exception("Math not working");
                }
                Console.WriteLine(exp);
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: int * (int + int)");
            }

            try
            {
                int exp = Evaluator.Evaluate("2 - (1 + 1)", BasicVarsLookup);
                if (exp != 0)
                {
                    throw new Exception("Math not working");
                }
                Console.WriteLine(exp);
            }
            catch
            {
                Console.WriteLine("Basic Variable Evaluation unsuccessful: int - (int + int)");
            }


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
