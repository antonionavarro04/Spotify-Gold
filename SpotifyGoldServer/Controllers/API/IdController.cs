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
            IActionResult result = NotFound();
            string? videoPath = await MusicFunctions.DownloadAudio(musicRoot, id)!;

            // Send the video to the client, not the path
            if (videoPath != null) {
				FileStream fileStream = new FileStream(videoPath, FileMode.Open);

                // Name will be the name of the file + [navarro41yt]
                string newFileName = $"[Spotify Gold] {Path.GetFileName(videoPath)}";
                result = File(fileStream, "application/octet-stream", newFileName);

                string ipClient = HttpContext.Connection.RemoteIpAddress.ToString();

                LogHandler.WriteToDDBB(new ClsLog(ipClient, $"Sent file '{newFileName}'"));
            }

            return result;
        }
    }
}
