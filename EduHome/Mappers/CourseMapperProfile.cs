using AutoMapper;
using EduHome.Areas.Admin.ViewModels.CourseViewModels;
using EduHome.Models;
using EduHome.ViewModels.BlogViewModels;
using EduHome.ViewModels.CourseViewModels;
//using EduHome.ViewModels.CourseViewModels;

namespace EduHome.Mappers
{
    public class CourseMapperProfile : Profile
    {
        public CourseMapperProfile()
        {
            CreateMap<Course, CourseViewModel>().ReverseMap();

            CreateMap<CreateCourseViewModel, Course>().ReverseMap();
            CreateMap<Course, DetailCourseViewModel>()
                .ForMember(cvc => cvc.Category, x => x.MapFrom(c => c.CourseCategories.Select(cvc => cvc.Category)))
                .ReverseMap();
            CreateMap<Course, UpdateCourseViewModel>().ForMember(c => c.Image, c => c.Ignore())
                .ForMember(cvc => cvc.CategoryId, x => x.MapFrom(c => c.CourseCategories.Select(cvc => cvc.CategoryId))).ReverseMap();

            CreateMap<Course, CourseCardViewModel>().ReverseMap();
            CreateMap<Course, CourseDetailViewModel>().ReverseMap();
			CreateMap<CourseCategory, CourseFilterViewModel>().ForMember(fvm => fvm.Id, x => x.MapFrom(cc => cc.CourseId))
				.ForMember(fvm => fvm.ImageName, x => x.MapFrom(cc => cc.Course.ImageName))
				.ForMember(fvm => fvm.Name, x => x.MapFrom(cc => cc.Course.Name))
				.ForMember(fvm => fvm.ShortDescription, x => x.MapFrom(cc => cc.Course.ShortDescription))
				.ReverseMap();

		}
	}
}
