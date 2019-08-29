using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary>
    /// This class contains a static function to evaluate a string mathmatical formula given a functor 
    /// to retrieve variable values
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// If the variable cannot be found or has no value, throws ArgumentException
        /// </summary>
        /// <param name="v">The variable to retrieve a value for</param>
        /// <returns>Value of the variable as an int</returns>
        public delegate int Lookup(String v);

        private static Stack<String> values = new Stack<String>();
        private static Stack<String> operators = new Stack<String>();

        /// <summary>
        /// Given a String formula, this method evaluates the formula mathematically,
        /// retrieving variable values using a provided Lookup delegate function
        /// </summary>
        /// <param name="exp">Expression to be evaluated</param>
        /// <param name="variableEvaluator">Function to use when retrieving variable values</param>
        /// <returns>The result of mathematical evaluation of the expression</returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            foreach (String token in substrings)
            {
                token.Trim();
                //TODO: Implement Algorithm here
            }
            return 0;
        }

    }
}
