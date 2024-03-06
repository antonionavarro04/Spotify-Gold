using SpotifyGoldServer.Models;
using Microsoft.AspNetCore.Mvc;
using DAL;

namespace SpotifyGoldServer.Controllers.API {

    [Route("api/[controller]")]
    [ApiController]
    public class IdController(IWebHostEnvironment env) : ControllerBase {
        private readonly string serverRoot = Path.Combine(env.ContentRootPath, "music");

        /// <summary>
        /// Function that gets the audio from the server and sends it to the client in base of a YouTube video ID
        /// </summary>
        /// <param name="id">Id of the Video</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) {
            IActionResult result = NotFound();
            string? videoPath = await VideoFunctions.DownloadAudio(serverRoot, id)!;

            // Send the video to the client, not the path
            if (videoPath != null) {
				FileStream fileStream = new FileStream(videoPath, FileMode.Open);

                // Name will be the name of the file + [navarro41yt]
                string newFileName = $"[Spotify Gold] {Path.GetFileNameWithoutExtension(videoPath)}{Path.GetExtension(videoPath)}";
                result = File(fileStream, "application/octet-stream", newFileName);

                // Delete the directory
				// VideoFunctions.DeleteDirectory(Path.GetDirectoryName(videoPath)!);
            }

            return result;
        }
    }
}
