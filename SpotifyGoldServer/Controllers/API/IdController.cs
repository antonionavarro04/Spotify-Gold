using Microsoft.AspNetCore.Mvc;
using DAL;

namespace SpotifyGoldServer.Controllers.API {
    [Route("api/[controller]")]
    [ApiController]
    public class IdController(IWebHostEnvironment env) : ControllerBase {
        private readonly string serverRoot = Path.Combine(env.ContentRootPath, "music");

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) {
            IActionResult result = NotFound();
            string? videoPath = await VideoFunctions.DownloadAudio(serverRoot, id)!;

            // Send the video to the client, not the path
            if (videoPath != null) {
                FileStream fileStream = new FileStream(videoPath, FileMode.Open);

                // Name will be the name of the file + [navarro41yt]
                string newFileName = $"[Spotify Gold] {Path.GetFileNameWithoutExtension(videoPath)}{Path.GetExtension(videoPath)}";
                return File(fileStream, "application/octet-stream", newFileName);
            }

            return result;
        }
    }
}
