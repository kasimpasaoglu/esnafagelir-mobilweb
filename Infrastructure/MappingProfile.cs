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

    }
}