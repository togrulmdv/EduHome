using AutoMapper;
using EduHome.Areas.Admin.ViewModels.SkillViewModels;
using EduHome.Models;

namespace EduHome.Mappers;

public class SkillMapperProfile : Profile
{
    public SkillMapperProfile()
    {
        CreateMap<Skill, SkillViewModel>().ReverseMap();
        CreateMap<CreateSkillViewModel, Skill>().ReverseMap();
        CreateMap<Skill, DetailSkillViewModel>().ForMember(ds => ds.TeacherName, x => x.MapFrom(s => s.TeacherSkills.Select(ts => ts.Teacher.Name))).ReverseMap();
        CreateMap<Skill, DeleteSkillViewModel>().ReverseMap();
        CreateMap<Skill, UpdateSkillViewModel>().ReverseMap();
    }
}
