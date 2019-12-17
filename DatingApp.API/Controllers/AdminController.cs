using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public AdminController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await _dataContext.Users.OrderBy(x => x.UserName).Select(user => new {
                Id = user.Id,
                UserName = user.UserName,
                Roles = (from userRole in user.UserRoles
                        join role in _dataContext.Roles on userRole.RoleId equals role.Id
                        select role.Name).ToList()
            }).ToListAsync();
            return Ok(userList);
        }
        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photosForModeration")]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("ADmins or moderators can see this");
        }
    }
}