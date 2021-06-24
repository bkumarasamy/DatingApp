using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser,MemberDTO>()
               .ForMember(dest=>dest.PhotoUrl,opt=>opt.MapFrom(src=>
                           src.Photos.FirstOrDefault(x=>x.IsMain).URL))
               .ForMember(dest=>dest.Age,opt=>opt.MapFrom(src=>
                          src.DateOfBirth.CalculateAge()))
               .ForMember(dest=>dest.KnownUs,opt=>opt.MapFrom(src=>
                        src.Username.ToString()));
            CreateMap<Photo,PhotoDTO>();
        }
    }
}