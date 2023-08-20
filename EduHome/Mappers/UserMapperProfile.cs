using AutoMapper;
using EduHome.Areas.Admin.ViewModels.UserViewModels;
using EduHome.Models.Identity;
using EduHome.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;

namespace EduHome.Mappers;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<RegisterViewModel, AppUser>().ReverseMap();
        CreateMap<AppUser, AccountDetailViewModel>().ReverseMap();

        //CreateMap<CreateUserViewModel, AppUser>()
        //        .ReverseMap();
        CreateMap<AppUser, UpdateUserViewModel>().ReverseMap();
        CreateMap<AppUser, DetailUserViewModel>().ReverseMap();
        CreateMap<IdentityRole, DetailUserViewModel>().ForMember(du => du.Role, x => x.MapFrom(ir => ir.Name.ToList())).ReverseMap();




        CreateMap<AppUser, UserViewModel>().ReverseMap();
        CreateMap<AppUser, ChangeUserViewModel>()
            .ReverseMap();

        CreateMap<AppUser, StatusUserViewModel>().ReverseMap();
    }
}