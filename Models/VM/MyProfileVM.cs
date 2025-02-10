public class MyProfileVM
{
    public UserVM User { get; set; }
    public List<RoleVM> Roles { get; set; }
    public int SelectedRoleId { get; set; }
    public BusinessVM Business { get; set; }
    public List<BusinessTypeVM> BusinessTypes { get; set; }
    public int SelectedBusinessTypeId { get; set; }
    public List<CityVM> Cities { get; set; }
    public int SelectedCityId { get; set; }
    public List<DistrictVM> Districts { get; set; }
    public int SelectedDisrictId { get; set; }
    public bool IsUpdatedSuccesfully { get; set; } = false;
}
// bu model ExpertRequestFormMail modelinde de kullaniliyor