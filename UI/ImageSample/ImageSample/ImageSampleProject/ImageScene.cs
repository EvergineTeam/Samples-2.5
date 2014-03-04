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

namespace ImageSampleProject
{
    public class ImageScene : Scene
    {
        Image image1, image2, image3, image4;
        TextBlock textblock1, textblock2, textblock3, textblock4;
        Image image;

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.Gray;
            //RenderManager.DebugLines = true;

            string filename = "Content/MarioBros.wpk";

            // Image stretch none
            textblock1 = new TextBlock()
            {
                Margin = new Thickness(20, 20, 0, 0),
                Text = "None",
                Foreground = Color.Yellow,
            };
            EntityManager.Add(textblock1.Entity);

            image1 = new Image(filename)
            {
                Margin = new Thickness(20, 70, 0, 0),
                Stretch = Stretch.None, // Default
                Width = 170,
                Height = 190,
                IsBorder = true,
                BorderColor = Color.Yellow,
            };
            EntityManager.Add(image1.Entity);

            // Image stretch Fill
            textblock2 = new TextBlock()
            {
                Margin = new Thickness(220, 20, 0, 0),
                Text = "Fill",
                Foreground = Color.Yellow,
            };
            EntityManager.Add(textblock2.Entity);

            image2 = new Image(filename)
            {
                Margin = new Thickness(220, 70, 0, 0),
                Stretch = Stretch.Fill,
                Width = 170,
                Height = 190,
                IsBorder = true,
                BorderColor = Color.Yellow,
            };
            EntityManager.Add(image2.Entity);

            // Image stretch Uniform
            textblock3 = new TextBlock()
            {
                Margin = new Thickness(20, 320, 0, 0),
                Text = "Uniform",
                Foreground = Color.Yellow,
            };
            EntityManager.Add(textblock3.Entity);

            image3 = new Image(filename)
            {
                Margin = new Thickness(20, 370, 0, 0),
                Stretch = Stretch.Uniform,
                Width = 170,
                Height = 190,
                IsBorder = true,
                BorderColor = Color.Yellow,
            };
            EntityManager.Add(image3.Entity);

            // Image stretch UniformToFill
            textblock4 = new TextBlock()
            {
                Margin = new Thickness(220, 320, 0, 0),
                Text = "UniformToFill",
                Foreground = Color.Yellow,
            };
            EntityManager.Add(textblock4.Entity);

            image4 = new Image(filename)
            {
                Margin = new Thickness(220, 370, 0, 0),
                Stretch = Stretch.UniformToFill,
                Width = 170,
                Height = 190,
                IsBorder = true,
                BorderColor = Color.Yellow,
            };
            EntityManager.Add(image4.Entity);

            // Image Check
            AddCheckImage("Content/ImageSample.wpk");
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
