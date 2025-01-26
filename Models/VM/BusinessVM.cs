public partial class BusinessVM
{
    public int BusinessId { get; set; }

    public int BusinessTypeId { get; set; }

    public int DistrictId { get; set; }

    public string BusinessName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public bool AllowsCooperation { get; set; }


}