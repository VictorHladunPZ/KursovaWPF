using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.Resources.Repositories
{
    public  static class ProjectsTimeTableRepository
    {
        public static ProjectsTimeTable GetByDates(DateOnly StartDate, DateOnly Deadline)
        {
            using (MainDBContext context = new MainDBContext())
            {
                var timeTableList = context.ProjectsTimeTables.ToList();
                return timeTableList.Where(c => c.StartDate.Equals(StartDate)).Where(c => c.DeadLine.Equals(Deadline)).FirstOrDefault();
            }
        }
        public static string CreateTimeTableProjects(DateOnly Start, DateOnly Deadline)
        {
            string result = "Already exists";
            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                var foundTimeTable = GetByDates(Start,Deadline);
                if (null == foundTimeTable)
                {
                    ProjectsTimeTable newTimeTable = new ProjectsTimeTable();
                    newTimeTable.StartDate = Start;
                    newTimeTable.DeadLine = Deadline;

                    context.ProjectsTimeTables.Add(newTimeTable);
                    context.SaveChanges();
                    result = "Successfully added!";
                }
            }
            Logger.LogAction(
                string.Format(
                    "User {0} attempted to add a Project's Time Table",
                    MainDBContext.Login
                    )
                );
            return result;
        }
        public static string RemoveTimeTableProjects(ProjectTimeTableViewModel viewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                var foundTimeTable = GetById(viewModel.ProjectTimeTableId);
                if (null != foundTimeTable)
                {
                    context.ProjectsTimeTables.Remove(foundTimeTable);
                    context.SaveChanges();
                    result = "Successfully removed!";
                }
            }
            Logger.LogAction(
                string.Format(
                    "User {0} attempted to remove a Project's Time Table",
                    MainDBContext.Login
                    )
                );
            return result;
        }
        public static string EditTimeTableProjects(ProjectTimeTableViewModel viewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                var foundTimeTable = GetById(viewModel.ProjectTimeTableId);
                if (null != foundTimeTable)
                {
                    foundTimeTable.StartDate = viewModel.StartDate;
                    foundTimeTable.DeadLine = viewModel.DeadLine;
                    context.SaveChanges();
                    result = "Successfully edited!";
                }
            }
            Logger.LogAction(
                string.Format(
                    "User {0} attempted to edit a Project's Time Table",
                    MainDBContext.Login
                    )
                );
            return result;
        }
        public static ProjectsTimeTable GetById(int id)
        {
            using (MainDBContext context = new MainDBContext())
            {
                var timeTableList = context.ProjectsTimeTables.ToList();
                return timeTableList.Where(c => c.ProjectTimeTableId == id).FirstOrDefault();
            }
        }
    }
}
