using AutoMapper;
using EduHome.Areas.Admin.ViewModels.SpeakerViewModels;
using EduHome.Models;

namespace EduHome.Mappers;

public class SpeakerMapperProfile : Profile
{
    public SpeakerMapperProfile()
    {
        CreateMap<Speaker, SpeakerViewModel>().ReverseMap();
        CreateMap<Speaker, DetailSpeakerViewModel>().ForMember(evc => evc.EventName, x => x.MapFrom(s => s.EventSpeakers.Select(evc => evc.Event.Name))).ReverseMap();
        CreateMap<CreateSpeakerViewModel, Speaker>().ReverseMap();
        CreateMap<Speaker, UpdateSpeakerViewModel>()
            .ForMember(svc => svc.EventId, x => x.MapFrom(s => s.EventSpeakers.Select(svc => svc.EventId)))
            .ForMember(s => s.Image, x => x.Ignore())
            .ReverseMap();
    }
}
