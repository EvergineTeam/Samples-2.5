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
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace TouchProject
{
    public class TouchesRenderer : Drawable2D
    {
        private Texture2D texture;
        private string pathTexture;
        private Vector2 origin;

        public TouchesRenderer(string pathTexture)
            : base("TouchesRenderer", DefaultLayers.GUI)
        {
            this.pathTexture = pathTexture;
        }

        protected override void Initialize()
        {
            base.Initialize();

            texture = Assets.LoadAsset<Texture2D>(pathTexture);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        protected override void DrawBasicUnit(int parameter)
        {
            TouchPanelState state = WaveServices.Input.TouchPanelState;

            foreach (var Touch in state)
            {
                spriteBatch.Draw(
                        texture,
                        Touch.Position - origin,
                        Color.White);
            }
        }

        protected override void Dispose(bool disposing)
        {
            Assets.UnloadAsset(pathTexture);
        }
    }
}
