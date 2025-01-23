using System;
using System.Collections.Generic;

namespace esnafagelir_mobilweb.DMO;

public partial class ContactRequest
{
    public int ContactRequestId { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime RecordDate { get; set; }

    public int RecordStatus { get; set; }

    public virtual User User { get; set; } = null!;
}
