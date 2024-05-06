using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KursovaWPF.Resources.Repositories
{
    public static class TeamsRepository
    {
        public static List<TeamViewModel> LoadTeams()
        {
            List<TeamViewModel> list = new List<TeamViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.Teams
                                            .Select(c => new TeamViewModel
                                            {
                                                ProjectId = c.ProjectId,
                                                TeamId = c.TeamId,
                                                TeamName = c.TeamName,
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
        public static string AddTeam(TeamViewModel ViewModel)
        {
            string result = "Already exists";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                bool isExisting = LoadTeams().Any(c => c.TeamName.Equals(ViewModel.TeamName));
                if (!isExisting)
                {
                    Team Team = new Team();
                    Team.ProjectId = ViewModel.ProjectId;
                    Team.TeamName = ViewModel.TeamName;
                    context.Teams.Add(Team);
                    context.SaveChanges();
                    result = "Added successfully";
                }
            }
            Logger.LogAction(
              string.Format(
                  "User {0} attempted to add a Team",
                  MainDBContext.Login
                  )
              );
            return result;
        }
        public static string RemoveTeam(TeamViewModel ViewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = LoadTeams().Any(c => c.TeamName.Equals(ViewModel.TeamName));
                if (isExisting)
                {
                    context.Teams.Remove(context.Teams.Where(cntr => cntr.TeamId == ViewModel.TeamId).First());
                    context.SaveChanges();
                    result = "Deleted successfully!";
                }
            }

            Logger.LogAction(
              string.Format(
                  "User {0} attempted to remove a Team",
                  MainDBContext.Login
                  )
              );
            return result;
        }
        public static string EditTeam(TeamViewModel ViewModel)
        {
            string result = "Not found";

            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = LoadTeams().Any(c => c.TeamName.Equals(ViewModel.TeamName));
                if (isExisting)
                {
                    Team Team = context.Teams.Where(c => c.TeamId == ViewModel.TeamId).First();
                    Team.ProjectId = ViewModel.ProjectId;
                    Team.TeamName = ViewModel.TeamName;
                    context.SaveChanges();
                    result = "Added successfully";
                }
            }

            Logger.LogAction(
              string.Format(
                  "User {0} attempted to edit a Team",
                  MainDBContext.Login
                  )
              );
            return result;
        }
    }
}
