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
    /// Interaction logic for AddContractWindow.xaml
    /// </summary>
    public partial class AddContractWindow : Window
    {
        private ContractViewModel _contractViewModel;

        public ContractViewModel ContractViewModel
        {
            get { return _contractViewModel; }
            set { _contractViewModel = value; }
        }
        public AddContractWindow()
        {
            InitializeComponent();
            StatusComboBox.ItemsSource = ContractsStatusRepository.GetStatusNames();
            ContractViewModel = new ContractViewModel();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ContractViewModel.Contractee = OwnerNameTextBox.Text.ToString();
            ContractViewModel.Description = DescriptionTextBox.Text.ToString();
            ContractViewModel.StartDate = DateOnly.Parse(StartDatePicker.Text.ToString());
            ContractViewModel.Deadline = DateOnly.Parse(DeadlinePicker.Text.ToString());
            ContractViewModel.Status = StatusComboBox.SelectedItem.ToString();
            ContractViewModel.Cost = decimal.Parse(CostTextBox.Text.ToString());
            MessageBox.Show(ContractRepository.CreateContract(ContractViewModel), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
