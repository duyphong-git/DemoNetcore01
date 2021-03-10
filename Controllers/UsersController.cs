using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Api.Services.Interfaces;
using Api.Model;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly DataContext _context;
        private readonly IAppUsersServices _appUsersServices;
        private readonly ITokenService _tokenService;

        public UsersController(IAppUsersServices appUsersServices, DataContext context, ITokenService tokenService)
        {
            _appUsersServices = appUsersServices;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _appUsersServices.GetUsersAsync();
            if (!result.Succeed)
            {
                return BadRequest(result.Errors);
            }
            return result.Value.Any() ? Ok(result.Value) : NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> CreateAsync(UserModel model)
        {
            var result = await _appUsersServices.CreateOrUpdateAsync(model);
            return result.Succeed ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPut]
        public async Task<ActionResult<AppUser>> UpdateAsync(UserModel model)
        {
            var result = await _appUsersServices.CreateOrUpdateAsync(model);
            return result.Succeed ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpDelete]
        public async Task<ActionResult<AppUser>> DeleteAsync(UserModel model)
        {
            var result = await _appUsersServices.CreateOrUpdateAsync(model);
            return result.Succeed ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginModel>> LoginAsync(LoginModel model)
        {
            var result = await _appUsersServices.LoginAsync(model);
 
            if (result.Succeed)
            {
                var user = new UserLoginModel { Username = result.Value.Username, Token = _tokenService.CreateToken(result.Value)};
                return Ok(user);
            }
            else
                return BadRequest(result.Errors);
        }
    }
}