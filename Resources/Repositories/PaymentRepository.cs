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
    public static class PaymentRepository
    {
        #region Window Methods
        public static void OpenAddWindow()
        {
            PaymentsAddWindow newWindow = new PaymentsAddWindow();
            SetupWindow(newWindow);
        }
        private static void SetupWindow(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
        #endregion
        public static Decimal GetContractSum(int ContractId)
        {
            using (MainDBContext context = new MainDBContext())
            {
                return context.Payments.Where(p => p.ContractId == ContractId).Select(p => p.Amount).Sum();
            }
        }
        public static List<PaymentsViewModel> LoadPayments()
        {
            List<PaymentsViewModel> list = new List<PaymentsViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.Payments
                                            .Select(c => new PaymentsViewModel
                                            {
                                               ContractId = c.ContractId,
                                               Amount = c.Amount,
                                               Date = c.Date,
                                               PaymentId = c.PaymentId,
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
        public static string AddPayment(PaymentsViewModel ViewModel)
        {
            string result = "Already exists";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists

                Payment newPayment = new Payment();
                newPayment.ContractId = ViewModel.ContractId;
                newPayment.Amount = ViewModel.Amount;
                newPayment.Date = ViewModel.Date;
                context.Payments.Add(newPayment);
                context.SaveChanges();
                result = "Added successfully";

            }
            Logger.LogAction(
                  string.Format(
                      "User {0} attempted to add a Contract Payment",
                      MainDBContext.Login
                      )
                  );
            return result;
        }
        public static string RemovePayment(PaymentsViewModel ViewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = LoadPayments().Any(c => c.PaymentId.Equals(ViewModel.PaymentId));
                if (isExisting)
                {
                    context.Payments.Remove(context.Payments.Where(cntr => cntr.PaymentId == ViewModel.PaymentId).First());
                    context.SaveChanges();
                    result = "Deleted successfully!";
                }
            }
            Logger.LogAction(
                 string.Format(
                     "User {0} attempted to remove a Contract Payment",
                     MainDBContext.Login
                     )
                 );
            return result;
        }
        public static string EditPayment(PaymentsViewModel ViewModel)
        {
            string result = "Not found";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                Payment EntryExisting = context.Payments.FirstOrDefault(cntr => cntr.PaymentId == ViewModel.PaymentId);
                EntryExisting.Date = ViewModel.Date;
                EntryExisting.Amount = ViewModel.Amount;
                EntryExisting.ContractId = ViewModel.ContractId;
                context.SaveChanges();
                result = "Successfully edited contract";
            }
            Logger.LogAction(
                 string.Format(
                     "User {0} attempted to edit a Contract Payment",
                     MainDBContext.Login
                     )
                 );
            return result;
        }
    }
}
