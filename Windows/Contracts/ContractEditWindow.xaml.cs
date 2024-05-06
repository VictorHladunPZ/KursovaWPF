using KursovaWPF.MVVM.CoreViewModels;
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
    /// Interaction logic for ContractEditWindow.xaml
    /// </summary>
    public partial class ContractEditWindow : Window
    {
        private ContractViewModel _contractViewModel;

        public ContractViewModel ContractViewModel
        {
            get { return _contractViewModel; }
            set { _contractViewModel = value; }
        }

        private List<string> ContractStatuses = ["Unpaid", "Paid", "Completed"];
        public ContractEditWindow()
        {
            InitializeComponent();
        }
        public ContractEditWindow(ContractViewModel viewModel)
        {
            InitializeComponent();
            DataContext = System.Windows.Application.Current.MainWindow.DataContext;
            StatusComboBox.ItemsSource = ContractStatuses;
            ContractViewModel = viewModel;
            OwnerNameTextBox.Text = viewModel.Contractee;
            DescriptionTextBox.Text = viewModel.Description.ToString();
            StartDatePicker.Text = viewModel.StartDate.ToString();
            DeadlinePicker.Text = viewModel.Deadline.ToString();
            if(viewModel.Status.Contains("Unpaid")) StatusComboBox.SelectedIndex = 0;
            if (viewModel.Status.Contains("Paid")) StatusComboBox.SelectedIndex = 1;
            if (viewModel.Status.Contains("Completed")) StatusComboBox.SelectedIndex = 2;
            CostTextBox.Text = viewModel.Cost.ToString();
        }

       

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ContractViewModel.Contractee = OwnerNameTextBox.Text.ToString();
            ContractViewModel.Description = DescriptionTextBox.Text.ToString();
            ContractViewModel.StartDate = DateOnly.Parse(StartDatePicker.Text.ToString());
            ContractViewModel.Deadline = DateOnly.Parse(DeadlinePicker.Text.ToString()) ;
            ContractViewModel.Status = StatusComboBox.SelectedItem.ToString();
            ContractViewModel.Cost = decimal.Parse(CostTextBox.Text.ToString());
            MessageBox.Show(ContractRepository.EditContract(ContractViewModel), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
