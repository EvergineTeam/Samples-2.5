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

namespace TextBlockSampleProject
{
    public class MyScene : Scene
    {
        TextBlock textblock1, textblock2, textblock3, textblock4, textblock5, textblock6;
        Image image;

        protected override void CreateScene()
        {
            FixedCamera2D camera2d = new FixedCamera2D("camera");
            camera2d.BackgroundColor = Color.Gray;
            EntityManager.Add(camera2d);
            
            int offsetTop = 20;
            int spacing = 60;

            textblock1 = new TextBlock()
            {
                Margin = new Thickness(20, offsetTop, 0, 0),
            };
            EntityManager.Add(textblock1.Entity);

            textblock2 = new TextBlock()
            {
                Margin = new Thickness(20, offsetTop + spacing, 0, 0),
                Text = "Simple Color text",
                Foreground = Color.Yellow,
            };
            EntityManager.Add(textblock2.Entity);

            textblock3 = new TextBlock()
            {
                Margin = new Thickness(20, offsetTop + spacing * 2, 0, 0),
                Text = "Font: SegoeBlack20",
                Foreground = Color.Orange,
            };
            EntityManager.Add(textblock3.Entity);

            textblock4 = new TextBlock()
            {
                Margin = new Thickness(20, offsetTop + spacing * 3, 0, 0),
                TextWrapping = true,
                Width = 360,
                Height = 100,
                Text = "Text with multi-lines activated and left alignment.",
                IsBorder = true,
                BorderColor = Color.Yellow,
                Foreground = Color.Purple
            };
            EntityManager.Add(textblock4.Entity);

            textblock5 = new TextBlock()
            {
                Margin = new Thickness(20, offsetTop + spacing * 5, 0, 0),
                TextWrapping = true,
                Width = 360,
                Height = 100,
                TextAlignment = TextAlignment.Center,
                Text = "Text with multi-lines activated and center alignment.",
                IsBorder = true,
                BorderColor = Color.Yellow,
                Foreground = Color.Blue
            };
            EntityManager.Add(textblock5.Entity);

            textblock6 = new TextBlock()
            {
                Margin = new Thickness(20, offsetTop + spacing * 7, 0, 0),
                TextWrapping = true,
                TextAlignment = TextAlignment.Right,
                Width = 360,
                Height = 100,
                Text = "Text with multi-lines activated and right alignment.",
                IsBorder = true,
                BorderColor = Color.Yellow,
                Foreground = Color.Red
            };
            EntityManager.Add(textblock6.Entity);

            // Image check
            AddCheckImage("Content/TextBlockSample.wpk");
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
                Width = 401,
                Height = 600,
            };
            EntityManager.Add(image.Entity);
        }
    }
}
