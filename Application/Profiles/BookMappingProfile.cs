using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;

namespace Application.Profiles;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src => src.Author.LastName));
        CreateMap<BookForCreationDto, Book>();
        CreateMap<BookForUpdateDto, Book>();
    }
}
