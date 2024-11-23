using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;

namespace Application;

public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<Book, BookDto>().ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src => src.Author.LastName));
        CreateMap<BookForCreationDto, Book>();
        CreateMap<BookForUpdateDto, Book>();
        
        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorForCreationDto, Author>();
        CreateMap<AuthorForUpdateDto, Author>();

        CreateMap<UserBookBorrowDto, UserBookBorrow>();
        
        CreateMap<UserForRegistrationDto, User>();
    }
}

