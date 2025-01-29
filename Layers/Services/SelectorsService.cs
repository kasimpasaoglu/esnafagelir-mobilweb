using AutoMapper;
using esnafagelir_mobilweb.DMO;

public class SelectorsService : ISelectorsService
{
    private IGenericRepository<Role> _rolesRepo;
    private IGenericRepository<BusinessType> _businessTypesRepo;
    private IGenericRepository<City> _citiesRepo;
    private IGenericRepository<District> _districtsRepo;
    private IMapper _mapper;

    public SelectorsService
    (
        IGenericRepository<Role> rolesRepo,
        IGenericRepository<BusinessType> businessTypesRepo,
        IGenericRepository<City> citiesRepo,
        IGenericRepository<District> district,
        IMapper mapper
    )
    {
        _rolesRepo = rolesRepo;
        _businessTypesRepo = businessTypesRepo;
        _citiesRepo = citiesRepo;
        _districtsRepo = district;
        _mapper = mapper;
    }

    public async Task<List<BusinessTypeDTO>> GetBusinessTypesList()
    {
        var typesListDMO = await _businessTypesRepo.FindAsync(x => x.BusinessTypeId > 0);
        return _mapper.Map<List<BusinessTypeDTO>>(typesListDMO);
    }

    public async Task<List<CityDTO>> GetCitiesList()
    {
        var citiesListDMO = await _citiesRepo.FindAsync(x => x.CityId > 0);
        return _mapper.Map<List<CityDTO>>(citiesListDMO);
    }

    public async Task<List<DistrictDTO>> GetDistrictsByCityId(int cityId)
    {
        var districtsListDMO = await _districtsRepo.FindAsync(x => x.CityId == cityId);
        return _mapper.Map<List<DistrictDTO>>(districtsListDMO);
    }
    public async Task<int> GetCityIdByDisrictId(int discrictId)
    {
        var district = await _districtsRepo.FindAsync(x => x.DistrictId == discrictId);
        return district?.FirstOrDefault().CityId ?? 0;
    }

    public async Task<List<RoleDTO>> GetRolesList()
    {
        var rolesListDMO = await _rolesRepo.FindAsync(x => x.RoleId > 0);
        return _mapper.Map<List<RoleDTO>>(rolesListDMO);
    }
}


public interface ISelectorsService
{
    Task<List<CityDTO>> GetCitiesList();
    Task<List<DistrictDTO>> GetDistrictsByCityId(int cityId);
    Task<int> GetCityIdByDisrictId(int discrictId);
    Task<List<RoleDTO>> GetRolesList();
    Task<List<BusinessTypeDTO>> GetBusinessTypesList();

}