using AutoMapper;
using esnafagelir_mobilweb.DMO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region User Model
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<UserDTO, UserVM>().ReverseMap();
        #endregion

        CreateMap<LoginDTO, LoginVM>().ReverseMap();

        #region DatabaseTipleri Maplemesi
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