using ENT;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace DAL {
    public static class MusicFunctions {

        /// <summary>
        /// Fucntion that creates a directory in the server
        /// </summary>
        /// <param name="serverRoot">Root folder of the server</param>
        /// <param name="id">Name of the folder</param>
        /// <returns>The string path of the created folder, null if it cannot be created or exists</returns>
        [Obsolete("This method is obsolete and will no longer be used", true)]
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

        [Obsolete("This method is obsolete and will no longer be used", true)]
        public static void DeleteDirectory(string directoryPath, int wait) {
            try {
                Thread.Sleep(wait);
                Directory.Delete(directoryPath, true);
            } catch (Exception ex) {
                Console.WriteLine($"Error deleting folder: {ex.Message}");
            }
        }

        /// <summary>
        /// Function that downloads a video from YouTube as mp3
        /// </summary>
        /// <param name="id">Id of the YouTube Video</param>
        /// <returns>The string path of the file</returns>
        public static async Task<ClsAudio> DownloadAudio(string id) {
            YoutubeClient youtube = new YoutubeClient();
            Video video = await youtube.Videos.GetAsync(id);
            ClsAudio audio = new();

            // Get all available audio-only streams
            StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            List<AudioOnlyStreamInfo> audioStreams = streamManifest.GetAudioOnlyStreams().OrderByDescending(s => s.Bitrate).ToList();

            if (audioStreams.Count != 0) {
                AudioOnlyStreamInfo audioStreamInfo = audioStreams.First();

                audio.Name = $"{Commons.Watermark} {video.Title}.mp3";
                audio.Stream = await youtube.Videos.Streams.GetAsync(audioStreamInfo);
            }

            return audio;
        }

    }
}
