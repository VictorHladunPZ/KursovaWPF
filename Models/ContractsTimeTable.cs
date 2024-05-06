using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWPF.Models;

public partial class ContractsTimeTable
{
    public int TimeTableId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? Deadline { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
