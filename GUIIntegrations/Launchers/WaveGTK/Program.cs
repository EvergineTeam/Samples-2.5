using System;
using Gtk;
using System.IO;

namespace WaveGTK
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Init();
            MainWindow win = new MainWindow();
            win.ShowAll();
            Application.Run();
        }
    }
}
