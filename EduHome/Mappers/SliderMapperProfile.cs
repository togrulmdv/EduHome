using AutoMapper;
using EduHome.Areas.Admin.ViewModels.SliderViewModels;
using EduHome.Models;
using EduHome.ViewModels.BlogViewModels;
using EduHome.ViewModels.SliderViewModels;

namespace EduHome.Mappers;

public class SliderMapperProfile : Profile
{
    public SliderMapperProfile()
    {
        CreateMap<Slider, SliderViewModel>().ReverseMap();
        CreateMap<Slider, DetailSliderViewModel>().ReverseMap();
        CreateMap<CreateSliderViewModel, Slider>().ForMember(s => s.ImageName, x => x.Ignore()).ReverseMap();
        CreateMap<Slider, DeleteSliderViewModel>().ReverseMap();
        CreateMap<Slider, UpdateSliderViewModel>().ForMember(up => up.Image, x => x.Ignore()).ReverseMap();
		CreateMap<Slider, SliderCardViewModel>().ReverseMap();
	}
}
