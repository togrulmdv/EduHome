using AutoMapper;
using EduHome.Areas.Admin.ViewModels.SettingViewModels;
using EduHome.Models;

namespace EduHome.Mappers;

public class SettingMapperProfile : Profile
{
    public SettingMapperProfile()
    {
        CreateMap<Setting, UpdateSettingViewModel>().ReverseMap();
    }
}
