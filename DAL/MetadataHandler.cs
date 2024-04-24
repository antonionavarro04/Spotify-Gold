using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos;
using Newtonsoft.Json;
using AngleSharp.Text;
using ENT.Dto;
using Microsoft.IdentityModel.Tokens;
using AngleSharp.Dom;
using YoutubeExplode.Search;

namespace DAL {
    internal static class MetadataHandler {

        private static JsonSerializerSettings settings = new() {
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
        };

        public static string GetDataJson(Video video, bool escapeAscii = true) {
            DtoMetadata dto = EntToDto(video);
            string json = escapeAscii ?
                JsonConvert.SerializeObject(dto, Formatting.None, settings) : JsonConvert.SerializeObject(dto, Formatting.Indented);

            return json;
        }

        public static string GetDataJson(VideoSearchResult searchResult) {
            //DtoMetadata dto = EntToDto(searchResult);
            string json = JsonConvert.SerializeObject(searchResult, Formatting.Indented);

            return json;
        }

        private static DtoMetadata EntToDto(Video ent) {
            List<string> urlsToSearch = new() {
                "maxresdefault",
                "sddefault",
                "hqdefault",
                "mqdefault",
                "default"
            };

            DtoMetadata dto = new() {
                Id = ent.Id.Value,
                Title = ent.Title,
                Description = ent.Description,
                UploadAt = ent.UploadDate,
                Duration = ent.Duration!,
                Author = new() {
                    Id = ent.Author.ChannelId,
                    Name = ent.Author.ChannelTitle
                },
                Engagement = new() {
                    Views = ent.Engagement.ViewCount,
                    Likes = ent.Engagement.LikeCount
                }
            };

            List<DtoThumbnail> thumbnails = new();

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

            return dto;
        }

        private static void EntToDto(VideoSearchResult dto) {

        }
    }
}
