using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class Team
{
    public int TeamId { get; set; }

    public int ProjectId { get; set; }

    public string TeamName { get; set; } = null!;

    public virtual ICollection<EmpTeamMember> EmpTeamMembers { get; set; } = new List<EmpTeamMember>();

    public virtual Project Project { get; set; } = null!;
}
