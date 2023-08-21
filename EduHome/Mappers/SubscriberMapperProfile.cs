using AutoMapper;
using EduHome.Areas.Admin.ViewModels.SubscribeViewModels;
using EduHome.Models;
using EduHome.ViewModels.HomeViewModels;

namespace EduHome.Mappers;

public class SubscriberMapperProfile : Profile
{
    public SubscriberMapperProfile()
    {
        CreateMap<SubscribeViewModel, Subscribe>().ReverseMap();
        CreateMap<Subscribe, StatusSubscribeViewModel>().ReverseMap();
    }
}