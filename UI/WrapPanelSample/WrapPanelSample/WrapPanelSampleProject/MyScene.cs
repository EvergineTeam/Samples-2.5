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

namespace WrapPanelSampleProject
{
    public class MyScene : Scene
    {
        ToggleSwitch debugMode;
        WrapPanel wrapPanel;

        protected override void CreateScene()
        {
            FixedCamera2D camera2d = new FixedCamera2D("camera");
            camera2d.BackgroundColor = Color.Gray;
            EntityManager.Add(camera2d);

            // Panel
            wrapPanel = new WrapPanel()
            {
                Width = 400,
                Height = 220,
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            EntityManager.Add(wrapPanel.Entity);

            // Elements
            Button button;
            for (int i = 0; i < 5; i++)
            {
                button = new Button()
                {
                    Text = i.ToString(),
                    Width = 100,
                    Height = 100,
                    Foreground = Color.Yellow,
                    BackgroundColor = Color.Red,
                };
                wrapPanel.Add(button);
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
            switch (this.wrapPanel.Orientation)
            {
                case Orientation.Horizontal:
                    this.wrapPanel.Orientation = Orientation.Vertical;
                    break;
                case Orientation.Vertical:
                    this.wrapPanel.Orientation = Orientation.Horizontal;
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
