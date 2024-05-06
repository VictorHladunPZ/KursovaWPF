using System;
using System.Collections.Generic;

namespace KursovaWPF.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public decimal Salary { get; set; }

    public virtual ICollection<EmpPosition> EmpPositions { get; set; } = new List<EmpPosition>();
}
