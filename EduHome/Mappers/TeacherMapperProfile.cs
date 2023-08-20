using AutoMapper;
using EduHome.Areas.Admin.ViewModels.TeacherViewModels;
using EduHome.Models;
using EduHome.ViewModels.TeacherViewModels;

namespace EduHome.Mappers;

public class TeacherMapperProfile : Profile
{
    public TeacherMapperProfile()
    {
        CreateMap<Teacher, TeacherCardViewModel>().ForMember(tvc => tvc.SocialMedias, x => x.MapFrom(t => t.SocialMedias)).ReverseMap();
        CreateMap<TeacherSkill, TeacherSkillViewModel>().ReverseMap();
        CreateMap<Teacher, TeacherDetailViewModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.TeacherSkills, opt => opt.MapFrom(src => src.TeacherSkills))
            .ForMember(tvc => tvc.SocialMedias, x => x.MapFrom(t => t.SocialMedias))
            .ReverseMap();

        CreateMap<Teacher, TeacherViewModel>().ReverseMap();
        CreateMap<Teacher, DeleteTeacherViewModel>().ReverseMap();
        CreateMap<UpdateTeacherViewModel, TeacherSkill>().ForMember(ts => ts.Percentage, x => x.MapFrom(ut => ut.Percentage))
            .ReverseMap();

        CreateMap<Teacher, UpdateTeacherViewModel>().ForMember(ut => ut.SkillName, x => x.MapFrom(t => t.TeacherSkills.Select(t => t.Skill.Name)))
              .ForMember(tvc => tvc.Percentage, x => x.MapFrom(t => t.TeacherSkills.Select(tp => tp.Percentage)))
            .ForMember(ut => ut.Image, x => x.Ignore())
            .ReverseMap();


        CreateMap<Teacher, DetailTeacherViewModel>().ForMember(dt => dt.Percentage, x => x.MapFrom(t => t.TeacherSkills.Select(t => t.Percentage)))
            .ForMember(dt => dt.SkillName, x => x.MapFrom(t => t.TeacherSkills.Select(t => t.Skill.Name)))
            .ReverseMap();

        CreateMap<CreateTeacherViewModel, Teacher>()
            .ReverseMap();
    }
}
