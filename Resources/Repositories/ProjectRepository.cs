using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KursovaWPF.Resources.Repositories
{
    public static class ProjectRepository
    {
        #region Project Methods
        public static Project GetByName(string Name)
        {
            using (MainDBContext dbContext = new MainDBContext())
            {
                return dbContext.Projects.Where(c => c.Title == Name).FirstOrDefault();
            }
        }
            
        public static List<ProjectViewModel> LoadProjects()
        {
            List<ProjectViewModel> list = new List<ProjectViewModel>();

            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    Console.WriteLine("Connection establishing...");
                    dbContext.Database.OpenConnection();
                    Console.WriteLine("Connection established successfully.");
                    list = dbContext.Projects
                    .Select(c => new ProjectViewModel
                    {
                        ProjectId = c.ProjectId,
                        Status = c.Status ? "Completed" : "Uncompleted",
                        StartDate = c.TimeTable.StartDate,
                        Deadline = c.TimeTable.DeadLine,
                        Title = c.Title,
                        ProductName = c.Product.Name,
                    })
                    .ToList();
                    
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log the error)
                    Console.WriteLine($"An error occurred while loading contracts: {ex.Message}");
                    // Optionally, you can re-throw the exception or take other appropriate actions.
                }
            }

            return list;
        }
        public static string DeleteProejct(ProjectViewModel deleteViewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = context.Projects.Any(prj => prj.ProjectId == deleteViewModel.ProjectId);
                if (isExisting)
                {
                    context.Projects.Remove(context.Projects.Where(cntr => cntr.ProjectId == deleteViewModel.ProjectId).First());
                    context.SaveChanges();
                    result = "Deleted successfully!";
                }
            }
            Logger.LogAction(
                string.Format(
                    "User {0} attempted to delete a Project",
                    MainDBContext.Login
                    )
                );
            return result;
        }
        public static string EditProject(ProjectViewModel editViewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                Project EntryExisting = context.Projects.FirstOrDefault(prj => prj.ProjectId == editViewModel.ProjectId);
                EntryExisting.Status = editViewModel.Status.Trim().Equals("Completed");
                var TimeTableFound = ProjectsTimeTableRepository.GetByDates(editViewModel.StartDate,editViewModel.Deadline);
                TimeTableFound.StartDate = editViewModel.StartDate;
                TimeTableFound.DeadLine = editViewModel.Deadline;
                EntryExisting.Product = ProductsRepository.GetByName(editViewModel.ProductName);
                EntryExisting.Title = editViewModel.Title;
                context.SaveChanges();
                result = "Successfully edited contract";
            }
            Logger.LogAction(
                string.Format(
                    "User {0} attempted to edit a Project",
                    MainDBContext.Login
                    )
                );
            return result;
        }
        public static string AddProject(ProjectViewModel addViewModel)
        {
            string result = "Already exists";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                bool isExisting = LoadProjects().Any(p => p.Title == addViewModel.Title);
                if (!isExisting)
                {
                    Project newProject = new();
                    newProject.Title = addViewModel.Title;
                    newProject.Status = addViewModel.Status.Trim().Equals("Completed");
                    newProject.ProductId = context.Products.Where(p => p.Name.Trim().Equals(addViewModel.ProductName.Trim())).FirstOrDefault().ProductId;
                    var timeTableList = context.ProjectsTimeTables.ToList();
                    var foundTimeTable = timeTableList.Where(c => c.StartDate.Equals(addViewModel.StartDate)).Where(c => c.DeadLine.Equals(addViewModel.Deadline)).FirstOrDefault();
                    if (foundTimeTable != null)
                    {
                        newProject.TimeTable = foundTimeTable;
                    }
                    else
                    {
                        ProjectsTimeTableRepository.CreateTimeTableProjects(addViewModel.StartDate, addViewModel.Deadline);
                        foundTimeTable = timeTableList.Where(c => c.StartDate.Equals(addViewModel.StartDate)).Where(c => c.DeadLine.Equals(addViewModel.Deadline)).FirstOrDefault();
                        newProject.TimeTable = foundTimeTable;
                    }
                    context.Projects.Add(newProject);
                    context.SaveChanges();
                    result = "Added successfully";
                }
            }
            Logger.LogAction(
                string.Format(
                    "User {0} attempted to add a Project",
                    MainDBContext.Login
                    )
                );
            return result;

        }

        #endregion


        private static void SetupWindow(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        public static Project GetById(int projectId)
        {
            using (MainDBContext dbContext = new MainDBContext())
            {
                return dbContext.Projects.Where(c => c.ProjectId == projectId).FirstOrDefault();
            }
        }

    }
}
