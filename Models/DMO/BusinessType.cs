using System;
using System.Collections.Generic;

namespace esnafagelir_mobilweb.DMO;

public partial class BusinessType
{
    public int BusinessTypeId { get; set; }

    public string BusinessTypeName { get; set; } = null!;

    public virtual ICollection<Business> Businesses { get; set; } = new List<Business>();
}
