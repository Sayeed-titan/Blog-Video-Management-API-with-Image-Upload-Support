using AutoMapper;
using MasterDetails.API.DTOs;
using MasterDetails.API.Entities;

namespace MasterDetails.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Blog, BlogDto>().ReverseMap();
            CreateMap<BlogVideo, BlogVideoDto>().ReverseMap();
        }
    }
}
