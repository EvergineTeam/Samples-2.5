// Copyright (C) 2014 Weekend Game Studio
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

#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
#endregion

namespace RadioButtonSampleProject
{
    public class MyScene : Scene
    {
        RadioButton radio1, radio2, radio3, radio4, radio5, radio6;
        TextBlock textBlock;
        ToggleSwitch toggle;
        Image image;

        protected override void CreateScene()
        {
            FixedCamera2D camera2d = new FixedCamera2D("camera");
            camera2d.BackgroundColor = Color.Gray;
            EntityManager.Add(camera2d);            

            // Group 1
            int group1Top = 20;
            radio1 = new RadioButton()
            {
                Width = 360,
                Margin = new Thickness(20, group1Top, 0, 0),
                Text = "Group 1 - Choice 1",
                GroupName = "First group",
                Foreground = Color.Black,
                IsChecked = true
            };
            EntityManager.Add(radio1.Entity);

            radio2 = new RadioButton()
            {
                Width = 360,
                Margin = new Thickness(20, group1Top + 40, 0, 0),
                Text = "Group 1 - Choice 2",
                GroupName = "First group",
                Foreground = Color.Black
            };
            EntityManager.Add(radio2.Entity);

            // Free
            radio3 = new RadioButton()
            {
                Width = 180,
                Margin = new Thickness(20, 180, 0, 0),
                Text = "Free choice",
                IsBorder = true,
                BorderColor = Color.Yellow
            };
            EntityManager.Add(radio3.Entity);

            // Toggle
            toggle = new ToggleSwitch()
            {
                Margin = new Thickness(220, 180, 0, 0)
            };
            toggle.Toggled += toggle_Toggled;
            EntityManager.Add(toggle.Entity);

            // Group 2
            int group2Top = 300;
            radio4 = new RadioButton()
            {
                Width = 360,
                Margin = new Thickness(20, group2Top, 0, 0),
                Text = "Group 2 - Choice 1",
                GroupName = "Second group",
                Foreground = Color.Yellow,
                IsChecked = true
            };
            radio4.Checked += radio_group2_Checked;
            EntityManager.Add(radio4.Entity);

            radio5 = new RadioButton()
            {
                Width = 360,
                Margin = new Thickness(20, group2Top + 40, 0, 0),
                Text = "Group 2 - Choice 2",
                GroupName = "Second group",
                Foreground = Color.LightSalmon,
            };
            radio5.Checked += radio_group2_Checked;
            EntityManager.Add(radio5.Entity);

            radio6 = new RadioButton()
            {
                Width = 360,
                Margin = new Thickness(20, group2Top + 80, 0, 0),
                Text = "Group 2 - Choice 3",
                GroupName = "Second group",
                Foreground = Color.LightSkyBlue
            };
            radio6.Checked += radio_group2_Checked;
            EntityManager.Add(radio6.Entity);

            // Event text
            textBlock = new TextBlock()
            {
                Width = 360,
                Height = 40,
                Margin = new Thickness(20, group2Top + 120, 0, 0),
                Text = "Selected:"
            };
            EntityManager.Add(textBlock.Entity);

            // Image check
            AddCheckImage("Content/RadioButtonSample.wpk");
        }

        /// <summary>
        /// Handles the Toggled event of the toggle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void toggle_Toggled(object sender, EventArgs e)
        {
            radio3.IsChecked = toggle.IsOn;
        }

        /// <summary>
        /// Handles the Checked event of the radio_group2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void radio_group2_Checked(object sender, EventArgs e)
        {
            textBlock.Text = "Selected: " + (sender as RadioButton).Text;
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
