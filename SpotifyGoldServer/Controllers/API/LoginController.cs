using DAL;
using ENT.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace SpotifyGoldServer.Controllers.API {

    [Route("api/user/")]
    [ApiController]
    public class LoginController: ControllerBase {

        /// <summary>
        /// Method that registers a new user
        /// </summary>
        /// <param name="userInfo">User to be registered</param>
        /// <returns>Registerd or Not</returns>
        [HttpPost]
        [SwaggerResponse(((int) HttpStatusCode.Created), "User Registered")]
        [SwaggerResponse(((int) HttpStatusCode.BadRequest), "User already exists")]
        [Route("register")]
        public IActionResult Register([FromBody] DtoRegister userInfo) {

            userInfo.Trim();
            string? reason = userInfo.Validate();

            if (reason != null) {
                return BadRequest(reason);
            } else {
                int registered = UserHandler.RegisterUser(userInfo); // 1 - Registered, 0 - Not Registered
                return registered == 1 ? Created("DDBB", "User Registered") : BadRequest("User already exists");
            }
        }

        [HttpGet]
        [SwaggerResponse(((int) HttpStatusCode.OK), "User activated")]
        [SwaggerResponse(((int) HttpStatusCode.BadRequest), "Validations Errors")]
        [SwaggerResponse(((int) HttpStatusCode.NotFound), "User not found")]
        [Route("{user}/activate/{code}")]
        public IActionResult Activate(string user, string code) {

            HttpStatusCode status = UserHandler.ActivateUser(user, code);
            IActionResult result = status switch {
                HttpStatusCode.OK => Ok("User activated"),
                HttpStatusCode.BadRequest => BadRequest("Validations Errors"),
                HttpStatusCode.NotFound => NotFound("User not found"),
                _ => BadRequest("Unknown Error")
            };

            return result;
        }

        /// <summary>
        /// Method that validates the user information such as username, password and email
        /// </summary>
        /// <param name="userInfo">User to be validated</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(((int) HttpStatusCode.Accepted), "User valid")]
        [SwaggerResponse(((int) HttpStatusCode.BadRequest), "User invalid")]
        [Route("validate")]
        public IActionResult Validate([FromBody] DtoRegister userInfo) {

            userInfo.Trim();
            string? reason = userInfo.Validate();

            return reason == null ? Accepted("User valid") : BadRequest(reason);
        }

        /// <summary>
        /// Method that logs in a user, it will return a token if the user is valid
        /// </summary>
        /// <param name="userInfo">User to be logged in</param>
        /// <returns>Token</returns>
        [HttpPost]
        [SwaggerResponse(((int) HttpStatusCode.OK), "User logged in")]
        [SwaggerResponse(((int) HttpStatusCode.BadRequest), "User not found")]
        [Route("login")]
        public IActionResult Login([FromBody] DtoRegister userInfo) {

            userInfo.Trim();
            bool hasNotEnterUsernameOrEmail = userInfo.UsernameAndEmailEmpty();

            if (hasNotEnterUsernameOrEmail) {
                return BadRequest("Username or Email are required");
            } else {
                string? token = UserHandler.LoginUser(userInfo);
                return token != null ? Ok(token) : NotFound("User not found");
            }
        }
    }
}
