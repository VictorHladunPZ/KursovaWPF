using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class ProjectBacklog
{
    public int BacklogId { get; set; }

    public string? ProjectRetrospective { get; set; }

    public int ProjectId { get; set; }

    public virtual Project Project { get; set; } = null!;
}
