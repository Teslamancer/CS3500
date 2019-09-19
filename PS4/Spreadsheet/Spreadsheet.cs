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
        private static readonly string validName = @"^[a-zA-Z_]+[a-zA-Z0-9_]*$";//determines if cell name matches basic requirements
        public Spreadsheet()
        {
            this.cells = new Dictionary<string, Cell>();
            this.graph = new DependencyGraph();
        }        
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

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return new List<string>(cells.Keys);
        }

        public override IList<string> SetCellContents(string name, double number)
        {
            cells[name] = new Cell(name, number);
            return new List<string>(GetCellsToRecalculate(name));//uses GetCellsToRecalculate to get all dependents
        }

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

        public override IList<string> SetCellContents(string name, Formula formula)
        {
            cells[name] = new Cell(name, formula);//Adds cell to dictionary
            foreach(string var in formula.GetVariables())//adds dependencies to graph
            {
                graph.AddDependency(var, name);
            }
            return new List<string>(GetCellsToRecalculate(name));//uses GetCellsToRecalculate to get all dependents
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name is null)
                throw new ArgumentNullException();
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
                if (contents is null)
                    throw new ArgumentNullException();                
                else if (System.Text.RegularExpressions.Regex.IsMatch(name, validName))
                    this.name = name;
                else
                    throw new InvalidNameException();
                this.contents = contents;
            }

            public IList<string> getDependents(string name)
            {
                HashSet<string> dependents = new HashSet<string>();
                dependents.Add(name);
                RGetDependents(name, name, dependents);
                return new List<string>(dependents);
            }

            private HashSet<string>RGetDependents(string start, string name, HashSet<string> visited)
            {
                throw new NotImplementedException();
            }
            


        }
    }
        
}

