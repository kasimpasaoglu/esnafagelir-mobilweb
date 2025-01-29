using System;
using System.Collections.Generic;

namespace esnafagelir_mobilweb.DMO;

public partial class ExpertCategory
{
    public int ExpertCategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? CategoryDescription { get; set; }

    public string ImagePath { get; set; } = null!;

    public virtual ICollection<ExpertRequest> ExpertRequests { get; set; } = new List<ExpertRequest>();
}
