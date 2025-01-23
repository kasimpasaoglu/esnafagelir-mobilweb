using System;
using System.Collections.Generic;

namespace esnafagelir_mobilweb.DMO;

public partial class ExpertRequest
{
    public int ExpertRequestId { get; set; }

    public int UserId { get; set; }

    public int BusinessId { get; set; }

    public int ExpertCategoryId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime RecordDate { get; set; }

    public int RecordStatus { get; set; }

    public virtual Business Business { get; set; } = null!;

    public virtual ExpertCategory ExpertCategory { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
