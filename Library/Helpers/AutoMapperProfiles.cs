using System;
using System.Linq;
using Api.Entities;
using Api.Model;
using AutoMapper;

namespace Api.Library.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberDTO, AppUser>();
            CreateMap<MemberDTO, UserModel>();
            CreateMap<UserModel, AppUser>();
        }
    }
}
