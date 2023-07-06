using AutoMapper;
using Backend.Identity.IdentityRoles;
using Backend.Identity.IdentityRoles.Dto;
using Backend.Identity.IdentityUsers;
using Backend.Identity.IdentityUsers.Dto;
using Backend.Identity.Positions;
using Backend.Identity.Positions.Dto;
using Backend.Identity.Ranks;
using Backend.Identity.Ranks.Dto;

namespace Backend.Identity;

public class IdentityApplicationModuleAutoMapperProfile: Profile
{
    public IdentityApplicationModuleAutoMapperProfile()
    {
        CreateMap<IdentityUser, IdentityUserDto>()
            .MapExtraProperties();

        CreateMap<IdentityRole, IdentityRoleDto>()
            .MapExtraProperties();

        CreateMap<Position, PositionDto>();
        CreateMap<PositionCreateDto, Position>();
        CreateMap<PositionUpdateDto, Position>();

        CreateMap<Rank, RankDto>();
        CreateMap<RankCreateDto, Rank>();
        CreateMap<RankUpdateDto, Rank>();
        
        CreateMap<ProfileDto, UserProfile>().ReverseMap();
    }
}