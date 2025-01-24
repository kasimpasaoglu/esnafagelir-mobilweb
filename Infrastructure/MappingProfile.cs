using AutoMapper;
using esnafagelir_mobilweb.DMO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        #region DatabaseTipleri Maplemesi
        CreateMap<User, UserDTO>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UserDTO, UserVM>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Business, BusinessDTO>().ReverseMap();
        CreateMap<BusinessType, BusinessTypeDTO>().ReverseMap();
        CreateMap<City, CityDTO>().ReverseMap();
        CreateMap<ContactRequest, ContactRequestDTO>().ReverseMap();
        CreateMap<District, DistrictDTO>().ReverseMap();
        CreateMap<ExpertCategory, ExpertCategoryDTO>().ReverseMap();
        CreateMap<ExpertRequest, ExpertRequestDTO>().ReverseMap();
        CreateMap<Role, RoleDTO>().ReverseMap();
        #endregion

    }
}