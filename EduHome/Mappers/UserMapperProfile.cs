using AutoMapper;
using EduHome.Models.Identity;
using EduHome.ViewModels.AccountViewModels;

namespace EduHome.Mappers;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<RegisterViewModel, AppUser>().ReverseMap();
        CreateMap<AppUser, AccountDetailViewModel>().ReverseMap();
    }
}