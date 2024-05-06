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
    public static class ContractsStatusRepository
    {
        public static ContractsStatusTable GetByName(string statusName)
        {
            using (MainDBContext context = new MainDBContext())
            {
                return context.ContractsStatusTables.Where(c => c.StatusName == statusName).FirstOrDefault();
            }
        }
        public static List<ContractStatusViewModel> LoadStatuses()
        {
            List<ContractStatusViewModel> list = new List<ContractStatusViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.ContractsStatusTables
                                            .Select(c => new ContractStatusViewModel
                                            {
                                                ContractStatusId = c.ContractStatusId,
                                                StatusName = c.StatusName,
                                            })
,
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
        public static List<string> GetStatusNames()
        {
            return LoadStatuses().Select(s => s.StatusName).ToList();
        }
    }
}
