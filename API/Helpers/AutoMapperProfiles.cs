using System;
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
                        src.UserName.ToString()));
            CreateMap<Photo,PhotoDTO>();

            CreateMap<MemberUpdateDto,AppUser>();
            CreateMap<RegisterDto,AppUser>();
            CreateMap<Message,MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl,opt => opt.MapFrom(src => 
                        src.Sender.Photos.FirstOrDefault(x => x.IsMain).URL))
                .ForMember(dest => dest.RecipientPhotoUrl,opt => opt.MapFrom(src => 
                        src.Recipient.Photos.FirstOrDefault(x => x.IsMain).URL));

            CreateMap<DateTime,DateTime>().ConvertUsing(d =>DateTime.SpecifyKind(d,DateTimeKind.Utc));
    
        }
    }
}