#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Input;
using WaveEngine.Framework;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Services; 
#endregion

namespace CubeTestProject
{
    public class MySceneBehavior: SceneBehavior
    {
        protected override void ResolveDependencies()
        {
            
        }

        protected override void Update(TimeSpan gameTime)
        {
            var input = WaveServices.Input;

            Labels.Add("X", input.MouseState.X.ToString());
            Labels.Add("Y", input.MouseState.Y.ToString());
            Labels.Add("BLeft", (input.MouseState.LeftButton == ButtonState.Pressed) ? "true" : "false");
            Labels.Add("BMiddle", (input.MouseState.MiddleButton == ButtonState.Pressed) ? "true" : "false");
            Labels.Add("BRight", (input.MouseState.RightButton == ButtonState.Pressed) ? "true" : "false");
            Labels.Add("Wheel", WaveServices.Input.MouseState.Wheel.ToString());
        }
    }
}
