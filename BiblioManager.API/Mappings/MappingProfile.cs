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
        }
    }
}