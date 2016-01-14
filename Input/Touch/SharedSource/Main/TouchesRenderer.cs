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
using System.Runtime.Serialization;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Diagnostic;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace Touch
{
    [DataContract]
    public class TouchesRenderer : Drawable2D
    {
        private Texture2D texture;
        private Vector2 origin;

        private string texturePath;

        [DataMember]
        [RenderPropertyAsAsset(AssetType.Texture)]
        public string TexturePath
        {
            get
            {
                return texturePath;
            }

            set
            {
                if (texturePath != value)
                {
                    texturePath = value;

                    ReloadTexture();
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            ReloadTexture();
        }

        private void ReloadTexture()
        {
            if (this.Assets != null && this.texturePath != null)
            {
                texture = Assets.LoadAsset<Texture2D>(texturePath);
                origin = new Vector2(texture.Width / 2, texture.Height / 2);
            }
        }

        public override void Draw(TimeSpan gameTime)
        {
            TouchPanelState state = WaveServices.Input.TouchPanelState;      

            int index = 0;
            foreach (var touch in state)
            {
                this.layer.SpriteBatch.DrawVM(
                        texture,
                        touch.Position - origin,
                        Color.White);

                Labels.Add("Touch" + index++, touch.Position);
            }
        }

        protected override void Dispose(bool disposing)
        {
            //if (texturePath != null)
            //{
            //    Assets.UnloadAsset(texturePath);
            //}
        }
    }
}
