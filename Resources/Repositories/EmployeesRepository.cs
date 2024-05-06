using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.Resources.Repositories
{
    public static class EmployeesRepository
    {
        public static List<EmployeeViewModel> LoadEmployees()
        {
            List<EmployeeViewModel> list = new List<EmployeeViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.Employees
                                            .Select(c => new EmployeeViewModel
                                            {
                                                EmployeeId = c.EmployeeId,
                                                FirstName = c.FirstName,
                                                LastName = c.LastName,
                                                Salary = c.Salary,
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
        public static string AddEmployee(EmployeeViewModel ViewModel)
        {
            string result = "Already exists";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                bool isExisting = LoadEmployees().Where(c => c.FirstName.Trim().Equals(ViewModel.FirstName.Trim())).Any(c => c.LastName.Trim().Equals(ViewModel.LastName.Trim()));
                if (!isExisting)
                {
                    Employee newEmp = new Employee();
                    newEmp.FirstName = ViewModel.FirstName;
                    newEmp.LastName = ViewModel.LastName;
                    newEmp.Salary = ViewModel.Salary;
                    context.Employees.Add(newEmp);
                    context.SaveChanges();
                    result = "Added successfully";
                }
            }
            Logger.LogAction(
                   string.Format(
                       "User {0} attempted to add an Employee",
                       MainDBContext.Login
                       )
                   );
            return result;
        }
        public static string EditEmployee(EmployeeViewModel ViewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = LoadEmployees().Any(c => c.EmployeeId.Equals(ViewModel.EmployeeId));
                if (isExisting)
                {
                    context.Employees.Remove(context.Employees.Where(cntr => cntr.EmployeeId == ViewModel.EmployeeId).First());
                    context.SaveChanges();
                    result = "Deleted successfully!";
                }
            }
            Logger.LogAction(
                  string.Format(
                      "User {0} attempted to edit an Employee",
                      MainDBContext.Login
                      )
                  );
            return result;
        }
        public static string RemoveEmployee(EmployeeViewModel ViewModel)
        {
            string result = "Not found";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                Employee EntryExisting = context.Employees.FirstOrDefault(cntr => cntr.EmployeeId == ViewModel.EmployeeId);
                EntryExisting.FirstName = ViewModel.FirstName;
                EntryExisting.LastName = ViewModel.LastName;
                EntryExisting.Salary = ViewModel.Salary;
                context.SaveChanges();
                result = "Successfully edited contract";
            }
            Logger.LogAction(
                  string.Format(
                      "User {0} attempted to remove an Employee",
                      MainDBContext.Login
                      )
                  );
            return result;
        }

        internal static Employee GetByName(string emp)
        {
            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                return context.Employees.FirstOrDefault(e => (e.FirstName + " " + e.LastName).Equals(emp));
            }

        }
    }
}
