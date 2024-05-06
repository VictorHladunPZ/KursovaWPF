using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class EmpRole
{
    public int EmpRoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<EmpPosition> EmpPositions { get; set; } = new List<EmpPosition>();
}
