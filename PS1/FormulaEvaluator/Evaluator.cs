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

        private static Stack<int> values = new Stack<int>();
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
            //removes whitespace of all kinds from expression
            exp = Regex.Replace(exp, @"[\t\s\n\r]", "", RegexOptions.Multiline);
            //System.Console.WriteLine(exp);
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            foreach (String token in substrings)
            {

                //Checks if token is empty string and ignores it if it is
                if (token == "")
                {
                    continue;
                }
                //Checks if token is a left paranthesis and pushes it onto the operators stack
                else if(token == "(")
                {
                    operators.Push(token);
                }
                //Checks if the token is * or / and then pushes it onto the operators stack
                else if (token == "*" || token == "/")
                    operators.Push(token);
                //Checks if the token is a variable matching the pattern (LettersDigits)
                else if (Regex.IsMatch(token, @"^[A-Z]+\d+[^\D]*$", RegexOptions.IgnoreCase))
                {
                    performMultDiv(variableEvaluator(token));
                }
                //Checks if token is a + or - and then performs the operations accordingly
                else if (token == "+"||token == "-")
                {
                    performAddSub(token);
                    operators.Push(token);
                }
                //checks if token is an integer and returns its value if it is
                else if (int.TryParse(token, out int tokenValue))
                {
                    performMultDiv(tokenValue);
                }
                else if (token == ")")
                {
                    if (operators.checkPeek("+"))
                    {
                        performAddSub("+");                        
                    }
                    else if (operators.checkPeek("-"))
                    {
                        performAddSub("-");
                    }
                    if (operators.checkPeek("("))
                    {
                        operators.Pop();
                    }
                    else
                    {
                        throw new ArgumentException("Invalid Expression");
                    }
                    performMultDiv(values.Pop());
                }
                else
                {
                    throw new ArgumentException("Invalid Variable!");
                }
                              
            }

            //This is the End of Expression behavior
            //Checks if there are no operators left and if there is one value left
            if (operators.Count == 0 && values.Count == 1)
            {
                return values.Pop();
            }
            //checks if there is more than one operator left or more than two values left
            else if (operators.Count > 1 || values.Count>2 || (operators.Count ==1 && values.Count<2))
                throw new ArgumentException("Invalid Expression");
            //Performs final operation and returns result
            else
            {
                if (operators.checkPeek("-"))
                {
                    operators.Pop();
                    int subtractor = values.Pop();
                    return values.Pop() - subtractor;
                }
                else if (operators.checkPeek("+"))
                {
                    operators.Pop();
                    return values.Pop() + values.Pop();
                }

                throw new ArgumentException("Invalid Expression");    
            }
        }
        /// <summary>
        /// This method takes a token that has a value, (can be passed value of variable from lookup or normal int,
        /// Then performs the necessary operations to evaluate it or push it onto the stack
        /// </summary>
        /// <param name="tokenValue">Value of the current token</param>
        private static void performMultDiv(int tokenValue)
        {
            if (operators.checkPeek("*"))
            {
                operators.Pop();
                int result = values.Pop() * tokenValue;
                values.Push(result);
            }
            else if (operators.checkPeek("/"))
            {
                if (tokenValue == 0)
                {
                    throw new ArgumentException("Divide by zero error!");
                }
                else
                {
                    operators.Pop();
                    int result = values.Pop() / tokenValue;
                    values.Push(result);
                }

            }
            else
            {
                values.Push(tokenValue);
            }
        }

        /// <summary>
        /// Performs validation for addition or subtraction, then performs the necessary operation
        /// </summary>
        /// <param name="token">current token (should be "+" or "-")</param>
        private static void performAddSub(String token)
        {
            if (values.Count >1 && operators.checkPeek("-"))
            {
                operators.Pop();
                int subtractor = values.Pop();
                values.Push(values.Pop() - subtractor);
            }
            else if (values.Count > 1 && operators.checkPeek("+"))
            {
                operators.Pop();
                values.Push(values.Pop() + values.Pop());
            }            
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
