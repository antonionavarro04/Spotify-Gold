namespace SpotifyGoldServer.Models {
    internal static class LoggingFunctions {
        public static string GetIpPlusPort(HttpContext httpContext) {
            return $"{httpContext.Connection.RemoteIpAddress}:{httpContext.Connection.RemotePort}";
        }
    }
}
