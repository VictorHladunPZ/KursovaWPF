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
    public static class ProductsRepository
    {
        public static Product GetByName(string productName)
        {
            using (MainDBContext context = new MainDBContext())
            {
                return context.Products.Where(p => p.Name.Trim().Equals(productName.Trim())).FirstOrDefault();
            }
        }
        public static List<ProductViewModel> LoadProducts()
        {
            List<ProductViewModel> list = new List<ProductViewModel>();
            using (MainDBContext dbContext = new MainDBContext())
            {
                try
                {
                    dbContext.Database.OpenConnection();
                    list =
                    [
                        .. dbContext.Products
                                            .Select(c => new ProductViewModel
                                            {
                                                ProductId = c.ProductId,
                                                Name = c.Name,
                                                ContractId = c.ContractId,
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
        public static string AddProduct(ProductViewModel ViewModel)
        {
            string result = "Already exists";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                bool isExisting = LoadProducts().Any(c => c.Name.Equals(ViewModel.Name));
                if (!isExisting)
                {
                    Product Product = new Product();
                    Product.ContractId = ViewModel.ContractId;
                    Product.Name = ViewModel.Name;
                    context.Products.Add(Product);
                    context.SaveChanges();
                    result = "Added successfully";
                }
            }
            Logger.LogAction(
                 string.Format(
                     "User {0} attempted to add a Product",
                     MainDBContext.Login
                     )
                 );
            return result;
        }
        public static string RemoveProduct(ProductViewModel ViewModel)
        {
            string result = "Does not exist";
            using (MainDBContext context = new MainDBContext())
            {
                bool isExisting = LoadProducts().Any(c => c.ProductId.Equals(ViewModel.ProductId));
                if (isExisting)
                {
                    context.Products.Remove(context.Products.Where(cntr => cntr.ProductId == ViewModel.ProductId).First());
                    context.SaveChanges();
                    result = "Deleted successfully!";
                }
            }
            Logger.LogAction(
                string.Format(
                    "User {0} attempted to remove a Product",
                    MainDBContext.Login
                    )
                );
            return result;
        }
        public static string EditProduct(ProductViewModel ViewModel)
        {
            string result = "Not found";

            using (MainDBContext context = new MainDBContext())
            {
                //check if exists
                Product EntryExisting = context.Products.FirstOrDefault(cntr => cntr.ProductId == ViewModel.ProductId);
                EntryExisting.Name = ViewModel.Name;
                EntryExisting.ContractId = ViewModel.ContractId;
                context.SaveChanges();
                result = "Successfully edited contract";
            }
            Logger.LogAction(
                string.Format(
                    "User {0} attempted to edit a Product",
                    MainDBContext.Login
                    )
                );
            return result;
        }
    }
}
