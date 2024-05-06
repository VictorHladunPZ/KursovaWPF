using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class EmpPosition
{
    public int PositionId { get; set; }

    public int EmpId { get; set; }

    public decimal PayFactor { get; set; }

    public int RoleId { get; set; }

    public virtual Employee Emp { get; set; } = null!;

    public virtual ICollection<EmpTeamMember> EmpTeamMembers { get; set; } = new List<EmpTeamMember>();

    public virtual EmpRole Role { get; set; } = null!;
}
