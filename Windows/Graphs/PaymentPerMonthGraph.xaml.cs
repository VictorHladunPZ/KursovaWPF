using KursovaWPF.MVVM.CoreViewModels;
using KursovaWPF.MVVM.ViewModels;
using KursovaWPF.Resources.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Windows;
using System.Windows.Controls;

namespace KursovaWPF
{
    /// <summary>
    /// Interaction logic for PaymentPerMonthGraph.xaml
    /// </summary>
    public partial class PaymentPerMonthGraph : UserControl
    {
        public PaymentPerMonthGraph()
        {
            this.DataContext = new PaymentGraphDataVM();
            InitializeComponent();

        }

        private void GraphButton_Click(object sender, RoutedEventArgs e)
        {
            if(StartDatePicker.Text.ToString().IsNullOrEmpty() || DeadLinePicker.Text.ToString().IsNullOrEmpty())
            {
                MessageBox.Show("No input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                PaymentGraphDataVM vm = (PaymentGraphDataVM)this.DataContext;
                DateOnly floor = DateOnly.Parse(StartDatePicker.Text.ToString());
                DateOnly ceiling = DateOnly.Parse(DeadLinePicker.Text.ToString());
                List<PaymentsViewModel> list = PaymentRepository.LoadPayments().Where(p => p.Date < ceiling).Where(p => p.Date > floor).ToList();
                vm.CalculatePaymentsByMonth(list);
            }
        }
    }
}
