using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos;
using Newtonsoft.Json;
using YoutubeExplode.Search;
using ENT.Dto.Metadata;
using ENT.Dto.Result;
using ENT;
using System.Net.Http;
using Newtonsoft.Json.Serialization;

namespace DAL {
    internal static class MetadataHandler {

        private static JsonSerializerSettings settings = new() {
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
        };

        private static JsonSerializerSettings settings2 = new() {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string GetDataJson(Video video, bool escapeAscii = true) {
            DtoMetadataResponse dto = EntToDto(video);
            string json = escapeAscii ?
                JsonConvert.SerializeObject(dto, Formatting.None, settings) : JsonConvert.SerializeObject(dto, Formatting.Indented, settings2);

            return json;
        }

        public static string GetDataJson(VideoSearchResult searchResult) {
            //DtoMetadata dto = EntToDto(searchResult);
            string json = JsonConvert.SerializeObject(searchResult, Formatting.Indented);

            return json;
        }

        private static DtoMetadataResponse EntToDto(Video ent) {
            List<string> urlsToSearch = new() {
                "maxresdefault",
                "sddefault",
                "hqdefault",
                "mqdefault",
                "default"
            };

            DtoMetadataResponse dto = new() {
                Id = ent.Id.Value,
                Title = ent.Title,
                Description = ent.Description,
                UploadAt = ent.UploadDate.ToString(),
                Author = new() {
                    Id = ent.Author.ChannelId,
                    Name = ent.Author.ChannelTitle
                },
                Engagement = new() {
                    Views = ent.Engagement.ViewCount,
                    Likes = ent.Engagement.LikeCount
                }
            };

            List<DtoThumbnailResponse> thumbnails = new();

            List<Thumbnail> thumbs = ent.Thumbnails.ToList<Thumbnail>();

            foreach (Thumbnail thumb in thumbs) {
                string url = thumb.Url;

                foreach (string urlToSearch in urlsToSearch) {
                    if (url.EndsWith($"{urlToSearch}.jpg") || url.EndsWith($"{urlToSearch}.webp")) {
                        thumbnails.Add(new() {
                            Url = url,
                            Size = thumb.Resolution.Area
                        });
                        urlsToSearch.Remove(urlToSearch);
                        break;
                    }
                }

                if (urlsToSearch.Count == 0) {
                    break;
                }
            }

            dto.Thumbnails = thumbnails;

            long durationLong = (long) ent.Duration!.Value.TotalSeconds;
            dto.Duration = durationLong;

            return dto;
        }

        public static async Task<DtoResultResponse> EntToDto(VideoSearchResult result) {
            ClsEngagement engagement = await GetEngagement(result.Id) ?? new ClsEngagement();

            TimeSpan duration = result.Duration ?? TimeSpan.Zero;

            var thumbs = result.Thumbnails.OrderByDescending(t => t.Resolution.Area).ToList();

            var dto = new DtoResultResponse {
                Id = result.Id,
                Title = result.Title,
                AuthorName = result.Author.ChannelTitle,
                Duration = (long) duration.TotalSeconds,
                Thumbnail = thumbs.LastOrDefault()?.Url,
                Views = engagement.Views,
                Likes = engagement.Likes
            };

            return dto;
        }

        public static async Task<ClsEngagement?> GetEngagement(string id) {
            using (var client = new HttpClient()) {
                string apiUrl = $"https://www.googleapis.com/youtube/v3/videos?part=statistics&id={id}&key={ClsConnection.YOUTUBE_API_KEY}";

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode) {
                    var responseData = await response.Content.ReadAsAsync<dynamic>();
                    var statistics = responseData["items"][0]["statistics"];

                    var engagement = new ClsEngagement {
                        Likes = Convert.ToInt64(statistics["likeCount"]),
                        Views = Convert.ToInt64(statistics["viewCount"])
                    };

                    return engagement;
                } else {
                    // Handle error
                    Console.WriteLine($"Failed to retrieve data from Youtube API. Status code: {response.StatusCode}");
                    return null;
                }
            }
        }
    }
}
