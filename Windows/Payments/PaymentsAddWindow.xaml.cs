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
    /// Interaction logic for PaymentsAddWindow.xaml
    /// </summary>
    public partial class PaymentsAddWindow : Window
    {
        public PaymentsAddWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            PaymentsViewModel viewModel = new PaymentsViewModel();
            viewModel.ContractId = int.Parse(ContractTextBox.Text.ToString());
            viewModel.Date = DateOnly.Parse(DatePicker.Text.ToString());
            viewModel.Amount = Decimal.Parse(AmountTextBox.Text.ToString());
            MessageBox.Show(PaymentRepository.AddPayment(viewModel), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
