public class MyProfileVM
{
    public UserVM User { get; set; }
    public List<RoleVM> Roles { get; set; }
    public int SelectedRoleId { get; set; }
    public BusinessVM Business { get; set; }
    public List<BusinessTypeVM> BusinessTypes { get; set; }
    public int SelectedBusinessTypeId { get; set; }
    public bool IsEditMode { get; set; } = false;
    public bool IsUpdatedSuccesfully { get; set; } = false;
}