using Microsoft.AspNetCore.Mvc;

namespace SpotifyGoldServer.Controllers.API {
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase {
        [HttpGet("{query}")]
        public string Get(string query) {
            return "Not yet implemented\nYour query was: " + query;
        }
    }
}
