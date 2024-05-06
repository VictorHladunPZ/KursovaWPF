using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KursovaWPF.Resources.Repositories
{
    public static class ContracteeRepository
    {
        #region normal methods
        public static Contractee GetByContractId(int contracteeId)
        {
            using (MainDBContext context = new MainDBContext())
            {
               return context.Contractees.Where(c => c.ContracteeId == contracteeId).FirstOrDefault();
            }
        }
        public static Contractee GetByContractName(string contractName)
        {
            using (MainDBContext context = new MainDBContext())
            {
               return context.Contractees.Where(c => c.Name == contractName).FirstOrDefault();
            }
        }

        public static List<ContracteeViewModel> LoadContractees()
        {
            List<ContracteeViewModel> list = new List<ContracteeViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.Contractees
                                            .Select(c => new ContracteeViewModel
                                            {
                                                ContactInformation = c.ContactInformation,
                                                ContracteeId = c.ContracteeId,
                                                Name = c.Name,
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
       
        public static string EditContractee(ContracteeViewModel editVM)
        {
            string result = "Not found";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                Contractee EntryExisting = context.Contractees.FirstOrDefault(cntr => cntr.ContracteeId == editVM.ContracteeId);
                EntryExisting.ContactInformation = editVM.ContactInformation;
                EntryExisting.Name = editVM.Name;
                context.SaveChanges();
                result = "Successfully edited contract";
            }
            Logger.LogAction(
                   string.Format(
                       "User {0} attempted to edit a Contractee",
                       MainDBContext.Login
                       )
                   );
            return result;
        }
        public static string DeleteContractee(ContracteeViewModel deleteVM)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = context.Contractees.Any(cntr => cntr.ContracteeId == deleteVM.ContracteeId);
                if (isExisting)
                {
                    context.Contractees.Remove(context.Contractees.Where(cntr => cntr.ContracteeId == deleteVM.ContracteeId).First());
                    context.SaveChanges();
                    result = "Deleted successfully!";
                }
            }
            Logger.LogAction(
                   string.Format(
                       "User {0} attempted to delete a Contractee",
                       MainDBContext.Login
                       )
                   );
            return result;
        }
        public static string AddContractee(ContracteeViewModel addVM)
        {
            string result = "Already exists";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                bool isExisting = LoadContractees().Any(c => c.Name.Trim().Equals(addVM.Name.Trim()));
                if (!isExisting)
                {
                    Contractee newContractee = new Contractee();
                    newContractee.ContactInformation = addVM.ContactInformation;
                    newContractee.Name = addVM.Name;

                    context.Contractees.Add(newContractee);
                    context.SaveChanges();
                    result = "Added successfully";
                }
            }
            Logger.LogAction(
                   string.Format(
                       "User {0} attempted to add a Contractee",
                       MainDBContext.Login
                       )
                   );
            return result;
        }

        #endregion
        #region Window Methods
        public static void OpenEditContracteeWindow(ContracteeViewModel selectedContract)
        {
            EditContracteeWindow newWindow = new EditContracteeWindow(selectedContract);
            SetupWindow(newWindow);
        }
        public static void OpenAddContracteeWindow()
        {
            AddContracteeWindow newWindow = new AddContracteeWindow();
            SetupWindow(newWindow);
        }
        private static void SetupWindow(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
        #endregion
    }
}
