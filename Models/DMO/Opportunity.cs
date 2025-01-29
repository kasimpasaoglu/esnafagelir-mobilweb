using System;
using System.Collections.Generic;

namespace esnafagelir_mobilweb.DMO;

public partial class Opportunity
{
    public int OpportunityId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ImagePath { get; set; } = null!;

    public string? Url { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsPrimary { get; set; }
}
