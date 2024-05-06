using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public int ContractId { get; set; }

    public virtual Contract Contract { get; set; } = null!;

    public virtual ICollection<ProdDemand> ProdDemands { get; set; } = new List<ProdDemand>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
