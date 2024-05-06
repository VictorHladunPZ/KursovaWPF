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
    public static class DemandsRepository
    {
        public static List<ProdDemandViewModel> LoadDemands()
        {
            List<ProdDemandViewModel> list = new List<ProdDemandViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.ProdDemands
                                            .Select(c => new ProdDemandViewModel
                                            {
                                                Demand = c.Demand,
                                                Product = c.Product.Name,
                                                DemandsId = c.DemandsId,
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
        public static string AddDemand(ProdDemandViewModel ViewModel)
        {
            string result = "Already exists";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                bool isExisting = LoadDemands().Any(c => c.Demand.Trim().Equals(ViewModel.Demand.Trim()));
                if (!isExisting)
                {
                    ProdDemand prodDemand = new ProdDemand();
                    prodDemand.ProductId = context.Products.Where(p => p.Name.Trim().Equals(ViewModel.Product.Trim())).FirstOrDefault().ProductId;
                    prodDemand.Demand = ViewModel.Demand;
                    context.ProdDemands.Add(prodDemand);
                    context.SaveChanges();
                    result = "Added successfully";
                }
            }
            Logger.LogAction(
                   string.Format(
                       "User {0} attempted to add a Product Demand",
                       MainDBContext.Login
                       )
                   );
            return result;
        }
        public static string RemoveDemand(ProdDemandViewModel ViewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = LoadDemands().Any(c => c.DemandsId.Equals(ViewModel.DemandsId));
                if (isExisting)
                {
                    context.ProdDemands.Remove(context.ProdDemands.Where(cntr => cntr.DemandsId == ViewModel.DemandsId).First());
                    context.SaveChanges();
                    result = "Deleted successfully!";
                }
            }
            Logger.LogAction(
                   string.Format(
                       "User {0} attempted to delete a Product Demand",
                       MainDBContext.Login
                       )
                   );
            return result;
        }
        public static string EditDemand(ProdDemandViewModel ViewModel)
        {
            string result = "Not found";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                ProdDemand EntryExisting = context.ProdDemands.FirstOrDefault(cntr => cntr.DemandsId == ViewModel.DemandsId);
                EntryExisting.Demand = ViewModel.Demand;
                EntryExisting.ProductId = ProductsRepository.GetByName(ViewModel.Product).ProductId;
                context.SaveChanges();
                result = "Successfully edited contract";
            }
            Logger.LogAction(
                   string.Format(
                       "User {0} attempted to edit a Product Demand",
                       MainDBContext.Login
                       )
                   );
            return result;
        }

        public static void OpenAddWindow()
        {
            DemandAddWindow newWindow = new DemandAddWindow();
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
