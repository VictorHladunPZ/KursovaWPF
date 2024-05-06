using KursovaWPF.MVVM.CoreViewModels;
using KursovaWPF.MVVM.ViewModels;
using KursovaWPF.Windows;
using System.Windows;
using System.Windows.Controls;

namespace KursovaWPF
{
    /// <summary>
    /// Interaction logic for ContractsUserControl.xaml
    /// </summary>
    public partial class ContractsUserControl : UserControl
    {
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public List<String> PropList = new List<string> { "ContractId", "Contractee", "Description", "StartDate", "Deadline", "Status", "Cost" };
        public ContractsUserControl()
        {
            InitializeComponent();
            SearchCombo.ItemsSource = PropList;
            ContractDataVM viewModel = new ContractDataVM();
            DataContext = viewModel;
        }

        

        private void Pagination_First_Click(object sender, RoutedEventArgs e)
        {
            ContractDataVM vm = (ContractDataVM)DataContext;
            vm.Navigate((int)PagingMode.First);
            contractDataGrid.ItemsSource = vm.ContractsPagination;
        }

        private void Pagination_Prev_Click(object sender, RoutedEventArgs e)
        {
            ContractDataVM vm = (ContractDataVM)DataContext;
            vm.Navigate((int)PagingMode.Previous);

            contractDataGrid.ItemsSource = vm.ContractsPagination;
        }

        private void Pagination_Next_Click(object sender, RoutedEventArgs e)
        {
            ContractDataVM vm = (ContractDataVM)DataContext;
            vm.Navigate((int)PagingMode.Next);

            contractDataGrid.ItemsSource = vm.ContractsPagination;
        }

        private void Pagination_Last_Click(object sender, RoutedEventArgs e)
        {
            ContractDataVM vm = (ContractDataVM)DataContext;
            vm.Navigate((int)PagingMode.Last);

            contractDataGrid.ItemsSource = vm.ContractsPagination;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if(null == SearchCombo.SelectedItem)
            {
                MessageBox.Show("You didn't select any field to search by!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string field = SearchCombo.SelectedItem.ToString();
                string value = SearchTextBox.Text;
                DataGridVM vm = (DataGridVM)DataContext;
                if (field.Equals("Cost"))
                {
                    if (!value.Contains(",")) value += ",00";
                }
                List<object> list = vm.Search(field, value);
                contractDataGrid.ItemsSource = list.OfType<ContractViewModel>().ToList();
                if (list.Count > 0)
                {
                    MessageBox.Show("Successfully applied filter!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Filter didn't find any entires. Please check if the inputted filter is correct!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ContractDataVM vM = (ContractDataVM)DataContext;
            vM.openEditWindow();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ContractDataVM vM = (ContractDataVM)DataContext;
            vM.openAddWindow();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ContractDataVM vM = (ContractDataVM)DataContext;
            vM.DeleteContract();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            ContractDataVM vM = (ContractDataVM)DataContext;
            var list = vM.FilterContract();
            contractDataGrid.ItemsSource = list;
            if (list.Count > 0)
            {
                MessageBox.Show("Successfully applied filter!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Filter didn't find any entires. Please check if the inputted filter is correct!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ContractDataVM vM = (ContractDataVM)DataContext;
            
            vM.LoadContracts();
            contractDataGrid.ItemsSource = vM.ContractsPagination;
        }
    }
}
