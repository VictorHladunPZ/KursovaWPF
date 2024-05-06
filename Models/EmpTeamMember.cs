using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class EmpTeamMember
{
    public int TeamMemberId { get; set; }

    public int TeamId { get; set; }

    public int EmpId { get; set; }

    public virtual EmpPosition Emp { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
