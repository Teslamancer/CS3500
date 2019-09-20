using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;
//Developed by Hunter Schmidt
namespace SS
{
    /// <summary>
    /// This class represents a Spreadsheet composed of Cells. It extends the AbstractSpreadsheet class.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, Cell> cells;
        private DependencyGraph graph;
        private static readonly string validName = @"^[a-zA-Z_]+[a-zA-Z0-9_]*$";//Regex to determine if cell name matches basic requirements
        /// <summary>
        /// Initializes Spreadsheet with a Dictionary of Cells and DependencyGraph
        /// </summary>
        public Spreadsheet()
        {
            this.cells = new Dictionary<string, Cell>();
            this.graph = new DependencyGraph();
        }        
        /// <summary>
        /// Returns contents of cell with name. If value is null or invalid, throws InvalidNameException. If Cell hasn't had a value set (is empty),
        /// returns an empty string.
        /// </summary>
        /// <param name="name">Name of Cell to get contents of</param>
        /// <returns>Contents of cell with name "name"</returns>
        public override object GetCellContents(string name)
        {
            if(name is null || !Regex.IsMatch(name, validName))//checks if name is null or invalid
            {
                throw new InvalidNameException();
            }
            else if(cells.ContainsKey(name))//checks if we have specific data for that name
            {                
                return cells[name].contents;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return new List<string>(cells.Keys);
        }
        /// <summary>
        /// Method to set cell contents to number. Throws InvalidNameException if name is null or invalid. Returns list containing name and all cells whose value
        /// depends on name.
        /// </summary>
        /// <param name="name">Name of cell to set value</param>
        /// <param name="number">double to set cell contents to</param>
        /// <returns>List containing this cell and all of its dependent cells</returns>
        public override IList<string> SetCellContents(string name, double number)
        {
            cells[name] = new Cell(name, number);
            return new List<string>(GetCellsToRecalculate(name));//uses GetCellsToRecalculate to get all dependents
        }
        /// <summary>
        /// Method to set cell contents to text. Throws InvalidNameException if name is null or invalid. 
        /// Throws ArugumentNullException if text is null. Returns list containing name and all cells whose value
        /// depends on name.
        /// </summary>
        /// <param name="name">Name of cell to set value</param>
        /// <param name="text">string to set contents of cell to</param>
        /// <returns>List containing this cell and all of its dependent cells</returns>
        public override IList<string> SetCellContents(string name, string text)
        {
            if (text == "")//Checks if setting cell to empty
            {
                if (cells.ContainsKey(name))
                    cells.Remove(name);
            }
            else
            {
                cells[name] = new Cell(name, text);
            }            
            return new List<string>(GetCellsToRecalculate(name));//uses GetCellsToRecalculate to get all dependents

        }
        /// <summary>
        /// Method to set cell contents to formula. Throws InvalidNameException if name is null or invalid. 
        /// Throws ArgumentNullException if formula is null. Throws CircularException if new Formula would cause a circular dependency, and reverts changes.
        /// Returns list containing name and all cells whose value
        /// depends on name.
        /// </summary>
        /// <param name="name">Name of cell to set value</param>
        /// <param name="formula">Formula to set contents of cell to</param>
        /// <returns>List containing this cell and all of its dependent cells</returns>
        public override IList<string> SetCellContents(string name, Formula formula)
        {
            Cell prevCell = new Cell(name, "");//stores previous cell if it already exists
            if (cells.ContainsKey(name))           
                prevCell = cells[name];                   
            cells[name] = new Cell(name, formula);//Adds/replaces cell in dictionary
            foreach(string var in formula.GetVariables())//adds dependencies to graph for new formula
            {
                graph.AddDependency(var, name);
            }
            try//tries to return all dependencies + cell of named cell
            {
                return new List<string>(GetCellsToRecalculate(name));//uses GetCellsToRecalculate to get all dependents
            }
            catch (CircularException)//if new Formula causes a circular dependency, then catches it
            {
                foreach(string var in formula.GetVariables())//removes dependencies added by circular formula
                {
                    graph.RemoveDependency(var, name);
                }          
                if(prevCell.contents is Formula)//if previous cell contained formula, restores dependencies
                {
                    Formula prevFormula = (Formula)prevCell.contents;
                    foreach (string var in prevFormula.GetVariables())
                    {
                        graph.AddDependency(var, name);
                    }
                }                
                cells[name] = prevCell;//restores cell to internal Dictionary
                throw new CircularException();
            }
            
        }
        /// <summary>
        /// Gets the immediate dependents of the cell with name.
        /// Throws ArgumentNullException if name is null.
        /// Throws InvalidNameException if name is not valid.
        /// </summary>
        /// <param name="name">Cell to get dependents of</param>
        /// <returns>IEnumerable string representation of dependent cells</returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name is null)
                throw new ArgumentNullException();
            else if (!Regex.IsMatch(name, validName))
                throw new InvalidNameException();
            else
                return new List<string>(graph.GetDependents(name));
        }
        /// <summary>
        /// This class represents one cell of a Spreadsheet, containing it's contents (what is entered into the cell) and it's value 
        /// (what would be displayed in the cell). It keeps track of it's dependencies and dependees with a DependencyGraph, and if it contains
        /// an expression, that is stored as a Formula object.
        /// </summary>
        private class Cell
        {
            public string name;
            public object contents;            
            public object value;
            /// <summary>
            /// This constructor initializes an empty cell;
            /// </summary>
            //Cell(string name) : this(name, "") { }
            /// <summary>
            /// This constructor allows the Spreadsheet to initialize the cell with its contents
            /// </summary>
            /// <param name="contents"></param>
            public Cell(string name, Object contents)
            {
                if (contents is null)//ensures cell isn't set to null contents
                    throw new ArgumentNullException();
                else if(name is null || !System.Text.RegularExpressions.Regex.IsMatch(name, validName))//Ensures name is valid (not null or invalid)
                {
                    throw new InvalidNameException();

                }
                else
                {
                    this.name = name;
                    this.contents = contents;
                }                    
            }
        }
    }
        
}

