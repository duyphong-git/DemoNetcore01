using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Api.Data;
using Api.Entities;
using Api.Library;
using Api.Model;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Implements
{
    public class AppUsersService : BaseService, IAppUsersServices
    {
        public AppUsersService(DataContext context) : base(context)
        {
        }

        public async Task<ProcessResult<IEnumerable<AppUser>>> GetUsersAsync()
        {
            Func<Task<IEnumerable<AppUser>>> action = async () =>
            {
                var result = await context.Users.AsNoTracking().Include(p=>p.Photos).ToListAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<AppUser>> CreateOrUpdateAsync(UserModel model)
        {
            Func<Task<AppUser>> action = async () =>
            {
                model.Username = model.Username.ToLower();

                var appUserEntity = await GetOrCreateEntityAsync(context.Users, x => x.Username == model.Username);
                var user = appUserEntity.result;

                if ((!appUserEntity.isCreated && model.Id == null) || (!appUserEntity.isCreated && model.Id != user.Id))
                    throw new InvalidOperationException("Username is exist!");

                if (!appUserEntity.isCreated && model.Id != user.Id)
                {
                    throw new InvalidOperationException("User not exist!");
                }

                using var hmac = new HMACSHA512();

                user.Id = model.Id;
                user.Username = model.Username;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                user.PasswordSalt = hmac.Key;

                await context.SaveChangesAsync();

                return user;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteAsync(int id)
        {
            Func<Task> action = async () =>
            {
                var user = await context.Users.FindAsync(id);

                if(user != null)
                {
                   context.Users.Remove(user);
                   await context.SaveChangesAsync();
                }
                else
                {

                }
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<AppUser>> LoginAsync(LoginModel model)
        {
            Func<Task<AppUser>> action = async () =>
            {
                var user = await context.Users.SingleOrDefaultAsync(x => x.Username == model.Username);
                if (user == null)
                    throw new InvalidOperationException("Invalid username!");

                using var hmac = new HMACSHA512(user.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != user.PasswordHash[i])
                        throw new InvalidOperationException("Invalid password!");
                }

                return user;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<AppUser>> GetUserByIdAsync(int id)
        {
            Func<Task<AppUser>> action = async () =>
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                return user;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<AppUser>> GetUserByUsernameAsync(string username)
        {
            Func<Task<AppUser>> action = async () =>
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Username == username);
                return user;
            };

            return await Process.RunAsync(action);
        }
    }
}