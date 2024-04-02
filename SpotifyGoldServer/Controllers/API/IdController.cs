using Microsoft.AspNetCore.Mvc;
using DAL;
using ENT;

namespace SpotifyGoldServer.Controllers.API {

    [Route("api/[controller]")]
    [ApiController]
    public class IdController(IWebHostEnvironment env) : ControllerBase {
        private readonly string serverRoot = env.ContentRootPath;
        private readonly string musicRoot = Path.Combine(env.ContentRootPath, "music");
        private readonly string logPath = Path.Combine(env.ContentRootPath, "log", "log.txt");

        /// <summary>
        /// Function that gets the audio from the server and sends it to the client in base of a YouTube video ID
        /// </summary>
        /// <param name="id">Id of the Video</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) {
            IActionResult result = BadRequest("Error getting the audio");

            Stream? audioStream = await MusicFunctions.DownloadAudio(musicRoot, id);

            if (audioStream != null) {
                result = File(audioStream, "audio/mpeg", $"{id}.mp3");
            }

            return result;
        }
    }
}
