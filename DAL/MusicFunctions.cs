using System.ComponentModel;
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
        /// <param name="serverRoot"></param>
        /// <param name="id"></param>
        /// <returns>The string path of the file</returns>
        public static async Task<Stream?> DownloadAudio(string serverRoot, string id) {
            YoutubeClient youtube = new YoutubeClient();
            Video video = await youtube.Videos.GetAsync(id);
            Stream? audio = null;

            // Sanitize the video title to remove invalid characters from the file name
            string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));

            // Get all available audio-only streams
            StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            List<AudioOnlyStreamInfo> audioStreams = streamManifest.GetAudioOnlyStreams().OrderByDescending(s => s.Bitrate).ToList();

            foreach (AudioOnlyStreamInfo audioStream in audioStreams) {
                Console.WriteLine($"Audio Stream: {audioStream.Bitrate}bps, {audioStream.Container.Name}, Size: {audioStream.Size}");
            }

            if (audioStreams.Count != 0) {
                AudioOnlyStreamInfo audioStreamInfo = audioStreams.First(); // You may want to choose a specific audio stream here based on your criteria

                audio = await youtube.Videos.Streams.GetAsync(audioStreamInfo);
            }

            return audio;
        }

    }
}
