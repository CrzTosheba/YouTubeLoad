using System;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Converter;
using System.Diagnostics;
using System.IO;

namespace YouTubeLoad
{
    public static class Program
    {
        public static async Task<int> Main()
        {
            // создадим отправителя
            var sender = new Sender();

            // создадим получателя
            var receiver = new Receiver();

            // создадим команду
            var commandOne = new CommandOne(receiver);

            // инициализация команды
            sender.SetCommand(commandOne);

            //  выполнение
            sender.Run();



            var youtube = new YoutubeClient();

            /// путь к видео ///
            Console.Write("Введите ссылку: ");
            var videoId = VideoId.Parse(Console.ReadLine() ?? "");
            var video = await youtube.Videos.GetAsync(videoId);
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();// запрос потока, звук и видео, поэтому юзаю Muxed
            /// мы у ранее созданного YoutubeClient() запрашиваем информацию о видео, URL которого будет тот, что мы указали.///
            Console.WriteLine($"Название: {video.Title}");
            Console.WriteLine($"Продолжительность: {video.Duration}");
            Console.WriteLine($"Автор: {video.Author}");
            var progress = new Progress<double>();
            progress.ProgressChanged += (s, e) =>
            {
                Debug.WriteLine($"Загружено: {e:P2}");
            };

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "video1.mp4");
            Console.WriteLine(path);
            await youtube.Videos.Streams.DownloadAsync(streamInfo, path, progress);
            Console.WriteLine("Загрузка завершена");
            Console.ReadLine();
            return 0;


        }
    }
}