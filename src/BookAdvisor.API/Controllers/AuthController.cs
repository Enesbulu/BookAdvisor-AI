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
        public async Task<IActionResult> Register([FromBody] CustomRegisterRequest request)
        {
            var result = await _identityService.RegisterAsync(request.Email, request.Password, firstName: request.FirstName, lastName: request.LastName);

            if (!result.Success)
            {
                return BadRequest(new { Error = result.ErrorMessage });
            }
            return Ok(new { UserId = result.UserId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CustomLoginRequest request)
        {
            var result = await _identityService.LoginAsync(request.Email, request.Password);
            if (!result.Success)
            {
                return BadRequest(new { Error = result.ErrorMessage });
            }
            return Ok(new { Token = result.Token });
        }

    }
    // DTO (Data Transfer Objects) Tanımları
    // Sınıf isimlerini çakışma olmasın diye "Custom" ile başlattım veya direkt buraya koydum.
    public class CustomRegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CustomLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
