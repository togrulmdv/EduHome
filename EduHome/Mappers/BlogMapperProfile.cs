using AutoMapper;
using EduHome.Areas.Admin.ViewModels.BlogViewModels;
using EduHome.Models;
using EduHome.ViewModels.BlogViewModels;

namespace EduHome.Mappers;

public class BlogMapperProfile : Profile
{
    public BlogMapperProfile()
    {
        CreateMap<Blog, BlogViewModel>().ReverseMap();

        CreateMap<Blog, DetailBlogViewModel>().ReverseMap();
        CreateMap<Blog, UpdateBlogViewModel>().ForMember(b => b.Image, x => x.Ignore())
            .ReverseMap();
        CreateMap<Blog, CreateBlogViewModel>().ReverseMap();
        CreateMap<Blog, DeleteBlogViewModel>().ReverseMap();


        CreateMap<Blog, BlogCardViewModel>().ReverseMap();
        CreateMap<Blog, BlogPostViewModel>().ReverseMap();
        CreateMap<Blog, BlogDetailViewModel>().ReverseMap();

	}
}
