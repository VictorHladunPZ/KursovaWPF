using KursovaWPF.MVVM.ViewModels;
using KursovaWPF.Resources.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KursovaWPF
{
    /// <summary>
    /// Interaction logic for ProductAddWindow.xaml
    /// </summary>
    public partial class ProductAddWindow : Window
    {
        public ProductAddWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.ContractId = int.Parse(OwnerNameTextBox.Text.ToString());
            viewModel.Name = DescriptionTextBox.Text.ToString();
            ProductsRepository.AddProduct(viewModel);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
