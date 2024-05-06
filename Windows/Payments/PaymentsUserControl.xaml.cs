using KursovaWPF.MVVM.CoreViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KursovaWPF
{
    /// <summary>
    /// Interaction logic for PaymentsUserControl.xaml
    /// </summary>
    public partial class PaymentsUserControl : UserControl
    {
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public PaymentsUserControl()
        {
            InitializeComponent();
            PaymentDataVM viewModel = new PaymentDataVM();
            DataContext = viewModel;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            PaymentDataVM vM = (PaymentDataVM)DataContext;
            var list = vM.Filter();
            if (list.Count > 0)
            {
                MessageBox.Show("Successfully applied filter!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Filter didn't find any entires. Please check if the inputted filter is correct!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            contractDataGrid.ItemsSource = list;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            PaymentDataVM vM = (PaymentDataVM)DataContext;

            vM.Load();
            contractDataGrid.ItemsSource = vM.Pagination;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            PaymentDataVM vM = (PaymentDataVM)DataContext;
            vM.openAddWindow();
        }

        private void Pagination_First_Click(object sender, RoutedEventArgs e)
        {
            PaymentDataVM vm = (PaymentDataVM)DataContext;
            vm.Navigate((int)PagingMode.First);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void Pagination_Prev_Click(object sender, RoutedEventArgs e)
        {
            PaymentDataVM vm = (PaymentDataVM)DataContext;
            vm.Navigate((int)PagingMode.Previous);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void Pagination_Next_Click(object sender, RoutedEventArgs e)
        {
            PaymentDataVM vm = (PaymentDataVM)DataContext;
            vm.Navigate((int)PagingMode.Next);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void Pagination_Last_Click(object sender, RoutedEventArgs e)
        {
            PaymentDataVM vm = (PaymentDataVM)DataContext;
            vm.Navigate((int)PagingMode.Last);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            PaymentDataVM vM = (PaymentDataVM)DataContext;
            vM.DeletePayment();
        }
    }
}
