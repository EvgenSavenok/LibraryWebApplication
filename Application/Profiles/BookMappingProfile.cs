using Application.DataTransferObjects;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src => src.Author.LastName))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
            .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        CreateMap<BookForCreationDto, Book>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.BookTitle))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
            .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId));
        CreateMap<BookForUpdateDto, Book>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.BookTitle))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
            .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId));
        
        CreateMap<BookDto, BookForUpdateDto>()
            .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.BookTitle))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.AuthorName))
            .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src => src.AuthorLastName))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));
    }
}
