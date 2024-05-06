using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class ContractView
{
    public int ContractId { get; set; }

    public string Contractee { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? Deadline { get; set; }

    public decimal? Cost { get; set; }
}
