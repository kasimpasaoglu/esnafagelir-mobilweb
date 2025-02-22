public class RegisterFirstVM
{
    public UserVM User { get; set; } = new UserVM();
    public int SelectedRoleId { get; set; }
    public List<RoleVM> Roles { get; set; }

}

public class RegisterSecondVM
{
    public UserVM User { get; set; } = new UserVM();
    public BusinessVM Business { get; set; } = new BusinessVM();
    public int SelectedBusinessTypeId { get; set; }
    public List<BusinessTypeVM> BusinessTypes { get; set; }
    public int SelectedCityId { get; set; }
    public List<CityVM> Cities { get; set; }
    public int SelectedDisrictId { get; set; }
    public List<DistrictVM> Districts { get; set; }
}
