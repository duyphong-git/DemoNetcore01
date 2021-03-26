using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Api.Services.Interfaces;
using Api.Model;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Collections.Generic;
using System;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IAppUsersServices _appUsersServices;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UsersController(IAppUsersServices appUsersServices, ITokenService tokenService, IMapper mapper)
        {
            _appUsersServices = appUsersServices;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetAll()
        {
            var result = await _appUsersServices.GetUsersAsync();
            if (!result.Succeed)
            {
                return BadRequest(result.Errors);
            }
            return result.Value.Any() ? Ok(_mapper.Map<IEnumerable<MemberDTO>>(result.Value)) : NoContent();
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<AppUser>> GetByIdAsync(int id)
        {
            var result = await _appUsersServices.GetUserByIdAsync(id);
            if (!result.Succeed)
            {
                return BadRequest(result.Errors);
            }
            return result.Value != null ? Ok(result.Value) : BadRequest();
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetByUsernameAsync(string username)
        {
            var result = await _appUsersServices.GetUserByUsernameAsync(username);
            if (!result.Succeed)
            {
                return BadRequest(result.Errors);
            }
            return result.Value != null ? Ok(_mapper.Map<MemberDTO>(result.Value)) : BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> CreateAsync(UserModel model)
        {
            var result = await _appUsersServices.CreateOrUpdateAsync(model);

            if (result.Succeed)
            {
                var user = new UserLoginModel { UserName = result.Value.UserName, Token = _tokenService.CreateToken(result.Value) };
                return Ok(user);
            }
            else
                return BadRequest(result.Errors);
        }

        //[HttpPut]
        //public async Task<ActionResult<AppUser>> UpdateAsync(UserModel model)
        //{
        //    var result = await _appUsersServices.CreateOrUpdateAsync(model);
        //    return result.Succeed ? Ok(result.Value) : BadRequest(result.Errors);
        //}

        [HttpPut]
        public async Task<ActionResult<AppUser>> UpdateAsync(MemberDTO dto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _appUsersServices.GetUserByUsernameAsync(username);
            var model = new UserModel();
            _mapper.Map(dto, model);
            _mapper.Map(user, model);

            var result = await _appUsersServices.CreateOrUpdateAsync(model);

            return result.Succeed ? Ok(result.Value) : BadRequest(result.Errors);
        }


        [HttpDelete]
        public async Task<ActionResult<AppUser>> DeleteAsync(UserModel model)
        {
            var result = await _appUsersServices.CreateOrUpdateAsync(model);
            return result.Succeed ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserLoginModel>> LoginAsync(LoginModel model)
        {
            var result = await _appUsersServices.LoginAsync(model);
 
            if (result.Succeed)
            {
                var user = new UserLoginModel { UserName = result.Value.UserName, Token = _tokenService.CreateToken(result.Value)};
                return Ok(user);
            }
            else
                return BadRequest(result.Errors);
        }


        [HttpGet("dto")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetAllHand()
        {
            var result = await _appUsersServices.GetMemberDOTAsync();
            if (!result.Succeed)
            {
                return BadRequest(result.Errors);
            }
            return result.Value.Any() ? Ok(result.Value) : NoContent();
        }

    }
}