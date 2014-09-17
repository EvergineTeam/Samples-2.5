using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaveEngine.Adapter.Win32;

namespace WaveForm
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
            Form1 form = new Form1();          
            //Application.Run(form);            
            RenderLoop.Run(form, () =>
            {
                form.Render();
            });
        }
    }
}
