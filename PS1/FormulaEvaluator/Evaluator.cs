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
                //Checks if token is empty string and ignores it if it is
                if (token == "")
                {
                    continue;
                }
                //checks if token is an integer and returns its value if it is
                else if (int.TryParse(token, out int tokenValue))
                {
                    if (operators.checkPeek("*"))
                    { 
                        int result = int.Parse(values.Pop()) * tokenValue;
                        values.Push(result.ToString());
                    }
                    else if (operators.checkPeek("/"))
                    {
                        if (tokenValue == 0)
                        {
                            throw new ArgumentException("Divide by zero error!");
                        }
                        else
                        {
                            int result = int.Parse(values.Pop()) / tokenValue;
                            values.Push(result.ToString());
                        }

                    }
                    else
                    {
                        values.Push(tokenValue.ToString());
                    }
                }
                //TODO: Implement Algorithm here
            }
            return 0;
        }

    }
}

/// <summary>
/// Contains extension methods for the generic stack class
/// </summary>
public static class StackUtils
{
    /// <summary>
    /// Checks if the stack is empty, then checks if the top element is equal to the given string
    /// </summary>
    /// <typeparam name="T">Stack type</typeparam>
    /// <param name="stack">stack to use</param>
    /// <param name="token">String to check against</param>
    /// <returns>True if top element of stack equals the string, false otherwise</returns>
    public static bool checkPeek<T>(this Stack<T> stack, String token)
    {
        if(stack.Count > 0)
        {
            if(stack.Peek().Equals(token))
            {
                return true;
            }
        }
        return false;
    }
}
