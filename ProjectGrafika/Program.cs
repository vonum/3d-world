using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProjectGrafika
{
    using System;
    using System.Windows.Forms;
    using Tao.OpenGl;

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
            Application.Run(new Form1());
        }
    }
}
