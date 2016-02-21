using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Services;

namespace WaveOculusDemoProject.Components
{
    /// <summary>
    /// Action frame period
    /// </summary>
    public class ActionPeriod 
    {
        public int Start;
        public int End;
    }

    /// <summary>
    /// Object frame action model
    /// </summary>
    public class ObjectAction
    {
        /// <summary>
        /// Action Period list
        /// </summary>
        public ActionPeriod[] Periods { get; private set; }

        /// <summary>
        /// Instantiate a new object frame action model
        /// </summary>
        /// <param name="objectPath">The path of the file that specify the action frame values</param>
        public ObjectAction(string objectPath)
        {
            using (var stream = WaveServices.Storage.OpenContentFile(objectPath))
            {
                var streamReader = new StreamReader(stream);
                string line = streamReader.ReadLine();                
                this.Periods = new ActionPeriod[int.Parse(line)];

                for (int i = 0; i < this.Periods.Length; i++)
                {
                    line = streamReader.ReadLine();
                    string[] lineParts = line.Split(' ');
                    ActionPeriod period = new ActionPeriod()
                    {
                        Start = int.Parse(lineParts[0]),
                        End = int.Parse(lineParts[1])
                    };

                    this.Periods[i] = period;
                }
            }
        }
    }
}
