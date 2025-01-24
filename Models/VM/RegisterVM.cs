using esnafagelir_mobilweb.DMO;

public class RegisterVM
{
    public UserVM User { get; set; } = new UserVM();
    public Business Business { get; set; } = new Business();
    public int SelectedRoleId { get; set; }
    public List<Role> Roles { get; set; }
    public int SelectedBusinessTypeId { get; set; }
    public List<BusinessType> BusinessTypes { get; set; }
    public int SelectedCityId { get; set; }
    public List<City> Cities { get; set; }
    public int SelectedDisrictId { get; set; }
    public List<District> Districts { get; set; }
}