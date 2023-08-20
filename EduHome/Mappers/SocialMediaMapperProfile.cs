using AutoMapper;
using EduHome.Areas.Admin.ViewModels.SocialMediaViewModels;
using EduHome.Models;

namespace EduHome.Mappers;

public class SocialMediaMapperProfile : Profile
{
    public SocialMediaMapperProfile()
    {
        CreateMap<SocialMedia, SocialMediaViewModel>().ReverseMap();
        CreateMap<SocialMedia, DeleteSocialMediaViewModel>().ReverseMap();

        CreateMap<CreateSocialMediaViewModel, SocialMedia>().ReverseMap();
        CreateMap<SocialMedia, DetailSocialMediaViewModel>().ForMember(dsc => dsc.TeacherName, x => x.MapFrom(s => s.Teacher.Name))
            .ReverseMap();
        CreateMap<SocialMedia, UpdateSocialMediaViewModel>().ForMember(svc => svc.TeacherId, x => x.MapFrom(t => t.TeacherId))
            .ReverseMap();


    }
}