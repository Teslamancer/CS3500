using System;
using System.Collections.Generic;
using System.Text;
using SpreadsheetUtilities;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
        private DependencyGraph graph = new DependencyGraph();
        private static readonly string validName = @"^[a-zA-Z_]+[a-zA-Z0-9_]*$";//determines if cell name matches basic requirements
        public Spreadsheet()
        {

        }
        public override object GetCellContents(string name)
        {
            if(name is null)
            {
                throw new InvalidNameException();
            }
            else if(cells.ContainsKey(name))
            {                
                return cells[name].contents;
            }
            else
            {
                return null;
            }
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            throw new NotImplementedException();
        }

        public override IList<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
        }

        public override IList<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override IList<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// This class represents one cell of a Spreadsheet, containing it's contents (what is entered into the cell) and it's value 
        /// (what would be displayed in the cell). It keeps track of it's dependencies and dependees with a DependencyGraph, and if it contains
        /// an expression, that is stored as a Formula object.
        /// </summary>
        private class Cell
        {
            protected string name;
            public object contents;
            public object value;     
            /// <summary>
            /// This constructor allows the Spreadsheet to initialize the cell with its contents
            /// </summary>
            /// <param name="contents"></param>
            Cell(string name, Object contents)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(name, validName))
                    this.name = name;
                else
                    throw new InvalidNameException();
                this.contents = contents;
            }
            /// <summary>
            /// This constructor initializes an empty cell;
            /// </summary>
            Cell(string name)
            {
                this.contents = "";
                this.value = "";
                this.name = name;
            }
        }
    }
        
}

