using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWPF.Models;

public partial class ContractsStatusTable
{
    public int ContractStatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
