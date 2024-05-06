using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWPF.Models;

public partial class Contractee
{
    public int ContracteeId { get; set; }

    public string Name { get; set; } = null!;

    public string ContactInformation { get; set; } = null!;

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
