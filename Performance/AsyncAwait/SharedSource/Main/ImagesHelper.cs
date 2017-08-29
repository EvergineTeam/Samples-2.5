using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    public class ImagesHelper
    {
        private static HttpClient httpClient = new HttpClient();

        private static List<string> images = new List<string>
        {
            "https://k38.kn3.net/taringa/2/4/3/0/6/6/13/soyluuchoox/A1D.jpg?1843",
            "https://s-media-cache-ak0.pinimg.com/originals/e1/00/1e/e1001e86903d5fccba2a7e83a0547bd4.jpg",
            "https://ugc.kn3.net/i/origin/http://1.bp.blogspot.com/-kjh6bxfAaqU/T7Pjw5hs9lI/AAAAAAAAN14/7x932jj3HU0/s1600/Fondos+de+pantalla+con+bellos+rincones+de+la+naturaleza+(72).jpg",
            "https://k31.kn3.net/taringa/0/C/1/2/0/0/KonahsFvZ/F6D.jpg",
            "http://www.blogdelfotografo.com/wp-content/uploads/2015/09/paisaje-t%C3%ADpico.jpg",
            "http://k31.kn3.net/taringa/2/9/6/A/7/2/KonahsFvZ/BE9.jpg",
            "http://i.huffpost.com/gen/1701435/images/o-PAISAJES-MIAMI-facebook.jpg",
            "https://s-media-cache-ak0.pinimg.com/originals/8e/90/66/8e906688e636f2db67d8fd3da82bd6bf.jpg",
            "https://artenet.es/media/reviews/photos/original/40/b6/42/11425-nostalgia-del-ayer-paisaje-sobre-tablilla-entelada-oleo-acrilico-37-1489006882.jpg",
            "http://imagenesdepaisajespreciosos.com/wp-content/uploads/2015/11/paisajes-oto%C3%B1ales-1-e1448792804299.jpg"
        };

        public static List<string> Images { get { return images; } }

        public static ImageStreamResult LoadImageStream(string imageUrl)
        {
            return LoadImageStreamAsync(imageUrl, CancellationToken.None).GetAwaiter().GetResult();
        }

        public static async Task<ImageStreamResult> LoadImageStreamAsync(string imageUrl, CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return new ImageStreamResult(response, stream);
        }

        public class ImageStreamResult : IDisposable
        {
            private IDisposable response;

            public Stream Stream { get; private set; }

            internal ImageStreamResult(IDisposable response, Stream stream)
            {
                this.response = response;
                this.Stream = stream;
            }

            public void Dispose()
            {
                this.Stream.Dispose();
                this.response.Dispose();
            }
        }
    }
}
