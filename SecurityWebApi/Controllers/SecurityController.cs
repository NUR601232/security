using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Application.Dto;
using Security.Application.Services;

namespace SecurityWebApi.Controllers
{
    [Route("api/security")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;

        public SecurityController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto model)
        {
            var result = await _securityService.Login(model.Username, model.Password);

            if (result.Success)
            {
                return Ok(new
                {
                    jwt = result.Value,
                });
            }
            else
            {
                return Unauthorized();
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterAplicationUserModel model)
        {
            var result = await _securityService.Register(model, false, false);

            if (result.Success)
            {
                return Ok(new
                {
                    result
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> DecodeJwt()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var result = await _securityService.DecodeJwt(token);

            if (result.Success)
            {
                return Ok(new
                {
                    result
                });
            }
            else
            {
                return Unauthorized();
            }
        }
  

    }
}
