using System;
using System.Collections.Generic;

namespace esnafagelir_mobilweb.DMO;

public partial class User
{
    public int UserId { get; set; }

    public Guid DeviceId { get; set; } = new Guid();

    public DateTime LastLogin { get; set; }

    public bool IsConfirmedInfoText { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int RoleId { get; set; }

    public DateTime RegisterDate { get; set; }

    public int BusinessId { get; set; }

    public virtual Business Business { get; set; } = null!;

    public virtual ICollection<ContactRequest> ContactRequests { get; set; } = new List<ContactRequest>();

    public virtual ICollection<ExpertRequest> ExpertRequests { get; set; } = new List<ExpertRequest>();

    public virtual Role Role { get; set; } = null!;
}
