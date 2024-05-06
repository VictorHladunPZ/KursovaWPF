using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KursovaWPF.Models;

public partial class LogTable
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int LogId { get; set; }

    public DateOnly Timestamp { get; set; }

    public string Message { get; set; } = null!;
}
