using System;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Converter;
using System.Diagnostics;

namespace YouTubeLoad
{
    public static class Program
    {
        public static async Task<int> Main()
        {

            var youtube = new YoutubeClient();

            /// путь к видео ///
           // Console.Write("Введите ссылку: ");
           // var videoId = VideoId.Parse(Console.ReadLine() ?? "");
            
      
            var video = await youtube.Videos.GetAsync("https://www.youtube.com/watch?v=4zQdusAXmNs");
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync("https://www.youtube.com/watch?v=4zQdusAXmNs");
            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();// запрос потока, звук и видео, поэтому юзаю Muxed
            /// мы у ранее созданного YoutubeClient() запрашиваем информацию о видео, URL которого будет тот, что мы указали.///
            Console.WriteLine($"Название: {video.Title}");
            Console.WriteLine($"Продолжительность: {video.Duration}");
            Console.WriteLine($"Автор: {video.Author}");
            var progress = new Progress<double>();
            progress.ProgressChanged += (s, e) => Debug.WriteLine($"Загружено: {e:P2}");

            await youtube.Videos.Streams.DownloadAsync(streamInfo, $"E:/Education/Video.{streamInfo.Container}", progress);
            Console.ReadLine();
            return 0;
            

        }
    }
}
