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

namespace ToggleSwitchSampleProject
{
    public class ToggleSwitchScene : Scene
    {
        ToggleSwitch toggle1, toggle2, toggle3, toggle4, toggle5;
        TextBlock textblock1;
        Button button1, button2;
        Image image;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Gray;
            //RenderManager.DebugLines = true;

            // Toggle 1
            int offsetTop = 20;
            int spacing = 80;
            toggle1 = new ToggleSwitch()
            {
                Margin = new Thickness(20, offsetTop, 0, 0),
            };
            EntityManager.Add(toggle1.Entity);

            // Toggle 2
            toggle2 = new ToggleSwitch()
            {
                Margin = new Thickness(20, offsetTop + spacing, 0, 0),
                Width = 150,
                OnText = "High",
                OffText = "Low",
            };
            EntityManager.Add(toggle2.Entity);

            // Toggle 3
            toggle3 = new ToggleSwitch()
            {
                Margin = new Thickness(20, offsetTop + spacing * 2, 0, 0),
                Width = 200,
                OnText = "DirectX",
                OffText = "OpenGL",
                Foreground = Color.Purple,
                Background = Color.LightPink,
            };
            EntityManager.Add(toggle3.Entity);

            // Toggle 4
            toggle4 = new ToggleSwitch()
            {
                Margin = new Thickness(20, offsetTop + spacing * 3, 0, 0),
                Width = 200,
                OnText = "Victory!",
                OffText = "You lose",
                Foreground = Color.Yellow,
                Background = Color.Red,
                TextColor = Color.Yellow,
                IsOn = true,
            };
            toggle4.Toggled += toggle4_Toggled;
            EntityManager.Add(toggle4.Entity);

            textblock1 = new TextBlock()
            {
                Margin = new Thickness(20, offsetTop + spacing * 4, 0, 0),
                Text = "Event:",
            };
            EntityManager.Add(textblock1.Entity);

            // Toggle 5
            toggle5 = new ToggleSwitch()
            {
                Margin = new Thickness(20, offsetTop + spacing * 5, 0, 0),
                Width = 200,
                OnText = "Sonic",
                OffText = "MarioBros",
                Foreground = Color.Orange,
                Background = Color.DarkGray,
                TextColor = Color.Orange,
            };
            EntityManager.Add(toggle5.Entity);

            button1 = new Button()
            {
                Margin = new Thickness(20, offsetTop + spacing * 6, 0, 0),
                Text = "Set On",
            };
            button1.Click += button1_Click;
            EntityManager.Add(button1.Entity);

            button2 = new Button()
            {
                Margin = new Thickness(130, offsetTop + spacing *6,0,0),
                Text = "Set Off",
            };
            button2.Click += button2_Click;
            EntityManager.Add(button2.Entity);

            // Image Check
            AddCheckImage("Content/ToggleSwitchSample.wpk");
        }

        /// <summary>
        /// Handles the Toggled event of the toggle4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void toggle4_Toggled(object sender, EventArgs e)
        {
            textblock1.Text = "Event: toggle=" + toggle4.IsOn.ToString();
        }

        /// <summary>
        /// Handles the Click event of the button2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void button2_Click(object sender, EventArgs e)
        {
            toggle5.IsOn = false;
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void button1_Click(object sender, EventArgs e)
        {
            toggle5.IsOn = true;
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
