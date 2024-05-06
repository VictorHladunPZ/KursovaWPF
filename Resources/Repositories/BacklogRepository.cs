using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KursovaWPF.Resources.Repositories
{
    public static class BacklogRepository
    {
        public static List<ProjectsBacklogViewModel> LoadBacklogs()
        {
            List<ProjectsBacklogViewModel> list = new List<ProjectsBacklogViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.ProjectBacklogs
                                            .Select(c => new ProjectsBacklogViewModel
                                            {
                                                Project = ProjectRepository.GetById(c.ProjectId).Title,
                                                ProjectRetrospective = c.ProjectRetrospective,
                                                BacklogId = c.BacklogId,
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
        public static string AddBacklog(ProjectsBacklogViewModel viewModel)
        {
            string result = "Already exists";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                bool isExisting = LoadBacklogs().Any(c => c.ProjectRetrospective.Trim().Equals(viewModel.ProjectRetrospective.Trim()));
                if (!isExisting)
                {
                    ProjectBacklog projectBacklog = new ProjectBacklog();
                    projectBacklog.ProjectId = context.Projects.Where(c => c.Title == viewModel.Project).FirstOrDefault().ProjectId;
                    projectBacklog.ProjectRetrospective = viewModel.ProjectRetrospective;
                    context.ProjectBacklogs.Add(projectBacklog);
                    context.SaveChanges();
                    result = "Added successfully";
                }
            }
            Logger.LogAction(
                    string.Format(
                        "User {0} attempted to create a project backlog",
                        MainDBContext.Login
                        )
                    );
            return result;
        }
        public static string RemoveBacklog(ProjectsBacklogViewModel viewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = LoadBacklogs().Any(c => c.BacklogId.Equals(viewModel.BacklogId));
                if (isExisting)
                {
                    context.ProjectBacklogs.Remove(context.ProjectBacklogs.Where(cntr => cntr.BacklogId == viewModel.BacklogId).First());
                    context.SaveChanges();
                    result = "Deleted successfully!";
                }
            }
            Logger.LogAction(
                   string.Format(
                       "User {0} attempted to delete a project backlog",
                       MainDBContext.Login
                       )
                   );
            return result;
        }
        public static string EditBacklog(ProjectsBacklogViewModel viewModel)
        {
            string result = "Not found";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                ProjectBacklog EntryExisting = context.ProjectBacklogs.FirstOrDefault(cntr => cntr.BacklogId == viewModel.BacklogId);
                EntryExisting.ProjectRetrospective = viewModel.ProjectRetrospective;
                EntryExisting.ProjectId = ProjectRepository.GetByName(viewModel.Project).ProjectId;
                context.SaveChanges();
                result = "Successfully edited contract";
            }
            Logger.LogAction(
                   string.Format(
                       "User {0} attempted to edit a project backlog",
                       MainDBContext.Login
                       )
                   );
            return result;
        }
    }
}
