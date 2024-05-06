using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class ProdDemand
{
    public int DemandsId { get; set; }

    public int ProductId { get; set; }

    public string Demand { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
