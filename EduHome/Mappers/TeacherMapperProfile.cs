using AutoMapper;
using EduHome.Models;
using EduHome.ViewModels.TeacherViewModels;

namespace EduHome.Mappers;

public class TeacherMapperProfile : Profile
{
    public TeacherMapperProfile()
    {
        CreateMap<Teacher, TeacherCardViewModel>().ForMember(tvc => tvc.SocialMedias, x => x.MapFrom(t => t.SocialMedias)).ReverseMap();
        CreateMap<Teacher, TeacherDetailViewModel>()
            .ForMember(tvc => tvc.SkillNames, x => x.MapFrom(t => t.TeacherSkills.Select(tp => tp.Skill.Name)))
            .ForMember(tvc => tvc.Percentage, x => x.MapFrom(t => t.TeacherSkills.Select(tp => tp.Percentage)))
			.ForMember(tvc => tvc.SocialMedias, x => x.MapFrom(t => t.SocialMedias))
			.ReverseMap();
        //CreateMap<Teacher, AdminTeacherViewModel>().ReverseMap();
        //CreateMap<CreateTeacherViewModel, Teacher>().ReverseMap();




    }

}
