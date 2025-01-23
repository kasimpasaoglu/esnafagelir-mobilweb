using System;
using System.Collections.Generic;

namespace esnafagelir_mobilweb.DMO;

public partial class District
{
    public int DistrictId { get; set; }

    public int CityId { get; set; }

    public string DistrictName { get; set; } = null!;

    public virtual ICollection<Business> Businesses { get; set; } = new List<Business>();

    public virtual City City { get; set; } = null!;
}
