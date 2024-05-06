using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KursovaWPF.Resources.Repositories
{
    public class ProjectTasksRepository
    {
        public static List<ProjTaskViewModel> LoadTasks()
        {
            List<ProjTaskViewModel> list = new List<ProjTaskViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.ProjTasks
                                            .Select(c => new ProjTaskViewModel
                                            {
                                               ProjectId = c.ProjectId,
                                               Description = c.Description,
                                               TaskId = c.TaskId,
                                            })
                    ];

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
        public static string AddTask(ProjTaskViewModel ViewModel)
        {
            string result = "Already exists";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                bool isExisting = LoadTasks().Where(c => c.ProjectId == ViewModel.ProjectId).Any(c => c.Description.Equals(ViewModel.Description));
                if (!isExisting)
                {
                    ProjTask ProjTask = new ProjTask();
                    ProjTask.ProjectId = ViewModel.ProjectId;
                    ProjTask.Description = ViewModel.Description;
                    context.ProjTasks.Add(ProjTask);
                    context.SaveChanges();
                    result = "Added successfully";
                }
            }
            Logger.LogAction(
               string.Format(
                   "User {0} attempted to add a Project's task",
                   MainDBContext.Login
                   )
               );
            return result;
        }
        public static string RemoveProjTask(ProjTaskViewModel ViewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = LoadTasks().Any(c => c.TaskId.Equals(ViewModel.TaskId));
                if (isExisting)
                {
                    context.ProjTasks.Remove(context.ProjTasks.Where(cntr => cntr.TaskId == ViewModel.TaskId).First());
                    context.SaveChanges();
                    result = "Deleted successfully!";
                }
            }
            Logger.LogAction(
                string.Format(
                    "User {0} attempted to remove a Project's task",
                    MainDBContext.Login
                    )
                );
            return result;
        }
        public static string EditProjTask(ProjTaskViewModel ViewModel)
        {
            string result = "Not found";

            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = LoadTasks().Any(c => c.TaskId == ViewModel.TaskId);
                if (isExisting)
                {
                    ProjTask ProjTask = context.ProjTasks.Where(c => c.TaskId == ViewModel.TaskId).First();
                    ProjTask.ProjectId = ViewModel.ProjectId;
                    ProjTask.Description = ViewModel.Description;
                    context.SaveChanges();
                    result = "Added successfully";
                }
            }
            Logger.LogAction(
               string.Format(
                   "User {0} attempted to edit a Project's task",
                   MainDBContext.Login
                   )
               );
            return result;
        }
    }
}
