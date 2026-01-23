using BookAdvisor.Application.DTOs.Auth;
using BookAdvisor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookAdvisor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Kayıt olma işlemi
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _identityService.RegisterAsync(request);

            if (!result.Success)
            {
                return BadRequest(new AuthResponse(UserId: null, Token: null, Success: false, ErrorMessage: result.ErrorMessage));
            }
            return Ok(new AuthResponse(UserId: result.UserId, Token: null, Success: true, ErrorMessage: null));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _identityService.LoginAsync(request);
            if (!result.Success)
            {
                return BadRequest(new AuthResponse(ErrorMessage: result.ErrorMessage, UserId: null, Token: null, Success: false));
            }
            return Ok(new AuthResponse(UserId: result.UserId, Token: result.Token, Success: true, ErrorMessage: null));
        }

    }
}
