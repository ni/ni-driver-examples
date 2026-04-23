using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NationalInstruments.Examples.ScopeSynchronizationUsingTClock
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
