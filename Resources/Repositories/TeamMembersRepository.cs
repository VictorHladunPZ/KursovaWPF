using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace KursovaWPF.Resources.Repositories
{
    public static class TeamMembersRepository
    {
        public static List<TeamMemberViewModel> LoadTeamMembers()
        {
            List<TeamMemberViewModel> list = new List<TeamMemberViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.EmpTeamMembers
                                            .Select(c => new TeamMemberViewModel
                                            {
                                                TeamMemberId = c.TeamMemberId,
                                                TeamId = c.TeamId,
                                                EmpId = c.EmpId,
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
        public static string AddTeamMember(TeamMemberViewModel ViewModel)
        {
            string result = "Already exists";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                bool isExisting = LoadTeamMembers().Where(c => c.EmpId == ViewModel.EmpId).Any(c => c.TeamId.Equals(ViewModel.TeamId));
                if (!isExisting)
                {
                    EmpTeamMember empTeamMember = new EmpTeamMember();
                    empTeamMember.EmpId = ViewModel.EmpId;
                    empTeamMember.TeamId = ViewModel.TeamId;
                    context.EmpTeamMembers.Add(empTeamMember);
                    context.SaveChanges();
                    result = "Added successfully";
                }
            }
            Logger.LogAction(
               string.Format(
                   "User {0} attempted to add a Team's Member",
                   MainDBContext.Login
                   )
               );
            return result;
        }
        public static string RemoveTeamMember(TeamMemberViewModel ViewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = LoadTeamMembers().Where(c => c.EmpId == ViewModel.EmpId).Any(c => c.TeamId.Equals(ViewModel.TeamId));
                if (isExisting)
                {
                    context.EmpTeamMembers.Remove(context.EmpTeamMembers.Where(cntr => cntr.TeamMemberId == ViewModel.TeamMemberId).First());
                    context.SaveChanges();
                    result = "Deleted successfully!";
                }
            }
            Logger.LogAction(
              string.Format(
                  "User {0} attempted to remove a Team's Member",
                  MainDBContext.Login
                  )
              );
            return result;
        }
        public static string EditProjTask(TeamMemberViewModel ViewModel)
        {
            string result = "Not found";

            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = LoadTeamMembers().Where(c => c.EmpId == ViewModel.EmpId).Any(c => c.TeamId.Equals(ViewModel.TeamId));
                if (isExisting)
                {
                    EmpTeamMember empTeamMember = context.EmpTeamMembers.Where(c => c.TeamMemberId == ViewModel.TeamMemberId).First(); ;
                    empTeamMember.EmpId = ViewModel.EmpId;
                    empTeamMember.TeamId = ViewModel.TeamId;
                    context.SaveChanges();
                    result = "Edited successfully";
                }
            }
            Logger.LogAction(
              string.Format(
                  "User {0} attempted to edit a Team's Member",
                  MainDBContext.Login
                  )
              );
            return result;
        }

        public static void OpenAddWindow()
        {
            AddTeamMemberWindow newWindow = new AddTeamMemberWindow();
            SetupWindow(newWindow);
        }
        private static void SetupWindow(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
    }
}
