using AngleSharp.Dom;
using ENT;
using Newtonsoft.Json;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace DAL {
    public static class MusicFunctions {

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

        public static async Task<string?> GetInfo(string id) {
            string? json;
            YoutubeClient youtube = new YoutubeClient();
            Video video;

            try {
                video = await youtube.Videos.GetAsync(id);
                json = MetadataHandler.GetDataJson(video, false);
            } catch (Exception) {
                json = null;
            }

            return json;
        }

        public static async Task<string?> Search(string query, int maxResults) {
            YoutubeClient youtube = new YoutubeClient();
            List<VideoSearchResult>? results = new();

            await foreach (VideoSearchResult result in youtube.Search.GetVideosAsync(query)) {
                results.Add(result);

                if (results.Count == maxResults) {
                    break;
                }
            }
            if (results.Count == 0) {
                results = null;
            }

            return JsonConvert.SerializeObject(results, Formatting.Indented);
        }
    }
}
