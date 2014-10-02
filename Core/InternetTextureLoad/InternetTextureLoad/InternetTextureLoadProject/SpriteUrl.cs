#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services; 
#endregion

namespace InternetTextureLoadProject
{
    public class SpriteUrl : Sprite
    {
        private readonly object lockObject = new object();

        private HttpWebRequest webRequest;

        private string previewUri;

        private string textureUrl;

        public bool IsImageDownloaded
        {
            get;
            private set;
        }

        public SpriteUrl(string textureUrl)
            : this(null, textureUrl)
        {
        }

        public SpriteUrl(string previewTexturePath, string textureUrl)
            : base(previewTexturePath)
        {
            if (!this.CheckValidUrl(textureUrl))
            {
                throw new ArgumentException("The textureUrl parameter is not a valid HTTP or HTTPS URL");
            }

            this.previewUri = previewTexturePath;
            this.textureUrl = textureUrl;
        }

        protected override void Initialize()
        {
            if (!string.IsNullOrEmpty(this.previewUri))
            {
                base.Initialize();
            }

            this.texturePath = this.textureUrl;
            this.LoadNewImage(this.texturePath);
        }

        protected void LoadNewImage(string url)
        {
            try
            {
                this.webRequest = (HttpWebRequest)WebRequest.Create(url);
                this.webRequest.BeginGetResponse(new AsyncCallback(this.GetResponseCallback), this.webRequest);
            }
            catch (Exception)
            {
            }
        }

        private bool CheckValidUrl(string uriName)
        {
            bool result = false;
            Uri uriResult;

            if (Uri.TryCreate(uriName, UriKind.Absolute, out uriResult))
            {
#if METRO
                result = uriResult.Scheme == "http" || uriResult.Scheme == "https";
#else
                result = uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
#endif
            }
            
            return result;
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            WaveServices.TaskScheduler.CreateTask(() =>
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

                    if (!this.IsGlobalAsset && !string.IsNullOrEmpty(this.previewUri))
                    {
                        // Dispose preview texture
                        this.Assets.UnloadAsset(this.previewUri);
                        this.previewUri = null;
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
