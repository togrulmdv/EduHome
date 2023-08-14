using AutoMapper;
using EduHome.Models;
using EduHome.ViewModels.HomeViewModels;

namespace EduHome.Mappers;

public class SubscriberMapperProfile : Profile
{
    public SubscriberMapperProfile()
    {
        CreateMap<SubscribeViewModel, Subscribe>().ReverseMap();
    }
}