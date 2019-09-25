using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
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
        private static readonly string validName = @"^[a-zA-Z]+[0-9]+$";//Regex to determine if cell name matches basic requirements
        private static readonly string isFormula = @"^=";//Regex to determine if string is a formula

        public override bool Changed { get; protected set; }

        /// <summary>
        /// Initializes Spreadsheet with a Dictionary of Cells and DependencyGraph, uses self=>self as normalizer,
        /// no extra validity conditions, and has version "default"
        /// </summary>
        public Spreadsheet() : this(s => true, s => s, "default")
        {
        }
        
        /// <summary>
        /// Initializes Spreadsheet with a Dictionary of Cells and DependencyGraph, uses provided delegate normalizer,
        /// and provided delegate validity conditions, and has version given by string
        /// </summary>
        /// <param name="isValid">Validity Checker Delegate for Cell Names</param>
        /// <param name="normalize">Normalizer Delegate for Cell Names</param>
        /// <param name="version">Version of the Spreadsheet</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version): base(isValid, normalize, version)
        {
            this.cells = new Dictionary<string, Cell>();
            this.graph = new DependencyGraph();
        }

        /// <summary>
        /// Initializes Spreadsheet with a Dictionary of Cells and DependencyGraph, uses provided delegate normalizer,
        /// and provided delegate validity conditions, and has version given by string
        /// </summary>
        /// <param name="filepath">string name of filepath to load for spreadsheet</param>
        /// <param name="isValid">Validity Checker Delegate for Cell Names</param>
        /// <param name="normalize">Normalizer Delegate for Cell Names</param>
        /// <param name="version">Version of the Spreadsheet</param>
        public Spreadsheet(string filepath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            //IMPLEMENT SOME GETTERS TO GET THE FILE AND POPULATE THE DICTIONARY AND DEPENDENCYGRAPH FROM IT
            throw new NotImplementedException();
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
        protected override IList<string> SetCellContents(string name, double number)
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
        protected override IList<string> SetCellContents(string name, string text)
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
        protected override IList<string> SetCellContents(string name, Formula formula)
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
            if (name is null)//double checks name is not null or invalid
                throw new ArgumentNullException();
            else if (!Regex.IsMatch(name, validName) || !IsValid(Normalize(name)))
                throw new InvalidNameException();
            else
                return new List<string>(graph.GetDependents(name));
        }

        public override string GetSavedVersion(string filename)
        {
            throw new NotImplementedException();
        }

        public override void Save(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.OmitXmlDeclaration = true;

            using(XmlWriter w = XmlWriter.Create(filename, settings))
            {
                w.WriteStartDocument();
                w.WriteStartElement("spreadsheet");
                // This adds an attribute to the Nation element
                w.WriteAttributeString("version", Version);
                foreach(string cell in GetNamesOfAllNonemptyCells())
                {
                    CelltoXML(w, cell);
                }
                
                w.WriteEndElement(); // Ends the spreadsheet block
                w.WriteEndDocument();
            }
            Changed = false;
        }

        public override object GetCellValue(string name)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Sets contents of cell with name to content. Throws InvalidNameException if name is null or invalid 
        /// (checking validity with both basic requirements and provided delegate. Throws Argument NullException if content is null.
        /// If it is a number, sets it to that number. If it is text, sets it to that text,
        /// if it is a formula, sets it to that formula
        /// </summary>
        /// <param name="name">Cell name</param>
        /// <param name="content">Content to set</param>
        /// <returns></returns>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            if (content is null)//check if content is null
                throw new ArgumentNullException();
            else if (name is null || !Regex.IsMatch(name, validName) || !IsValid(Normalize(name)))//check name is not null or invalid
                throw new InvalidNameException();
            //checks if cell previously contained Formula, and removes previous dependencies if it had any.
            else if (cells.ContainsKey(name) && cells[name].contents.GetType() == typeof(Formula))
            {
                Formula f = (Formula)cells[name].contents;
                foreach (string dependency in f.GetVariables())
                {
                    graph.RemoveDependency(dependency, name);
                }
            }
            double dcontents;
            if (Double.TryParse(content, out dcontents))//checks if content is double
            {
                if (cells.ContainsKey(name) && cells[name].contents.GetType() == typeof(double) && dcontents == (double)GetCellContents(name))
                    Changed = false;
                else                
                    Changed = true;
                return this.SetCellContents(name, dcontents);
            }
            else if (Regex.IsMatch(content, isFormula))//checks if content is a formula
            {
                if (cells.ContainsKey(name) && cells[name].contents.GetType() == typeof(Formula) && content.TrimStart('=') == (GetCellContents(name).ToString()))
                    Changed = false;
                else
                    Changed = true;                
                return this.SetCellContents(name, new Formula(content.TrimStart('='), Normalize, IsValid));                    
            }
            if ((content == "" && !cells.ContainsKey(name)) || (cells.ContainsKey(name) && cells[name].contents.GetType() == typeof(string) && content == (string)GetCellContents(name)))
                Changed = false;
            else            
                Changed = true;            
            return this.SetCellContents(name, content);//sets cell content as string text of content
                        
        }
        /// <summary>
        /// Writes Cell Contents to an XML format
        /// </summary>
        private void CelltoXML(XmlWriter w, string name)
        {
            w.WriteStartElement("cell");
            w.WriteElementString("name", name);
            w.WriteElementString("contents", GetCellContents(name).ToString());
            w.WriteEndElement();
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