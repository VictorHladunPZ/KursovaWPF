using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public int ProductId { get; set; }

    public string Title { get; set; } = null!;

    public int TimeTableId { get; set; }

    public bool Status { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProjTask> ProjTasks { get; set; } = new List<ProjTask>();

    public virtual ICollection<ProjectBacklog> ProjectBacklogs { get; set; } = new List<ProjectBacklog>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

    public virtual ProjectsTimeTable TimeTable { get; set; } = null!;
}
