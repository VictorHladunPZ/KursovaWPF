using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWPF.Models;

public partial class Contract
{
    public int ContractId { get; set; }
    public int OwnerId { get; set; }

    public string Description { get; set; } = null!;
    public int TimeTableId { get; set; }
    public int StatusId { get; set; }

    public decimal? Cost { get; set; }

    public virtual Contractee Owner { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ContractsStatusTable Status { get; set; } = null!;

    public virtual ContractsTimeTable TimeTable { get; set; } = null!;
}
