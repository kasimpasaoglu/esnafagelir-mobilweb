using AutoMapper;
using esnafagelir_mobilweb.DMO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LoginVM, UserVM>().ReverseMap();

        #region DatabaseTipleri Maplemesi
        CreateMap<User, UserDTO>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UserDTO, UserVM>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Business, BusinessDTO>().ReverseMap();
        CreateMap<BusinessDTO, BusinessVM>().ReverseMap();

        CreateMap<BusinessType, BusinessTypeDTO>().ReverseMap();
        CreateMap<BusinessTypeDTO, BusinessTypeVM>().ReverseMap();

        CreateMap<City, CityDTO>().ReverseMap();
        CreateMap<CityDTO, CityVM>().ReverseMap();

        CreateMap<ContactRequest, ContactRequestDTO>().ReverseMap();
        CreateMap<ContactRequestDTO, ContactRequestVM>().ReverseMap();

        CreateMap<District, DistrictDTO>().ReverseMap();
        CreateMap<DistrictDTO, DistrictVM>().ReverseMap();

        CreateMap<ExpertCategory, ExpertCategoryDTO>().ReverseMap();
        CreateMap<ExpertCategoryDTO, ExpertCategoryVM>().ReverseMap();

        CreateMap<ExpertRequest, ExpertRequestDTO>().ReverseMap();
        CreateMap<ExpertRequestDTO, ExpertRequestVM>().ReverseMap();

        CreateMap<Role, RoleDTO>().ReverseMap();
        CreateMap<RoleDTO, RoleVM>().ReverseMap();
        #endregion

    }
}