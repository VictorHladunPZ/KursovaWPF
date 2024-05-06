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
    public static class RolesRepository
    {
        public static EmpRole GetByName(string role)
        {
            using(MainDBContext context = new MainDBContext())
            {
                return context.EmpRoles.Where(r => r.RoleName.Equals(role)).First();
            }
        }
        public static List<EmpRoleViewModel> LoadRoles()
        {
            List<EmpRoleViewModel> list = new List<EmpRoleViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.EmpRoles
                                            .Select(c => new EmpRoleViewModel
                                            {
                                               RoleName = c.RoleName,
                                               EmpRoleId = c.EmpRoleId
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
    }
}
