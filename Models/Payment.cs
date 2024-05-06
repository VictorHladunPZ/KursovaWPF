using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int ContractId { get; set; }

    public DateOnly Date { get; set; }

    public decimal Amount { get; set; }

    public virtual Contract Contract { get; set; } = null!;
}
