using Microsoft.AspNetCore.Mvc;
using DAL;
using ENT;

namespace SpotifyGoldServer.Controllers.API {

    [Route("api/[controller]")]
    [ApiController]
    public class IdController(IWebHostEnvironment env) : ControllerBase {

        [Obsolete("This field is obsolete and will no longer be used")]
        private readonly string serverRoot = env.ContentRootPath;

        [Obsolete("This field is obsolete and will no longer be used, replaced not to sending the file")]
        private readonly string musicRoot = Path.Combine(env.ContentRootPath, "music");

        [Obsolete("This field is obsolete and will no longer be used, replaced by ddbb log")]
        private readonly string logPath = Path.Combine(env.ContentRootPath, "log", "log.txt");

        /// <summary>
        /// Function that gets the audio from the server and sends it to the client in base of a YouTube video ID
        /// </summary>
        /// <param name="id">Id of the Video</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, [FromQuery(Name = "q")] int quality) {
            IActionResult result = BadRequest("Error getting the audio");

            ClsAudio audio = await MusicFunctions.DownloadAudio(id, quality);

            if (audio.Stream != null) {
                result = File(audio.Stream, "audio/mpeg", audio.Name);

                Console.WriteLine($"Sent audio '{audio.Name}'");

                string ip = $"{HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.RemotePort}";
                string message = $"Sent audio '{audio.Name}'";
                ClsLog log = new(ip, message);

                LogHandler.WriteToDDBB(log);
                Console.WriteLine(log);
            }

            return result;
        }
    }
}
