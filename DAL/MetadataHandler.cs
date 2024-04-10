using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos;
using Newtonsoft.Json;

namespace DAL {
    internal static class MetadataHandler {

        internal static async Task<string> GetDataJson(Video video) {
            // Add the non latin characters to the json
            JsonSerializerSettings settings = new() {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };

            return JsonConvert.SerializeObject(video, Formatting.None, settings);
        }
    }
}
