using AutoMapper;
using EShopOnAbp.IdentityService.Keycloak.Service;
using Keycloak.Net.Models.Roles;
using Keycloak.Net.Models.Users;

namespace EShopOnAbp.IdentityService;

public class EShopOnAbpIdentityServiceAutoMapperProfile : Profile
{
    public EShopOnAbpIdentityServiceAutoMapperProfile()
    {
        CreateMap<User, CachedKeycloakUser>().ReverseMap();
        CreateMap<UserAccess, CachedUserAccess>().ReverseMap();
        CreateMap<UserConsent, CachedUserConsent>().ReverseMap();
        CreateMap<Credentials, CachedCredentials>().ReverseMap();
        CreateMap<FederatedIdentity, CachedFederatedIdentity>();
        
        CreateMap<Role, CachedKeycloakRole>().ReverseMap();
        CreateMap<RoleComposite, CachedRoleComposite>().ReverseMap();
    }
}