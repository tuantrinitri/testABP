using AutoMapper;
using Backend.Account.Dto;
using Backend.Account.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;

namespace Backend.Account.Web;

public class AccountWebAutoMapperProfile : Profile
{
    public AccountWebAutoMapperProfile()
    {
        CreateMap<ProfileDto, AccountProfilePersonalInfoManagementGroupViewComponent.PersonalInfoModel>()
            .MapExtraProperties();
    }
}
