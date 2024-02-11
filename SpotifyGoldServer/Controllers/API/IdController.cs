using Microsoft.AspNetCore.Mvc;
using DAL;

namespace SpotifyGoldServer.Controllers.API {
    [Route("api/[controller]")]
    [ApiController]
    public class IdController(IWebHostEnvironment env) : ControllerBase {
        private readonly string serverRoot = env.ContentRootPath;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) {
            IActionResult result = NotFound();
            string? videoPath = await VideoFunctions.DownloadVideo(serverRoot, id)!;

            // Send the video to the client, not the path
            if (videoPath != null) {
                FileStream fileStream = new FileStream(videoPath, FileMode.Open);
                result = File(fileStream, "application/octet-stream", "music.mp3");
            }

            return result;
        }
    }
}

/* Código para crear un directorio en el servidor
string directoryPath = Path.Combine(serverRoot, id);

Console.WriteLine($"Creating folder '{id}' at '{directoryPath}'");

try {
    // Check if the directory doesn't exist, then create it
    if (!Directory.Exists(directoryPath)) {
        Directory.CreateDirectory(directoryPath);
        return $"Folder '{id}' created successfully.";
    } else {
        return $"Folder '{id}' already exists.";
    }
} catch (Exception ex) {
    return $"Error creating folder: {ex.Message}";
}
*/
