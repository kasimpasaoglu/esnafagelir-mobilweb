using System;
using System.Collections.Generic;

namespace esnafagelir_mobilweb.DMO;

public partial class Admin
{
    public int AdminId { get; set; }

    public string DeviceId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string Salt { get; set; } = null!;
}
