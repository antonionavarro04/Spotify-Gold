using AngleSharp.Dom;
using ENT;
using YoutubeExplode;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace DAL {
	public static class MusicFunctions {

		/// <summary>
		/// Function that creates a directory in the server
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
		/// Function that returns the index of the song based on the available strams and the quality
		/// Example: If there are 4 songs and quality is 0 index will be 0 (max quality)
		/// In the other hand if there are 5 songs and quality is 1 index will be 2 (medium quality)
		/// Last one if quality is 2 it'll return the last index (bad quality)
		/// 
		/// This is done so the user can decide whether download songs in good quality with a cost of
		/// storage or in bad quality occupying less storage
		/// </summary>
		/// <param name="availableSongs">Available songs for download</param>
		/// <param name="quality">Quality to download (0 = Best, 1 = Normal, 2 = Lowest)</param>
		/// <returns>Index of the song to be downloaded</returns>
		private static int GetQualityIndex(int availableSongs, int quality) {
			int index = 0;

			switch (quality) {
				case 1:
				index = (availableSongs - 1) / 2;
				break;

				case 2:
				index = availableSongs - 1;
				break;

				default:
				break;
			}

			return index;
		}

		/// <summary>
		/// Function that downloads a video from YouTube as mp3
		/// </summary>
		/// <param name="id">Id of the YouTube Video</param>
		/// <returns>The string path of the file</returns>
		public static async Task<ClsAudio> DownloadAudio(string id, int quality) {
			YoutubeClient youtube = new YoutubeClient();
			Video video = await youtube.Videos.GetAsync(id);
			ClsAudio audio = new();

			audio.Json = MetadataHandler.GetDataJson(video);

			// Get all available audio-only streams
			StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
			List<AudioOnlyStreamInfo> audioStreams = streamManifest.GetAudioOnlyStreams().OrderByDescending(s => s.Bitrate).ToList();
			int i = 0;

			Console.WriteLine($"Quality: {quality}");
			foreach (AudioOnlyStreamInfo info in audioStreams) {
				Console.WriteLine($"{i}: {info.Size}");
				i++;
			}

			if (audioStreams.Count != 0) {
				int index = GetQualityIndex(audioStreams.Count, quality);

				AudioOnlyStreamInfo audioStreamInfo = audioStreams[index];

				audio.Name = $"{Commons.Watermark} {video.Title}.mp3";
				audio.Stream = await youtube.Videos.Streams.GetAsync(audioStreamInfo);
			}

			return audio;
		}

		public static async void GetVideos(string query) {
			YoutubeClient youtube = new YoutubeClient();
			IAsyncEnumerable<ISearchResult> result = youtube.Search.GetResultsAsync(query);

			await foreach (ISearchResult searchResult in result) {
				Console.WriteLine(searchResult.ToString);
			}
		}

	}
}
