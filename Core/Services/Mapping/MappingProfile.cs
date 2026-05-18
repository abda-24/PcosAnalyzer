using AutoMapper;
using Domain.Entities;
using Shared.DTOs.AuthDto;

namespace Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.MemberSince, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.TotalAnalyses, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.LastDiagnosis, opt => opt.Ignore());
        }
    }
}
