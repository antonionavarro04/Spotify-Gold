using ENT.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace SpotifyGoldServer.Controllers.API {
    [Route("api/")]
    [ApiController]
    public class LoginController: ControllerBase {

        ///
        /// Method that registers a user in the database

        /// <summary>
        /// Method that validates the user information such as username, password and email
        /// </summary>
        /// <param name="userInfo">User to be validated</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(((int)HttpStatusCode.Accepted), "User valid")]
        [SwaggerResponse(((int)HttpStatusCode.BadRequest), "User invalid")]
        [Route("register")]
        public IActionResult Validate([FromBody] DtoUserInfoRequest userInfo) {

            userInfo.Trim();
            string? reason = userInfo.Validate();

            return reason == null ? Accepted("User valid") : BadRequest(reason);
        }
    }
}
