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
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;

namespace StackPanelSampleProject
{
    public class StackPanelScene : Scene
    {
        StackPanel stackPanel;
        ToggleSwitch debugMode;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Gray;

            // Panel
            stackPanel = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 400,
                Height = 400,
            };
            EntityManager.Add(stackPanel.Entity);

            // Elements
            Button button;
            for (int i = 0; i < 4; i++)
            {
                button = new Button()
                {
                    Text = i.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 100,
                    Height = 100,
                    Foreground = Color.Yellow,
                    BackgroundColor = Color.Red,
                };
                stackPanel.Add(button);  
            }

            // Set Orientation
            Button button1 = new Button()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Text = "Change Orientation",
                Width = 200,
                Margin = new Thickness(20),
                Foreground = Color.LightPink,
                BackgroundColor = Color.Purple,
                BorderColor = Color.LightPink,
            };
            button1.Click += b_Click;
            EntityManager.Add(button1.Entity);

            // Debug
            this.CreateDebugMode();
        }

        /// <summary>
        /// Handles the Click event of the b control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void b_Click(object sender, EventArgs e)
        {
            switch (this.stackPanel.Orientation)
            {
                case Orientation.Horizontal:
                    this.stackPanel.Orientation = Orientation.Vertical;
                    break;
                case Orientation.Vertical:
                    this.stackPanel.Orientation = Orientation.Horizontal;
                    break;
            }

            WaveServices.Layout.PerformLayout(this);
        }

        /// <summary>
        /// Creates the debug mode.
        /// </summary>
        private void CreateDebugMode()
        {
            debugMode = new ToggleSwitch()
            {
                OnText = "Debug On",
                OffText = "Debug Off",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 200,
            };
            debugMode.Toggled += debugMode_Toggled;
            EntityManager.Add(debugMode.Entity);
        }

        /// <summary>
        /// Handles the Toggled event of the debugMode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void debugMode_Toggled(object sender, EventArgs e)
        {
            RenderManager.DebugLines = this.debugMode.IsOn;
        }
    }
}
