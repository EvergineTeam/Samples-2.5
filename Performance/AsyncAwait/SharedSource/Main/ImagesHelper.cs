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
            "https://wave.blob.core.windows.net/samplesresouces/AsyncAwait/AAM_Japan_2015_0361.jpg",
            "https://wave.blob.core.windows.net/samplesresouces/AsyncAwait/AAM_Japan_2015_0600.jpg",
            "https://wave.blob.core.windows.net/samplesresouces/AsyncAwait/AAM_Japan_2015_1034.jpg",
            "https://wave.blob.core.windows.net/samplesresouces/AsyncAwait/AAM_Japan_2015_1134.jpg",
            "https://wave.blob.core.windows.net/samplesresouces/AsyncAwait/Paris-140.jpg",
            "https://wave.blob.core.windows.net/samplesresouces/AsyncAwait/Paris-292.jpg",
            "https://wave.blob.core.windows.net/samplesresouces/AsyncAwait/RomeAlbum-116.jpg",
            "https://wave.blob.core.windows.net/samplesresouces/AsyncAwait/RomeAlbum-118.jpg",
            "https://wave.blob.core.windows.net/samplesresouces/AsyncAwait/RomeAlbum-25.jpg",
            "https://wave.blob.core.windows.net/samplesresouces/AsyncAwait/RomeAlbum-5.jpg",
        };

        public static List<string> Images { get { return images; } }

        public static ImageStreamResult LoadImageStream(string imageUrl)
        {
            return LoadImageStreamAsync(imageUrl, CancellationToken.None).GetAwaiter().GetResult();
        }

        public static async Task<ImageStreamResult> LoadImageStreamAsync(string imageUrl, CancellationToken cancellationToken)
        {
            try
            {
                var response = await httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return new ImageStreamResult(response, stream);
            }
            catch
            {
                return new ImageStreamResult(null, null);
            }
        }

        public class ImageStreamResult : IDisposable
        {
            private HttpResponseMessage response;

            public bool IsSuccess { get; private set; }

            public Stream Stream { get; private set; }

            internal ImageStreamResult(HttpResponseMessage response, Stream stream)
            {
                this.response = response;
                this.IsSuccess = response?.IsSuccessStatusCode ?? false;
                this.Stream = stream;
            }

            public void Dispose()
            {
                this.Stream?.Dispose();
                this.response?.Dispose();
            }
        }
    }
}
