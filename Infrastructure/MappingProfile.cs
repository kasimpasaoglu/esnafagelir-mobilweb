using AutoMapper;
using esnafagelir_mobilweb.DMO;
using Infrastructure.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region User Model
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<UserDTO, UserVM>().ReverseMap();
        #endregion

        CreateMap<LoginDTO, LoginVM>().ReverseMap();

    }
}