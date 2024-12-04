using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;

namespace Application.Profiles;

public class AuthorMappingProfile : Profile
{
    public AuthorMappingProfile()
    {
        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorForCreationDto, Author>();
        CreateMap<AuthorForUpdateDto, Author>();
    }
}
