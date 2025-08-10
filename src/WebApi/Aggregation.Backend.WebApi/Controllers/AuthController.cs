using Aggregation.Backend.Domain.Constants;
using Aggregation.Backend.Domain.Dtos.Auth;
using Aggregation.Backend.Infrastructure.Cache;
using Aggregation.Backend.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aggregation.Backend.WebApi.Controllers
{
    [ApiController]
    [Route(ApiEndpoints.Authenticate)]
    public class AuthController : ControllerBase
    {
        private readonly LoginStore _store;
        private readonly TokenGenerator _tokenGenerator;

        public AuthController(LoginStore store, TokenGenerator tokenGenerator)
        {
            _store = store;
            _tokenGenerator = tokenGenerator;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <remarks>
        /// For Authentication demonstration purposes there is an in memory login store to authenticate users.
        /// The store has 10 generated user-password pairs following the pattern <c>{"username":"user{i}","password":"password{i}"} </c> where <c>i</c> in range [0,9] so you can try any of those.
        /// </remarks>
        /// <param name="loginDto">The login credentials (username and password).</param>
        /// <returns>A JWT token if authentication is successful; otherwise, 401 Unauthorized.</returns>
        /// <response code="200">Returns the JWT token</response>
        /// <response code="401">If the credentials are invalid</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult Auth([FromBody] LoginDto loginDto)
        {
            var isValid = _store.ValidateUsersPassword(loginDto.Username, loginDto.Password);
            if (!isValid)
            {
                return Unauthorized();
            }

            var token = _tokenGenerator.GenerateJwtToken(loginDto.Username);
            return Ok(token);
        }
    }
}