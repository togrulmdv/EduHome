using AutoMapper;
using EduHome.Areas.Admin.ViewModels.EventViewModels;
using EduHome.Models;
using EduHome.ViewModels.BlogViewModels;
using EduHome.ViewModels.EventViewModels;

namespace EduHome.Mappers
{
    public class EventMapperProfile : Profile
    {
        public EventMapperProfile()
        {
            ////CreateMap<Event, EventViewModel>().ReverseMap();
            //CreateMap<Event, DetailEventViewModel>()
            //    .ForMember(evc => evc.Speakers, x => x.MapFrom(e => e.EventSpeakers.Select(es => es.Speaker)))
            //    .ReverseMap();
            ////CreateMap<Event, EventViewModel>().ReverseMap();
            //CreateMap<CreateEventViewModel, Event>().ReverseMap();
            //CreateMap<Event, DetailEventViewModel>().ForMember(evc => evc.Speakers, x => x.MapFrom(e => e.EventSpeakers.Select(s => s.Speaker.Name))).ReverseMap();
            //CreateMap<Event, UpdateEventViewModel>().ForMember(evc => evc.SpeakerId, x => x.MapFrom(e => e.EventSpeakers.Select(s => s.SpeakerId)))
            //    .ForMember(evc => evc.Image, e => e.Ignore())

            //    .ReverseMap();

			CreateMap<Event, EventCardViewModel>().ReverseMap();
            CreateMap<Event, EventDetailViewModel>().ReverseMap();

		}
	}
}
