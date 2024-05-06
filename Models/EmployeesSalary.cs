using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class EmployeesSalary
{
    public int EmployeeKey { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public decimal? Pay { get; set; }
}
