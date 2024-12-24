using Application.DataTransferObjects;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles;

public class BorrowMappingProfile : Profile
{
    public BorrowMappingProfile()
    {
        CreateMap<UserBookBorrowDto, UserBookBorrow>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
            .ForMember(dest => dest.BorrowDate, opt => opt.MapFrom(src => src.BorrowDate))
            .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate))
            .ReverseMap();
    }
}
