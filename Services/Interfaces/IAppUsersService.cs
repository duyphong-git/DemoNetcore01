using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Entities;
using Api.Library;
using Api.Model;

namespace Api.Services.Interfaces
{
    public interface IAppUsersServices
    {
        Task<ProcessResult<IEnumerable<AppUser>>> GetUsersAsync();
        Task<ProcessResult<AppUser>> CreateOrUpdateAsync(UserModel model);
        Task<ProcessResult> DeleteAsync(int id);
        Task<ProcessResult<AppUser>> LoginAsync(LoginModel model);
        Task<ProcessResult<AppUser>> GetUserByIdAsync(int id);
        Task<ProcessResult<AppUser>> GetUserByUsernameAsync(string username);
        Task<ProcessResult<IEnumerable<MemberDTO>>> GetMemberDOTAsync();
    }
}
