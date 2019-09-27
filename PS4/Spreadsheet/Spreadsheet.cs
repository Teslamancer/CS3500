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
        /// <summary>
        /// Indicates whether a spreadsheet has been changed since its creation, last save, or loading from file.
        /// </summary>
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
            if (isValid is null || normalize is null || version is null)
                throw new ArgumentNullException();
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
        public Spreadsheet(string filepath, Func<string, bool> isValid, Func<string, string> normalize, string version) : this(isValid, normalize, version)
        {
            try
            {
                using (XmlReader r = XmlReader.Create(filepath))//reads in file using xmlreader
                {
                    r.Read();
                    if (r["version"] != version)//checks for version mismatch
                    {
                        Console.WriteLine("Version Mismatch! Provided version does not match version found.");
                        throw new SpreadsheetReadWriteException("Version Mismatch! Provided version does not match version found.");
                    }
                    else
                    {
                        while (r.Read())//reads file in, inserting cell values from file, skips until cell element found
                        {
                            if (r.IsStartElement() && r.Name=="cell")
                            {
                                readInCell(r);
                            }
                        }
                    }
                        
                }
            }
            catch (ArgumentNullException)//catches if filepath is null
            {
                throw new SpreadsheetReadWriteException("Filepath cannot be null!");
            }
            catch (System.IO.FileNotFoundException)//catches if file cannot be found or path is invalid
            {
                throw new SpreadsheetReadWriteException("File not found!");
            }
            //catch(SpreadsheetReadWriteException e)
            //{
            //    throw e;
            //}


            this.Changed = false;
        }
        /// <summary>
        /// Returns a cell with appropriate data given an XmlReader object at a Cell start tag
        /// </summary>
        /// <param name="r">XmlReader to use</param>
        /// <returns>Cell from Xml File</returns>
        private void readInCell(XmlReader r)
        {
            string contents;
            string name;            
            r.Read();            //read three times to get to name
            r.Read();
            r.Read();
            //r.Read();
            name = r.Value;         //read in name value   
            r.Read();//read four times to get to contents
            r.Read();            
            r.Read();
            r.Read();
            contents = r.Value;   //read in contents value         
            //r.Read();
            //r.Read();
            //r.Read();
            this.SetContentsOfCell(name, contents);//add in new cell with name and contents
        }
        /// <summary>
        /// Returns contents of cell with name. If value is null or invalid(either due to basic requirements or delegate), throws InvalidNameException.
        /// If Cell hasn't had a value set (is empty), returns an empty string.
        /// </summary>
        /// <param name="name">Name of Cell to get contents of</param>
        /// <returns>Contents of cell with name "name"</returns>
        public override object GetCellContents(string name)
        {
            if(name is null || !Regex.IsMatch(name, validName) || !IsValid(Normalize(name)))//checks if name is null or invalid
            {
                throw new InvalidNameException();
            }
            else if(cells.ContainsKey(name))//checks if we have specific data for that name
            {
                object toReturn = cells[name].contents;
                if (toReturn.GetType() == typeof(string))
                    toReturn = (string)toReturn;
                else if (toReturn.GetType() == typeof(double))
                    toReturn = (double)toReturn;
                else if (toReturn.GetType() == typeof(Formula))
                    toReturn = (Formula)toReturn;
                return toReturn;
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
            return new List<string>(graph.GetDependents(name));
        }
        /// <summary>
        /// Returns version of saved file with filename
        /// </summary>
        /// <param name="filename">File to get version of</param>
        /// <returns></returns>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                using (XmlReader r = XmlReader.Create(filename))//tries to read in file at filename
                {
                    r.Read();
                    string toReturn = r["version"];
                    if (toReturn is null)
                        throw new SpreadsheetReadWriteException("Invalid File!");
                    else
                        return toReturn;
                }
            }
            catch (System.IO.DirectoryNotFoundException)//catches if directory can't be found
            {
                throw new SpreadsheetReadWriteException("Directory not found!");
            }
            catch (System.IO.FileNotFoundException)//catches if the file doesn't exist
            {
                throw new SpreadsheetReadWriteException("File not found!");
            }
            catch (ArgumentNullException)//catches if filename is null
            {
                throw new SpreadsheetReadWriteException("File name cannot be null!");
            }
            
        }
        /// <summary>
        /// Saves the Spreadsheet to an xml file at filename throws SpreadsheetReadWrite Exception if anything goes wrong with the saving process
        /// </summary>
        /// <param name="filename">path of saved file</param>
        public override void Save(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();//creates settings option for xml writer
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.OmitXmlDeclaration = true;
            try
            {
                using (XmlWriter w = XmlWriter.Create(filename, settings))//Writes beginning and end of XML doc
                {
                    w.WriteStartDocument();
                    w.WriteStartElement("spreadsheet");
                    // This adds an attribute to the Nation element
                    w.WriteAttributeString("version", Version);
                    foreach (string cell in GetNamesOfAllNonemptyCells())//Writes for each nonempty cell in helper method
                    {
                        CelltoXML(w, cell);
                    }

                    w.WriteEndElement(); // Ends the spreadsheet block
                    w.WriteEndDocument();
                }
            }            
            catch(System.IO.DirectoryNotFoundException)//catches if path is invalid
            {
                //Console.WriteLine("InvalidPath");
                throw new SpreadsheetReadWriteException("Invalid Path!");
            }
            catch(System.IO.IOException)//catches if filename is invalid
            {
                //Console.WriteLine("InvalidNameException");
                throw new SpreadsheetReadWriteException("Invalid Filename!");
            }
            Changed = false;
        }
        /// <summary>
        /// Returns value stored at cell with name.
        /// Throws InvalidNameException if name is null or invalid according to delegates or base
        /// </summary>
        /// <param name="name">Cell to retrieve value from</param>
        /// <returns></returns>
        public override object GetCellValue(string name)
        {
            if (name is null || !Regex.IsMatch(name, validName) || !IsValid(Normalize(name)))//checks if name is null or invalid
            {
                throw new InvalidNameException();
            }
            else
            {
                //object toReturn = null;
                //object value = cells[name].value;
                //if (value.GetType() == typeof(FormulaError))//checks if value is formulaerror
                //{
                //    toReturn = new FormulaError();
                //    toReturn = (FormulaError)value;
                //}
                //else if (value.GetType() == typeof(double))//checks if value is double
                //{
                //    toReturn = new double();
                //    toReturn = (double)value;
                //}
                //else//else, makes it string
                //{
                //    toReturn = (string)value;
                //}
                //return toReturn;
                return cells[name].value;
            }
        }
        /// <summary>
        /// Recalculates values of cells after changes to cell with name
        /// </summary>
        /// <param name="changedCell">Name of cell that was just changed</param>
        private void recalulateCellValues(string changedCell)
        {
            foreach(string cell in GetCellsToRecalculate(changedCell))//recalculates value for each cell that was affected
            {
                calculateValue(cell);
            }
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
            IList<string> toReturn = new List<string>();
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
                
                toReturn = this.SetCellContents(name, dcontents);                
            }
            else if (Regex.IsMatch(content, isFormula))//checks if content is a formula
            {
                if (cells.ContainsKey(name) && cells[name].contents.GetType() == typeof(Formula) && content.TrimStart('=') == (GetCellContents(name).ToString()))
                    Changed = false;
                else
                    Changed = true;
                
                toReturn = this.SetCellContents(name, new Formula(content.TrimStart('='), Normalize, IsValid));                    
            }
            else 
            {
                if ((content == "" && !cells.ContainsKey(name)) || (cells.ContainsKey(name) && cells[name].contents.GetType() == typeof(string) && content == (string)GetCellContents(name)))
                    Changed = false;
                else
                    Changed = true;
                toReturn = this.SetCellContents(name, content);//sets cell content as string text of content
            }                        
            recalulateCellValues(name);//recalculates all dependent cells after change
            return toReturn;
                        
        }
        /// <summary>
        /// Writes Cell Contents to an XML format
        /// </summary>
        private void CelltoXML(XmlWriter w, string name)
        {
            string toWrite = "";
            w.WriteStartElement("cell");
            w.WriteElementString("name", name);
            if (cells[name].contents.GetType() == typeof(Formula))
                toWrite = "=";
            w.WriteElementString("contents", toWrite + GetCellContents(name).ToString());
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
            /// This constructor allows the Spreadsheet to initialize the cell with its contents
            /// </summary>
            /// <param name="contents"></param>
            public Cell(string name, Object contents)
            {                            
                this.name = name;
                this.contents = contents;                           
            }         
            
        }
        /// <summary>
        /// Helper method that populates cell with name with its appropriate value
        /// </summary>
        /// <param name="name">Name of cell</param>
        private void calculateValue(string name)
        {
            if (!cells.ContainsKey(name))
                return;
            if (cells[name].contents.GetType() == typeof(string) || cells[name].contents.GetType() == typeof(double))
                cells[name].value = cells[name].contents;
            else
            {
                Formula f = (Formula)cells[name].contents;
                cells[name].value = f.Evaluate(cellLookup);
            }
            
        }
        /// <summary>
        /// Returns value of cell as double given name
        /// </summary>
        /// <param name="name">Cell Name to Lookup</param>
        /// <returns>Value of Cell</returns>
        private double cellLookup(string name)
        {
            if(!cells.ContainsKey(name))
                throw new ArgumentException();
            if (cells[name].value.GetType() == typeof(string))
            {
                throw new ArgumentException();
            }
            else if (cells[name].value.GetType() == typeof(FormulaError))
            {
                throw new ArgumentException();
            }
            else
            {
                return (double)cells[name].value;
            }
        }

    }
        
}