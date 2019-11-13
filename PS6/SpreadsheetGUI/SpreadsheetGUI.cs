using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;

namespace SpreadsheetGUI
{
    /// <summary>
  /// Keeps track of how many top-level forms are running
  /// </summary>
  class SSApplicationContext : ApplicationContext
  {
    // Number of open forms
    private int formCount = 0;

    // Singleton ApplicationContext
    private static SSApplicationContext appContext;

    /// <summary>
    /// Private constructor for singleton pattern
    /// </summary>
    private SSApplicationContext()
    {
    }

    /// <summary>
    /// Returns the one DemoApplicationContext.
    /// </summary>
    public static SSApplicationContext getAppContext()
    {
      if (appContext == null)
      {
        appContext = new SSApplicationContext();
      }
      return appContext;
    }

    /// <summary>
    /// Runs the form
    /// </summary>
    public void RunForm(Form form)
    {
      // One more form is running
      formCount++;

      // When this form closes, we want to find out
      form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

      // Run the form
      form.Show();
    }

  }
    static class SpreadsheetGUI
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //gets appcontext for new Window and runs it
            SSApplicationContext appContext = SSApplicationContext.getAppContext();
            appContext.RunForm(new Window());
            Application.Run(appContext);
        }        
    }
}
