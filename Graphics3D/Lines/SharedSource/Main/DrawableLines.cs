#region File Description
//-----------------------------------------------------------------------------
// DrawableLines
//
// Copyright © 2012 Weekend Game Studio. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace Lines
{
    [DataContract]
    public class DrawableLines : Drawable3D
    {
        public int StartingLinesCount { get; set; }

        public int MaxLinesCount { get; set; }

        public int Size { get; set; }

        private List<Line> lines;
        private Line l;
        private WaveEngine.Framework.Services.Random random;
        private LineBatch3D lineBatch;

        public DrawableLines()
            : base("DrawableLines")
        {
        }

        protected override void DefaultValues()
        {
            StartingLinesCount = 1;
            MaxLinesCount = 10000;
            Size = 200;
            lines = new List<Line>();
            random = WaveServices.Random;

            l = new Line();
        }

        protected override void Initialize()
        {
            for (int i = 0; i < StartingLinesCount; i++)
            {
                AddNewLine();
            }

            this.lineBatch = this.RenderManager.FindLayer(WaveContent.RenderLayers.Debug).LineBatch3D;
        }

        private void AddNewLine()
        {
            var size = Size;
            l.StartPoint = new Vector3((float)random.NextDouble() * size * (float)Math.Pow(-1, random.Next()),
                                        (float)random.NextDouble() * size * (float)Math.Pow(-1, random.Next()),
                                        (float)random.NextDouble() * size * (float)Math.Pow(-1, random.Next()));

            l.EndPoint = new Vector3((float)random.NextDouble() * size * (float)Math.Pow(-1, random.Next()),
                    (float)random.NextDouble() * size * (float)Math.Pow(-1, random.Next()),
                    (float)random.NextDouble() * size * (float)Math.Pow(-1, random.Next()));

            l.Color = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());

            lines.Add(l);
        }

        public override void Draw(TimeSpan gameTime)
        {
            if (lines.Count < MaxLinesCount)
            {
                for (int i = 0; i < 2; i++)
                {
                    AddNewLine();
                }
            }

            Labels.Add("Lines", lines.Count.ToString());

            for (int i = 0; i < lines.Count; i++)
            {
                Line l = lines[i];
                this.lineBatch.DrawLine(ref l);
            }
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
