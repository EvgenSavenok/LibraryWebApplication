using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;

namespace Application.Profiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserForRegistrationDto, User>();
    }
}
