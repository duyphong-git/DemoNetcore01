using System;
using Api.Entities;
using Api.Model;
using AutoMapper;

namespace Api.Library.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>();
            CreateMap<Photo, PhotoDTO>();
        }
    }
}
