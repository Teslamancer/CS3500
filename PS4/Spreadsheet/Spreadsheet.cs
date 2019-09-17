using System;
using System.Collections.Generic;
using System.Text;
using SpreadsheetUtilities;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private HashSet<Cell> cells = new HashSet<Cell>();
        public Spreadsheet()
        {

        }
        public override object GetCellContents(string name)
        {
            throw new NotImplementedException();
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
            private string name;
            private Formula formulaContent;
            private double doubleContent;
            private string stringContent;
            private Object value;
            public DependencyGraph graph { private set; get; }
            /// <summary>
            /// This constructor allows the Spreadsheet to initialize the cell with its contents
            /// </summary>
            /// <param name="contents"></param>
            Cell(string name, Object contents)
            {
                this.name = name;
                if(contents.GetType() == typeof(Formula))
                {
                    this.formulaContent = (Formula)contents;
                    this.graph = new DependencyGraph();
                    foreach(string cellName in this.formulaContent.GetVariables())
                    {
                        this.graph.AddDependency(cellName, this.name);
                    }
                }
                else if(contents.GetType() == typeof(string))
                {
                    this.stringContent = (string)contents;
                    this.value = this.stringContent;
                }
                else if(contents.GetType() == typeof(double))
                {
                    this.doubleContent = (double)contents;
                    this.value = this.doubleContent;
                }
            }
            /// <summary>
            /// This constructor initializes an empty cell;
            /// </summary>
            Cell(string name)
            {
                this.stringContent = "";
                this.value = "";
                this.name = name;
            }
        }
    }
        
}

