using System.Threading.Tasks;
using GameCatcherWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameCatcherWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Welcome()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok("You are NOT authenticated");
            }

            return Ok("You are authenticated");
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return BadRequest();
            }

            var UserProfile = new UserProfile
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName ?? "",
                Email = currentUser.Email ?? "",
            };

            return Ok(UserProfile);
        }
    }
}
