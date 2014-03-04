using System;
using System.Diagnostics;
using System.Windows.Forms;
using WaveEngine.Adapter;

namespace MotorRevoluteJoint2DSample
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using (App game = new App())
            {
                game.Run();
            }
        }
    }
}

