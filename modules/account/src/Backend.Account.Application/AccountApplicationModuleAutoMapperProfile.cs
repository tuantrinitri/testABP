using AutoMapper;
using Backend.Account.Dto;
using Backend.Identity.IdentityUsers;

namespace Backend.Account;

public class AccountApplicationModuleAutoMapperProfile : Profile
{
    public AccountApplicationModuleAutoMapperProfile()
    {
        CreateMap<IdentityUser, ProfileDto>()
            .ForMember(dest => dest.HasPassword,
                op => op.MapFrom(src => src.PasswordHash != null))
            .MapExtraProperties();
    }
}
