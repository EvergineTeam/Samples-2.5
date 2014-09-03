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

namespace TextBoxSampleProject
{
    public class MyScene : Scene
    {
        TextBox textBoxSingle, textBoxMultiple1, textBoxMultiple2;
        Button button;
        Image image;

        protected override void CreateScene()
        {
            FixedCamera2D camera2d = new FixedCamera2D("camera");
            camera2d.BackgroundColor = Color.Gray;
            EntityManager.Add(camera2d);

            // TextBox Single
            textBoxSingle = new TextBox()
            {
                Margin = new Thickness(20, 20, 0, 0),
                Width = 360,
                Height = 30,
                IsBorder = true,
                BorderColor = Color.White,
                Text = "Haz click para escribir"
            };
            EntityManager.Add(textBoxSingle.Entity);

            // TextBox Multiple 1
            textBoxMultiple1 = new TextBox()
            {
                Margin = new Thickness(20, 100, 0, 0),
                Width = 360,
                Height = 200,
                AcceptsReturn = true,
                IsBorder = true,
                BorderColor = Color.Yellow,
                Background = Color.LightGreen,
                Text = "Un mago nunca llega tarde. Ni pronto. Llega justo cuando se le necesita. by Gandalf",
                TextAlignment = TextAlignment.Right,
                TextWrapping = true,
                Foreground = Color.Green
            };
            EntityManager.Add(textBoxMultiple1.Entity);

            // Button
            button = new Button()
            {
                Margin = new Thickness(20, 320, 0, 0),
                Text = "Clear",
                Foreground = Color.Gray,
                BackgroundColor = Color.LightBlue,
                BorderColor = Color.LightBlue,
            };
            button.Click += button_Click;
            EntityManager.Add(button.Entity);

            // TextBox Multiple 2
            textBoxMultiple2 = new TextBox()
            {
                Margin = new Thickness(20, 380, 0, 0),
                Width = 360,
                Height = 200,
                IsBorder = true,
                BorderColor = Color.Brown,
                AcceptsReturn = true,
                Text = "Un mago nunca llega tarde. Ni pronto. Llega justo cuando se le necesita. by Gandalf",
                TextAlignment = TextAlignment.Center,
                TextWrapping = true,
                Foreground = Color.Brown,
                Background = Color.LightSalmon
            };
            EntityManager.Add(textBoxMultiple2.Entity);

            AddCheckImage("Content/TextBoxSample.wpk");
        }

        void button_Click(object sender, EventArgs e)
        {
            textBoxMultiple2.Text = string.Empty;
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
