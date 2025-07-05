using AutoMapper;
using MasterDetails.API.DTOs;
using MasterDetails.API.Entities;

namespace MasterDetails.API.Mapping
{
    public class BlogMappingProfile : Profile
    {
        public BlogMappingProfile()
        {
            // Entity → DTO
            CreateMap<Blog, BlogDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
                .ForMember(dest => dest.TagNames, opt => opt.MapFrom(src => src.BlogTags.Select(bt => bt.Tag.Name)))
                .ForMember(dest => dest.BlogVideos, opt => opt.MapFrom(src => src.BlogVideos));

            CreateMap<BlogVideo, BlogVideoDto>().ReverseMap();

            // DTO → Entity (used manually in controller)
            CreateMap<BlogVideoDto, BlogVideo>();
        }
    }
}
