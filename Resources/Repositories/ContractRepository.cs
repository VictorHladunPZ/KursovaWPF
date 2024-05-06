using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace KursovaWPF.Resources.Repositories
{
    public static class ContractRepository
    {
        #region Window Methods
        public static void OpenEditContractWindow(ContractViewModel selectedContract)
        {
            ContractEditWindow newWindow = new ContractEditWindow(selectedContract);
            SetupWindow(newWindow);
        }
        public static void OpenAddContractWindow()
        {
            AddContractWindow newWindow = new AddContractWindow();
            SetupWindow(newWindow);
        }
        private static void SetupWindow(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
        #endregion
        #region Contract Methods
        public static List<ContractViewModel> LoadContracts()
        {
            List<ContractViewModel> list = new List<ContractViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.Contracts
                                            .Select(c => new ContractViewModel
                                            {
                                                ContractId = c.ContractId,
                                                Contractee = c.Owner.Name,
                                                Description = c.Description,
                                                StartDate = (DateOnly)c.TimeTable.StartDate,
                                                Deadline = (DateOnly)c.TimeTable.Deadline,
                                                Status = c.Status.StatusName,
                                                Cost = (decimal)c.Cost
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

        public static string CreateContract(ContractViewModel searchContr)
        {
            string result = "Already exists";

            using (MainDBContext context = new MainDBContext())
            {
                var contracteeCheckList = LoadContracts().Where(c => c.Contractee.Trim().Equals(searchContr.Contractee.Trim()));
                //check if exists
                bool isExisting = contracteeCheckList.Any(c => c.Description.Trim().Equals(searchContr.Description.Trim()));
                if (!isExisting)
                {
                    Contract newContract = new Contract();
                    newContract.Cost = searchContr.Cost;
                    newContract.Description = searchContr.Description;
                    newContract.Owner = context.Contractees.Where(c => c.Name == searchContr.Contractee).FirstOrDefault();
                    newContract.Status = context.ContractsStatusTables.Where(c => c.StatusName == searchContr.Status).FirstOrDefault();
                    var timeTableList = context.ContractsTimeTables.ToList();
                    
                    var foundTimeTable = timeTableList.Where(c => c.StartDate.Equals(searchContr.StartDate)).Where(c => c.Deadline.Equals(searchContr.Deadline)).FirstOrDefault(); ;
                    if (foundTimeTable != null)
                    {
                        newContract.TimeTable = foundTimeTable;
                    }
                    else
                    {
                        ContractsTimeTableRepository.CreateTimeTableContracts(searchContr.StartDate, searchContr.Deadline);
                        timeTableList = context.ContractsTimeTables.ToList();
                        foundTimeTable = timeTableList.Where(c => c.StartDate.Equals(searchContr.StartDate)).Where(c => c.Deadline.Equals(searchContr.Deadline)).FirstOrDefault();
                        newContract.TimeTable = foundTimeTable;
                    }
                    context.Contracts.Add(newContract);
                    context.SaveChanges();
                    result = "Added successfully";
                }
                Logger.LogAction(
                    string.Format(
                        "User {0} attempted to create contract",
                        MainDBContext.Login
                        )
                    );
            }
            return result;
        }
        public static string DeleteContract(ContractViewModel delContr)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = context.Contracts.Any(cntr => cntr.ContractId == delContr.ContractId);
                if (isExisting)
                {
                    var ContractId = context.Contracts.Where(cntr => cntr.ContractId == delContr.ContractId).First().ContractId;
                    context.Contracts.Remove(context.Contracts.Where(cntr => cntr.ContractId == delContr.ContractId).First());
                    context.SaveChanges();
                    result = "Deleted contract " + ContractId + " successfully!";
                }
            }
            Logger.LogAction(
                    string.Format(
                        "User {0} attempted to delete contract",
                        MainDBContext.Login
                        )
                    );

            return result;
        }
        public static string EditContract(ContractViewModel editCntr)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                Contract EntryExisting = context.Contracts.FirstOrDefault(cntr => cntr.ContractId == editCntr.ContractId);
                EntryExisting.Status = ContractsStatusRepository.GetByName(editCntr.Status);
                var TimeTableFound = ContractsTimeTableRepository.GetById(EntryExisting.TimeTableId);
                TimeTableFound.StartDate = editCntr.StartDate;
                TimeTableFound.Deadline = editCntr.Deadline;
                EntryExisting.Description = editCntr.Description;
                EntryExisting.Cost = editCntr.Cost;
                EntryExisting.Owner = ContracteeRepository.GetByContractName(editCntr.Contractee);
                context.SaveChanges();
                result = "Successfully edited contract";
            }
            Logger.LogAction(
                    string.Format(
                        "User {0} attempted to edit contract",
                        MainDBContext.Login
                        )
                    );
            return result;
        }
        #endregion
    }
}
