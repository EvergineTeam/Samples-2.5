#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Attributes;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace InternetTextureLoad
{
    [DataContract]
    public class SpriteUrl : Sprite
    {
        private object lockObject;

        private HttpWebRequest webRequest;

        [DontRenderProperty]
        public bool IsImageDownloaded
        {
            get;
            private set;
        }

        [DataMember]
        public string TextureUrl { get; set; }

        protected void LoadNewImage(string url)
        {
            try
            {
                this.webRequest = WebRequest.CreateHttp(url);
                this.webRequest.BeginGetResponse(new AsyncCallback(this.GetResponseCallback), this.webRequest);
            }
            catch (Exception)
            {
            }
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
			WaveServices.Dispatcher.RunOnWaveThread(() =>
            {
                HttpWebRequest request = asynchronousResult.AsyncState as HttpWebRequest;

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult))
                    using (var imageStream = response.GetResponseStream())
                    {
                        if (!this.disposed)
                        {
                            this.LoadTextureFromStream(imageStream);
                            this.IsImageDownloaded = true;
                        }
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private void LoadTextureFromStream(Stream imageStream)
        {
            lock (this.lockObject)
            {
                this.texture = Texture2D.FromFile(WaveServices.GraphicsDevice, imageStream);
            }

            this.UpdateSourceRectangle();
        }

        protected override void DefaultValues()
        {
            base.DefaultValues();

            this.lockObject = new object();
        }

        protected override void Initialize()
        {
            this.texturePath = TextureUrl;
            this.LoadNewImage(this.texturePath);            
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.webRequest != null)
                    {
                        this.webRequest.Abort();
                    }

                    if (!this.IsGlobalAsset && !string.IsNullOrEmpty(TextureUrl))
                    {
                        // Dispose preview texture
                        this.Assets.UnloadAsset(TextureUrl);
                        TextureUrl = null;
                    }

                    if (this.IsImageDownloaded)
                    {
                        // Destroy the downloaded texture
                        lock (this.lockObject)
                        {
                            WaveServices.GraphicsDevice.Textures.DestroyTexture(this.texture);
                        }
                    }

                    this.disposed = true;
                }
            }
        }
    }
}
