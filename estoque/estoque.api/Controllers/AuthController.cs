using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoogleOAuthExample.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Inicia o processo de autenticação com Google
        /// </summary>
        [HttpGet("login")]
        public IActionResult Login()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback))
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Endpoint de callback após autenticação com Google
        /// </summary>
        [HttpGet("callback")]
        public async Task<IActionResult> Callback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result.Succeeded)
            {
                var claims = result.Principal?.Claims.Select(c => new { c.Type, c.Value }).ToList();

                return Ok(new
                {
                    Message = "Autenticação bem-sucedida!",
                    User = new
                    {
                        Name = result.Principal?.FindFirst(ClaimTypes.Name)?.Value,
                        Email = result.Principal?.FindFirst(ClaimTypes.Email)?.Value,
                        Id = result.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    },
                    Claims = claims
                });
            }

            return Unauthorized(new { Message = "Falha na autenticação" });
        }

        /// <summary>
        /// Realiza logout do usuário
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Logout realizado com sucesso!" });
        }

        /// <summary>
        /// Verifica o status de autenticação atual
        /// </summary>
        [HttpGet("status")]
        public IActionResult Status()
        {
            var user = HttpContext.User;

            return Ok(new
            {
                IsAuthenticated = user.Identity?.IsAuthenticated ?? false,
                User = user.Identity?.IsAuthenticated == true ? new
                {
                    Name = user.FindFirst(ClaimTypes.Name)?.Value,
                    Email = user.FindFirst(ClaimTypes.Email)?.Value,
                    Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                } : null
            });
        }

        /// <summary>
        /// Endpoint protegido que requer autenticação - retorna informações do usuário
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        public IActionResult Profile()
        {
            var user = HttpContext.User;

            return Ok(new
            {
                Message = "Acesso autorizado!",
                User = new
                {
                    Name = user.FindFirst(ClaimTypes.Name)?.Value,
                    Email = user.FindFirst(ClaimTypes.Email)?.Value,
                    Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    IsAuthenticated = user.Identity?.IsAuthenticated ?? false
                }
            });
        }

        /// <summary>
        /// Retorna os tokens do Google (se SaveTokens estiver habilitado)
        /// </summary>
        [HttpGet("tokens")]
        [Authorize]
        public async Task<IActionResult> GetTokens()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IdToken = idToken
            });
        }
    }
}