using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT {
    public static class Commons {
        public static string Vendor { get; } = "Spotify Gold";

        public static string Version { get; } = "1.0.0";

        public static string Watermark { get; } = $"[{Vendor}]";
    }
}
