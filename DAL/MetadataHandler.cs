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
using ENT;

namespace DAL {
    internal static class MetadataHandler {

        private static JsonSerializerSettings settings = new() {
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
        };

        public static string GetDataJson(Video video) {
			return JsonConvert.SerializeObject(video, Formatting.None, settings);
        }

        public static string ClsToDto(string json) {
            DtoMetadata dto = (DtoMetadata) JsonConvert.DeserializeObject(json);

            return JsonConvert.SerializeObject(dto, Formatting.None, settings);
		}
    }
}
