using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;

namespace Application.Profiles;

public class BorrowMappingProfile : Profile
{
    public BorrowMappingProfile()
    {
        CreateMap<UserBookBorrowDto, UserBookBorrow>();
    }
}
