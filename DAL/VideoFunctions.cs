using Xabe.FFmpeg;
using YoutubeExplode;
using YoutubeExplode.Videos;

namespace DAL {
    public static class VideoFunctions {
		private static readonly string FFmpegPath = @"C:\path\to\ffmpeg\directory";

		static VideoFunctions() {
			//Console.WriteLine()
		}

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
		/// <returns>The string path of the file</returns>
		public static async Task<string?> DownloadAudio(string serverRoot, string id) {
			string? directoryPath = CreateDirectory(serverRoot, id);

			if (directoryPath != null) {
				YoutubeClient youtube = new YoutubeClient();
				Video video = await youtube.Videos.GetAsync(id);

				// Sanitize the video title to remove invalid characters from the file name
				string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));

				// Get all available audio-only streams
				var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
				var audioStreams = streamManifest.GetAudioOnlyStreams().OrderByDescending(s => s.Bitrate).ToList();

				if (audioStreams.Count != 0) {
					var audioStreamInfo = audioStreams.First(); // You may want to choose a specific audio stream here based on your criteria

					using var httpClient = new HttpClient();
					Console.WriteLine("BitRate: " + audioStreamInfo.Bitrate);
					Console.WriteLine("AudioCodec: " + audioStreamInfo.AudioCodec);
					var stream = await httpClient.GetStreamAsync(audioStreamInfo.Url);

					var datetime = DateTime.Now;

					directoryPath = Path.Combine(directoryPath, $"{sanitizedTitle}.mp3");
					using var outputStream = File.Create(directoryPath);
					await stream.CopyToAsync(outputStream);

					Console.WriteLine("Download completed!");
					Console.WriteLine($"Audio saved as: {directoryPath} at {datetime}");
				} else {
					Console.WriteLine($"No suitable audio stream found for {video.Title}.");
				}
			}

			return directoryPath;
		}
	}
}
