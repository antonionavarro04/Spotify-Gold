using YoutubeExplode;
using YoutubeExplode.Converter;

namespace DAL {
    public static class VideoFunctions {
        /// <summary>
        /// Fucntion that creates a directory in the server
        /// </summary>
        /// <param name="serverRoot">Root folder of the server</param>
        /// <param name="id">Name of the folder</param>
        /// <returns>The string path of the created folder, null if it cannot be created or exists</returns>
        private static string? CreateDirectory(string serverRoot, string id) {
            string? directoryPath = Path.Combine(serverRoot, id);
            
            Console.WriteLine($"Creating folder '{id}' at '{directoryPath}'");
            
            try {
                // Check if the directory doesn't exist, then create it
                if (!Directory.Exists(directoryPath)) {
                    Directory.CreateDirectory(directoryPath);
                } else {
                    directoryPath = null;
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error creating folder: {ex.Message}");
                directoryPath = null;
            }

            return directoryPath;
        }

        /// <summary>
        /// Function that downloads a video from YouTube as mp3
        /// </summary>
        /// <param name="serverRoot"></param>
        /// <param name="id"></param>
        /// <returns>The music.mp3 file</returns>
        public static async Task<string?> DownloadVideo(string serverRoot, string id) {
            string? directoryPath = CreateDirectory(serverRoot, id);

            if (directoryPath != null) {
                var youtube = new YoutubeClient();
                var video = await youtube.Videos.GetAsync(id);

                // Sanitize the video title to remove invalid characters from the file name
                string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));

                // Get all available muxed streams
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
                var muxedStreams = streamManifest.GetMuxedStreams().OrderByDescending(s => s.VideoQuality).ToList();

                if (muxedStreams.Count != 0) {
                    var streamInfo = muxedStreams.First();
                    using var httpClient = new HttpClient();
                    Console.WriteLine("BitRate: " + streamInfo.Bitrate);
                    Console.WriteLine("AudioCodec: " + streamInfo.AudioCodec);
                    var stream = await httpClient.GetStreamAsync(streamInfo.Url);
                    var datetime = DateTime.Now;

                    directoryPath = Path.Combine(directoryPath, $"{sanitizedTitle}.{streamInfo.Container}");
                    using var outputStream = File.Create(directoryPath);
                    await stream.CopyToAsync(outputStream);

                    Console.WriteLine("Download completed!");
                    Console.WriteLine($"Video saved as: {directoryPath} at {datetime}");
                } else {
                    Console.WriteLine($"No suitable video stream found for {video.Title}.");
                }
            }

            return directoryPath;
        }
    }
}
