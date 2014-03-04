// Copyright (C) 2012-2013 Weekend Game Studio
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.UI;

namespace ProgressBarSampleProject
{
    public class ProgressBarScene : Scene
    {
        ProgressBar progressbar1, progressbar2, progressbar3, progressbar4;
        TextBlock textblock1, textblock2, textblock3, textblock4, info1, info2;
        Button button1, button2;
        Image image;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Gray;
            //RenderManager.DebugLines = true;

            // Progress 1
            int progress1Top = 20;
            int spacing = 40;
            textblock1 = new TextBlock()
            {
                Margin = new Thickness(20, progress1Top, 0, 0),
                Text = "Range: [0, 100] Value = 23",
            };
            EntityManager.Add(textblock1.Entity);

            progressbar1 = new ProgressBar()
            {
                Margin = new Thickness(20, progress1Top + spacing, 0, 0),
                Width = 360,
                Value = 23,
            };
            EntityManager.Add(progressbar1.Entity);

            // Progress 2
            int progress2Top = 120;
            textblock2 = new TextBlock()
            {
                Margin = new Thickness(20, progress2Top, 0, 0),
                Text = "Range: [-21, 300] Value= -5",
            };
            EntityManager.Add(textblock2.Entity);

            progressbar2 = new ProgressBar()
            {
                Margin = new Thickness(20, progress2Top + spacing, 0, 0),
                Width = 360,
                Minimum = -21,
                Maximum = 300,
                Value = -5,
                Foreground = Color.OliveDrab,
                Background = Color.LightGreen,
            };
            EntityManager.Add(progressbar2.Entity);

            // Progress 3
            int progress3Top = 220;
            textblock3 = new TextBlock()
            {
                Margin = new Thickness(20, progress3Top, 0, 0),
                Text = "Range: [400, 600] InitValue: 580",
            };
            EntityManager.Add(textblock3.Entity);

            progressbar3 = new ProgressBar()
            {
                Margin = new Thickness(20, progress3Top + spacing, 0, 0),
                Width = 360,
                Minimum = 400,
                Maximum = 600,
                Value = 580,
                Foreground = Color.Purple,
                Background = Color.LightPink,
            };
            progressbar3.ValueChanged += progressbar3_ValueChanged;
            EntityManager.Add(progressbar3.Entity);

            button1 = new Button()
            {
                Margin = new Thickness(20, progress3Top + spacing * 2, 0, 0),
                Text = "Down",
                Foreground = Color.Gray,
                BackgroundColor = Color.LightBlue,
                BorderColor = Color.LightBlue,
            };
            button1.Click += button1_Click;
            EntityManager.Add(button1.Entity);

            info1 = new TextBlock()
            {
                Margin = new Thickness(20, progress3Top + spacing * 3, 0, 0),
                Text = string.Empty,
                Foreground = Color.Purple,
            };
            EntityManager.Add(info1.Entity);

            // Progress 4
            int progress4Top = 380;
            textblock4 = new TextBlock()
            {
                Margin = new Thickness(20, progress4Top, 0, 0),
                Text = "Range: [-300, -100] InitValue: -280",
            };
            EntityManager.Add(textblock4.Entity);

            progressbar4 = new ProgressBar()
            {
                Margin = new Thickness(20, progress4Top + spacing, 0, 0),
                Width = 360,
                Minimum = -300,
                Maximum = -100,
                Value = -280,
                Foreground = Color.Purple,
                Background = Color.LightPink,
            };
            progressbar4.ValueChanged += progressbar4_ValueChanged;
            EntityManager.Add(progressbar4.Entity);

            button2 = new Button()
            {
                Margin = new Thickness(20, progress4Top + spacing * 2, 0, 0),
                Text = "Up",
                Foreground = Color.Gray,
                BackgroundColor = Color.LightBlue,
                BorderColor = Color.LightBlue,
            };
            button2.Click += button2_Click;
            EntityManager.Add(button2.Entity);

            info2 = new TextBlock()
            {
                Margin = new Thickness(20, progress4Top + spacing * 3, 0, 0),
                Text = string.Empty,
                Foreground = Color.Purple,
            };
            EntityManager.Add(info2.Entity);

            // Image Check
            AddCheckImage("Content/ProgressBarSample.wpk");
        }

        /// <summary>
        /// Handles the Click event of the button2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void button2_Click(object sender, EventArgs e)
        {
            int currentvalue = progressbar4.Value;
            if (currentvalue + 20 <= progressbar4.Maximum)
            {
                progressbar4.Value += 20;
            }
            else
            {
                progressbar4.Value = progressbar4.Minimum;
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the progressbar4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ChangedEventArgs" /> instance containing the event data.</param>
        void progressbar4_ValueChanged(object sender, ChangedEventArgs e)
        {
            info2.Text = "NewValue: " + e.NewValue + " OldValue: " + e.OldValue;
        }

        /// <summary>
        /// Handles the ValueChanged event of the progressbar3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ChangedEventArgs" /> instance containing the event data.</param>
        void progressbar3_ValueChanged(object sender, ChangedEventArgs e)
        {
            info1.Text = "NewValue: " + e.NewValue + " OldValue: " + e.OldValue;
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void button1_Click(object sender, EventArgs e)
        {
            int currentvalue = progressbar3.Value;
            if (currentvalue - 20 >= progressbar3.Minimum)
            {
                progressbar3.Value -= 20;
            }
            else
            {
                progressbar3.Value = progressbar3.Maximum;
            }
        }

        /// <summary>
        /// Adds the check image.
        /// </summary>
        /// <param name="filename">The filename.</param>
        private void AddCheckImage(string filename)
        {
            image = new Image(filename)
            {
                Margin = new Thickness(400, 0, 0, 0),
                Width = 400,
                Height = 600,
            };
            EntityManager.Add(image.Entity);
        }
    }
}
