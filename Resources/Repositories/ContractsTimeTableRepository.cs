using KursovaWPF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.Resources.Repositories
{
    public static class ContractsTimeTableRepository
    {
        public static ContractsTimeTable GetByDates(DateOnly StartDate, DateOnly Deadline)
        {
            using (MainDBContext context = new MainDBContext())
            {
                var timeTableList = context.ContractsTimeTables.ToList();
                return timeTableList.Where(c => c.StartDate.Equals(StartDate)).Where(c => c.Deadline.Equals(Deadline)).FirstOrDefault();
            }
        }
        public static void CreateTimeTableContracts(DateOnly Start, DateOnly Deadline)
        {
            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                var timeTableList = context.ContractsTimeTables.ToList();
                var foundTimeTable = GetByDates(Start,Deadline);
                if (null == foundTimeTable)
                {
                    ContractsTimeTable newTimeTable = new ContractsTimeTable();
                    newTimeTable.StartDate = Start;
                    newTimeTable.Deadline = Deadline;

                    context.ContractsTimeTables.Add(newTimeTable);
                    context.SaveChanges();

                }
            }
            Logger.LogAction(
                   string.Format(
                       "User {0} attempted to add a Contract Time Table",
                       MainDBContext.Login
                       )
                   );
        }
        public static ContractsTimeTable GetById(int id)
        {
            using (MainDBContext context = new MainDBContext())
            {
                var timeTableList = context.ContractsTimeTables.ToList();
                return timeTableList.Where(c => c.TimeTableId == id ).FirstOrDefault();
            }
        }
    }
}
