// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens
//
//
//(Hunter Schmidt)
//Version 1.3 (9/10/19)
// Change log:
// (Version 1.3) Implemented class


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private readonly String expression;
        private List<String> tokens;
        private HashSet<String> vars;
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            this.tokens = new List<string>();
            this.vars = new HashSet<string>();
            string baseVariable = @"^[a-zA-Z_]+[a-zA-Z0-9_]*$";//determines if token matches basic definition of a variable
            string isDouble = @"(^[0-9]+$)|(^[0-9]+.*[0-9]+$)";//determines if a token is a number
            string isOperator = @"^[-+*/()]$";//determines if a token in an operator or parentheses
            String prevToken="";//stores previous token for validation
            StringBuilder sb = new StringBuilder();
            int numParentheses = 0;//to make sure a matching number of parentheses are used
            //Iterates through tokens in expression and validates them
            foreach (String t in GetTokens(formula))
            {
                if (Regex.IsMatch(t, isDouble))//checks if token is a number
                {
                    if (prevToken == ")")//validation
                        throw new FormulaFormatException("Numbers cannot immediately follow a closing parentheses!");
                    this.tokens.Add(double.Parse(t).ToString());//converts to string for normalizing purposes
                    sb.Append(t);
                }
                else if (Regex.IsMatch(t, baseVariable))//checks if token is a variable
                {
                    if (Regex.IsMatch(prevToken, isDouble))//validation
                        throw new FormulaFormatException("The provided variable "+prevToken+t+"Does not meet basic variable criteria!");
                    if (prevToken == ")")
                        throw new FormulaFormatException("Variables cannot immediately follow a closing parentheses!");
                    if (Regex.IsMatch(normalize(t), baseVariable) && isValid(normalize(t)))//validates that variable matches provided delegates
                    {
                        this.tokens.Add(normalize(t));
                        this.vars.Add(normalize(t));
                        sb.Append(normalize(t));
                    }
                    else
                        throw new FormulaFormatException("The provided variable " + t + " does not meet the criteria specified. Please change this variable to a valid one.");
                }
                else if (Regex.IsMatch(t, isOperator))//checks if token is an operator or parentheses
                {
                    if(t == ")")
                    {
                        if(numParentheses == 0)
                           throw new FormulaFormatException("Unexpected ')'");                         
                        else
                        {
                            numParentheses--;                            
                        }
                        
                    }
                    else if (t == "(")
                    {
                        if(Regex.IsMatch(prevToken, isDouble)|| Regex.IsMatch(prevToken, baseVariable))
                        {
                            throw new FormulaFormatException("Cannot follow number or variable with a parentheses!");
                        }
                        numParentheses++;
                    }
                    else if(tokens.Count ==0)
                        throw new FormulaFormatException("Cannot start with an operator!");
                    else
                    {
                        if(Regex.IsMatch(prevToken, @"^[-+*/]$"))
                        {
                            throw new FormulaFormatException("Cannot have two operators in a row!");
                        }
                        else if(prevToken == "(")
                        {
                            throw new FormulaFormatException("Cannot have operator immediately following opening parentheses!");
                        }
                    }


                    this.tokens.Add(t);
                    
                    sb.Append(t);
                }
                else
                    throw new FormulaFormatException("The provided variable " + t + " does not meet the basic criteria for a variable. Please change this variable to a valid one.");
                prevToken = t;
            }
            //End of expression validation
            if (Regex.IsMatch(prevToken, @"^[-+*/]$"))
                throw new FormulaFormatException("Cannot End with an Operator!");
            this.expression = sb.ToString();
            if (formula.Equals(""))
                throw new FormulaFormatException("The Formula was empty!");
            if(numParentheses !=0)
                throw new FormulaFormatException("Uneven number of Parentheses!");

        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Object errorToReturn=null;
            Stack<double> values = new Stack<double>();
            Stack<String> operators = new Stack<String>();
            //System.Console.WriteLine(exp);
            foreach (String token in this.tokens)
            {

                
                //Checks if token is a left paranthesis and pushes it onto the operators stack
                if (token == "(")
                {
                    operators.Push(token);
                }
                //Checks if the token is * or / and then pushes it onto the operators stack
                else if (token == "*" || token == "/")
                    operators.Push(token);
                //Checks if the token is a variable matching the pattern (LettersDigits)
                else if (vars.Contains(token))
                {
                    errorToReturn = performMultDiv(lookup(token), operators, values);
                }
                //Checks if token is a + or - and then performs the operations accordingly
                else if (token == "+" || token == "-")
                {
                    performAddSub(token, operators, values);
                    operators.Push(token);
                }
                //checks if token is an integer and returns its value if it is
                else if (Double.TryParse(token, out double tokenValue))
                {
                    errorToReturn = performMultDiv(tokenValue, operators, values);
                }
                else if (token == ")")
                {
                    if (operators.checkPeek("+"))
                    {
                        performAddSub("+", operators, values);
                    }
                    else if (operators.checkPeek("-"))
                    {
                        performAddSub("-", operators, values);
                    }
                    if (operators.checkPeek("("))
                    {
                        operators.Pop();
                    }
                    else
                    {
                        return new FormulaError("Orphan Right Parentheses");
                    }
                    errorToReturn = performMultDiv(values.Pop(), operators, values);
                }
                else
                {
                    return new FormulaError("Unrecognized operator or variable: "+ token);
                }
                if (errorToReturn != null)
                    return errorToReturn;
            }

            //This is the End of Expression behavior
            //Checks if there are no operators left and if there is one value left
            if (operators.Count == 0 && values.Count == 1)
            {

                return values.Pop();
            }
            //checks if there is more than one operator left or more than two values left
            else if (operators.Count > 1 || values.Count > 2 || (operators.Count == 1 && values.Count < 2))
                return new FormulaError("Too many values or operators left" );
            //Performs final operation and returns result
            else
            {
                if (operators.checkPeek("-"))
                {
                    operators.Pop();
                    double subtractor = values.Pop();
                    return values.Pop() - subtractor;
                }
                else if (operators.checkPeek("+"))
                {
                    operators.Pop();
                    return values.Pop() + values.Pop();
                }

                return new FormulaError("Invalid Expression");
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            
            return new List<String>(this.vars);
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return this.expression;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            return this.GetHashCode().Equals(obj.GetHashCode());
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
               /// <summary>
        /// This method takes a token that has a value, (can be passed value of variable from lookup or normal int,
        /// Then performs the necessary operations to evaluate it or push it onto the stack
        /// </summary>
        /// <param name="tokenValue">Value of the current token</param>
        private static Object performMultDiv(double tokenValue, Stack<String> operators, Stack<double> values)
        {
            if (operators.checkPeek("*"))
            {
                operators.Pop();
                double result = values.Pop() * tokenValue;
                values.Push(result);
            }
            else if (operators.checkPeek("/"))
            {
                if (tokenValue == 0)
                {
                    return new FormulaError("Divide by zero error!");
                }
                else
                {
                    operators.Pop();
                    double result = values.Pop() / tokenValue;
                    values.Push(result);
                }

            }
            else
            {
                values.Push(tokenValue);
            }
            return null;
        }

        /// <summary>
        /// Performs validation for addition or subtraction, then performs the necessary operation
        /// </summary>
        /// <param name="token">current token (should be "+" or "-")</param>
        private static void performAddSub(String token, Stack<String> operators, Stack<double> values)
        {
            if (values.Count >1 && operators.checkPeek("-"))
            {
                operators.Pop();
                double subtractor = values.Pop();
                values.Push(values.Pop() - subtractor);
            }
            else if (values.Count > 1 && operators.checkPeek("+"))
            {
                operators.Pop();
                values.Push(values.Pop() + values.Pop());
            }            
        }

    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
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
        if (stack.Count > 0)
        {
            if (stack.Peek().Equals(token))
            {
                return true;
            }
        }
        return false;
    }
}
