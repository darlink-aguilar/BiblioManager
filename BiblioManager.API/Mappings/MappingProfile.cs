using AutoMapper;
using BiblioManager.API.DTOs.Request;
using BiblioManager.API.DTOs.Response;
using BiblioManager.Domain.Entities;
using System.Numerics;
using System.Text.RegularExpressions;

namespace BiblioManager.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Author mappings
            CreateMap<AuthorRequestDTO, Author>();
            CreateMap<Author, AuthorResponseDTO>();

            // Member mappings
            CreateMap<MemberRequestDTO, Member>();
            CreateMap<Member, MemberResponseDTO>();

            // Category mappings
            CreateMap<CategoryRequestDTO, Category>();
            CreateMap<Category, CategoryResponseDTO>();

            // Book mappings
            CreateMap<BookRequestDTO, Book>();
            CreateMap<Book, BookResponseDTO>()
                .ForMember(
                    dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}