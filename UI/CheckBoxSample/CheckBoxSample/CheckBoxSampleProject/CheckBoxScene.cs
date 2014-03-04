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

namespace CheckBoxSampleProject
{
    public class CheckBoxScene : Scene
    {
        CheckBox checkbox1, checkbox2, checkbox3, checkbox4, checkbox5;
        Button button1;
        TextBlock textblock1, textblock2;
        int checkedCounter, uncheckedCounter;
        Image image;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Gray;
            //RenderManager.DebugLines = true;

            int offsetTop = 20;
            int spacing = 80;

            // Checkbox 1
            checkbox1 = new CheckBox()
            {
                Margin = new Thickness(20, offsetTop, 0, 0),
            };
            EntityManager.Add(checkbox1.Entity);

            // Checkbox 2
            checkbox2 = new CheckBox()
            {
                Margin = new Thickness(20, offsetTop + spacing, 0, 0),
                Text = "Antialising",
                Foreground = Color.Red,
                IsChecked = true,
            };
            EntityManager.Add(checkbox2.Entity);

            // Checkbox 3
            checkbox3 = new CheckBox()
            {
                Margin = new Thickness(20, offsetTop + spacing * 2, 0, 0),
                Width = 250,
                Text = "SegoeBlack20",
                Foreground = Color.Blue,
                IsBorder = true,
            };
            EntityManager.Add(checkbox3.Entity);

            // Checkbox 4
            checkbox4 = new CheckBox()
            {
                Margin = new Thickness(20, offsetTop + spacing * 3, 0, 0),
                Width = 300,
                Text = "You can modify me",
                Foreground = Color.Yellow,
            };
            EntityManager.Add(checkbox4.Entity);

            button1 = new Button()
            {
                Margin = new Thickness(20, offsetTop + spacing * 4, 0, 0),
                Width = 150,
                Text = "Set Checked",
                Foreground = Color.LightPink,
                BackgroundColor = Color.Purple,
            };
            button1.Click += button1_Click;
            EntityManager.Add(button1.Entity);

            // CheckBox 5
            checkbox5 = new CheckBox()
            {
                Margin = new Thickness(20, offsetTop + spacing * 5, 0, 0),
                Text = "Physic",
                Foreground = Color.Orange,
            };
            checkbox5.Checked += checkbox5_Checked;
            EntityManager.Add(checkbox5.Entity);

            textblock1 = new TextBlock()
            {
                Margin = new Thickness(20, offsetTop + spacing * 5.5f, 0, 0),
                Text = "CheckedCount: " + checkedCounter,
                Foreground = Color.Orange,
            };
            EntityManager.Add(textblock1.Entity);

            textblock2 = new TextBlock()
            {
                Margin = new Thickness(20, offsetTop + spacing * 6, 0, 0),
                Text = "UncheckedCount: " + uncheckedCounter,
                Foreground = Color.Orange,
            };
            EntityManager.Add(textblock2.Entity);

            // Image Check
            AddCheckImage("Content/CheckBoxSample.wpk");
        }

        /// <summary>
        /// Handles the Unchecked event of the checkbox5 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void checkbox5_Unchecked(object sender, EventArgs e)
        {
            textblock2.Text = "UncheckedCount: " + ++uncheckedCounter;
        }

        /// <summary>
        /// Handles the Checked event of the checkbox5 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void checkbox5_Checked(object sender, WaveEngine.Common.Helpers.BoolEventArgs e)
        {
            if (e.Value)
            {
                textblock1.Text = "CheckedCount: " + ++checkedCounter;
            }
            else
            {
                textblock2.Text = "UncheckedCount: " + ++uncheckedCounter;
            }
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void button1_Click(object sender, EventArgs e)
        {
            checkbox4.IsChecked = true;
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
