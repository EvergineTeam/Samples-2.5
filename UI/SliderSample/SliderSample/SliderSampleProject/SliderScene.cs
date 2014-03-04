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

namespace SliderSampleProject
{
    public class SliderScene : Scene
    {
        Slider slider1, slider2, slider3;
        TextBlock textblock1, textblock2;
        Button button1;
        Image image;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Gray;
            //RenderManager.DebugLines = true;

            // Slider 1
            int slider1Top = 60;
            slider1 = new Slider()
            {
                Margin = new Thickness(20, slider1Top, 0, 0),
                Width = 360,
                Value = 50,
            };
            EntityManager.Add(slider1.Entity);

            // Slider 2
            int slider2Top = 130;
            slider2 = new Slider()
            {
                Margin = new Thickness(20, slider2Top, 0, 0),
                Width = 360,
                Minimum = -20,
                Maximum = 500,
                Value = 10,
                Foreground = Color.Purple,
                Background = Color.LightPink,
                TextColor = Color.Purple,
            };
            EntityManager.Add(slider2.Entity);

            // Slider 3
            int slider3Top = 220;
            int spacing = 60;
            slider3 = new Slider()
            {
                Margin = new Thickness(60, slider3Top, 0, 0),
                Orientation = Orientation.Vertical,
                Height = 360,
                Width = 20,
                Minimum = 40,
                Maximum = 500,
                Value = 80,
                Foreground = Color.Yellow,
                Background = Color.Red,
                TextColor = Color.LightGreen
            };
            slider3.ValueChanged += slider3_ValueChanged;
            EntityManager.Add(slider3.Entity);

            textblock1 = new TextBlock()
            {
                Margin = new Thickness(100, slider3Top, 0, 0),
                Text = "Range: [40, 500] InitValue: 80",
                Width = 300,
                Height = 40,
            };
            EntityManager.Add(textblock1.Entity);

            button1 = new Button()
            {
                Margin = new Thickness(100, slider3Top + spacing, 0, 0),
                Width = 160,
                Text = "Set Value 250",
                Foreground = Color.Gray,
                BackgroundColor = Color.LightBlue,
                BorderColor = Color.LightBlue,
            };
            button1.Click += button1_Click;
            EntityManager.Add(button1.Entity);

            textblock2 = new TextBlock()
            {
                Margin = new Thickness(100, slider3Top + spacing * 2, 0, 0),
                Text = "<Info>",
                Width = 300,
                Height = 40,
                Foreground = Color.Yellow
            };
            EntityManager.Add(textblock2.Entity);

            // Image Check
            AddCheckImage("Content/SliderSample.wpk");
        }

        /// <summary>
        /// Handles the ValueChanged event of the slider3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ChangedEventArgs" /> instance containing the event data.</param>
        void slider3_ValueChanged(object sender, ChangedEventArgs e)
        {
            textblock2.Text = "NewValue: " + e.NewValue + " OldValue: " + e.OldValue;
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void button1_Click(object sender, EventArgs e)
        {
            slider3.Value = 250;
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
