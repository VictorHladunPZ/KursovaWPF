using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class ProjTask
{
    public int TaskId { get; set; }

    public int ProjectId { get; set; }

    public string Description { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
