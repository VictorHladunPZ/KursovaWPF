using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace KursovaWPF.Resources.Repositories
{
    public static class PositionsRepository
    {
        public static List<EmpPositionViewModel> LoadPositions()
        {
            List<EmpPositionViewModel> list = new List<EmpPositionViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.EmpPositions
                                            .Select(c => new EmpPositionViewModel
                                            { 
                                                Emp = c.Emp.FirstName + " " + c.Emp.LastName,
                                                PayFactor = c.PayFactor,
                                                PositionId = c.PositionId,
                                                Role = c.Role.RoleName,
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
        public static string AddPosition(EmpPositionViewModel ViewModel)
        {
            string result = "Already exists";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                bool isExisting = LoadPositions().Where(c => c.Emp.Equals(ViewModel.Emp)).Any(c => c.Role.Equals(ViewModel.Role));
                if (!isExisting)
                {
                    EmpPosition EmpPosition = new EmpPosition();
                    EmpPosition.EmpId = context.Employees.FirstOrDefault(e => (e.FirstName + " " + e.LastName).Equals(ViewModel.Emp)).EmployeeId;
                    EmpPosition.PayFactor = ViewModel.PayFactor;
                    EmpPosition.RoleId = context.EmpRoles.Where(r => r.RoleName.Equals(ViewModel.Role)).First().EmpRoleId;
                    context.EmpPositions.Add(EmpPosition);
                    context.SaveChanges();
                    result = "Added successfully";
                }
            }
            Logger.LogAction(
                 string.Format(
                     "User {0} attempted to add a Position for an Employee",
                     MainDBContext.Login
                     )
                 );
            return result;
        }
        public static string RemovePosition(EmpPositionViewModel ViewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = LoadPositions().Any(c => c.PositionId.Equals(ViewModel.PositionId));
                if (isExisting)
                {
                    context.EmpPositions.Remove(context.EmpPositions.Where(cntr => cntr.PositionId == ViewModel.PositionId).First());
                    context.SaveChanges();
                    result = "Deleted successfully!";
                }
            }
            Logger.LogAction(
                 string.Format(
                     "User {0} attempted to remove a Position for an Employee",
                     MainDBContext.Login
                     )
                 );
            return result;
        }
        public static string EditPosition(EmpPositionViewModel ViewModel)
        {
            string result = "Not found";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                EmpPosition EntryExisting = context.EmpPositions.FirstOrDefault(cntr => cntr.PositionId == ViewModel.PositionId);
                EntryExisting.EmpId = EmployeesRepository.GetByName(ViewModel.Emp).EmployeeId;
                EntryExisting.PayFactor = ViewModel.PayFactor;
                EntryExisting.RoleId = RolesRepository.GetByName(ViewModel.Role).EmpRoleId;
                context.SaveChanges();
                result = "Successfully edited contract";
            }
            Logger.LogAction(
                 string.Format(
                     "User {0} attempted to edit a Position for an Employee",
                     MainDBContext.Login
                     )
                 );
            return result;
        }
    }
}
