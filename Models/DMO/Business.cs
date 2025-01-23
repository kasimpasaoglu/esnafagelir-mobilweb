using System;
using System.Collections.Generic;

namespace esnafagelir_mobilweb.DMO;

public partial class Business
{
    public int BusinessId { get; set; }

    public int BusinessTypeId { get; set; }

    public int DistrictId { get; set; }

    public string BusinessName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public bool AllowsCooperation { get; set; }

    public virtual BusinessType BusinessType { get; set; } = null!;

    public virtual District District { get; set; } = null!;

    public virtual ICollection<ExpertRequest> ExpertRequests { get; set; } = new List<ExpertRequest>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
