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
    public static class LogRepository
    {
        internal static List<LogViewModel> Load()
        {
            List<LogViewModel> list = new List<LogViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.LogTables
                                            .Select(c => new LogViewModel
                                            {
                                                LogId = c.LogId,
                                                Message = c.Message,
                                                Timestamp = c.Timestamp,
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
