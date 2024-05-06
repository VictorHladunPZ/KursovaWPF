using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class ProjectsTimeTable
{
    public int ProjectTimeTableId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly DeadLine { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
