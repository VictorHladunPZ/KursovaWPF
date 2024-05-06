using KursovaWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.Resources
{
    static class Logger
    {
        public static void LogAction(string message )
        {
            using (MainDBContext dbContext = new MainDBContext())
            {
                LogTable log = new LogTable();
                log.Message = message;
                log.LogId = dbContext.LogTables.Select(l => l.LogId).Max() + 1;
                log.Timestamp = DateOnly.FromDateTime(DateTime.Now);
                dbContext.LogTables.Add(log);
                dbContext.SaveChanges();
            }
        }
    }
}
