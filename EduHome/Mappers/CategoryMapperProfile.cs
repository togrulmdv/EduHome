using AutoMapper;
using EduHome.Areas.Admin.ViewModels.CategoryViewModels;
using EduHome.Models;
using EduHome.ViewModels.CategoryViewModels;

namespace EduHome.Mappers;

public class CategoryMapperProfile : Profile
{
    public CategoryMapperProfile()
    {
        CreateMap<Category, CategoryViewModel>().ReverseMap();
        CreateMap<Category, DeleteCategoryViewModel>().ReverseMap();
        CreateMap<CreateCategoryViewModel, Category>().ReverseMap();
        CreateMap<Category, DetailCategoryViewModel>().ForMember(cvc => cvc.Courses, x => x.MapFrom(c => c.CourseCategories.Select(cvc => cvc.Course.Name))).ReverseMap();
        CreateMap<Category, UpdateCategoryViewModel>().ForMember(cvc => cvc.CourseId, x => x.MapFrom(c => c.CourseCategories.Select(cvc => cvc.CourseId)))
            .ReverseMap();
    }
}
