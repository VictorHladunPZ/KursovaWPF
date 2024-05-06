using KursovaWPF.MVVM.CoreViewModels;
using KursovaWPF.MVVM.ViewModels;
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
    /// Interaction logic for ContracteeUserControl.xaml
    /// </summary>
    public partial class ContracteeUserControl : UserControl
    {
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public List<String> PropList = new List<string> { "ContracteeId", "Name", "ContactInformation"};
        public ContracteeViewModel SelectedContractee { get; set; }
        public ContracteeUserControl()
        {
            InitializeComponent();

            SearchCombo.ItemsSource = PropList;
        }

        private void Pagination_First_Click(object sender, RoutedEventArgs e)
        {
            ContracteeDataVM vm = (ContracteeDataVM)DataContext;
            vm.Navigate((int)PagingMode.First);
            contracteeDataGrid.ItemsSource = vm.ContracteesPagination;
        }

        private void Pagination_Prev_Click(object sender, RoutedEventArgs e)
        {
            ContracteeDataVM vm = (ContracteeDataVM)DataContext;
            vm.Navigate((int)PagingMode.Previous);
            contracteeDataGrid.ItemsSource = vm.ContracteesPagination;
        }

        private void Pagination_Next_Click(object sender, RoutedEventArgs e)
        {
            ContracteeDataVM vm = (ContracteeDataVM)DataContext;
            vm.Navigate((int)PagingMode.Next);
            contracteeDataGrid.ItemsSource = vm.ContracteesPagination;
        }

        private void Pagination_Last_Click(object sender, RoutedEventArgs e)
        {
            ContracteeDataVM vm = (ContracteeDataVM)DataContext;
            vm.Navigate((int)PagingMode.Last);
            contracteeDataGrid.ItemsSource = vm.ContracteesPagination;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (null == SearchCombo.SelectedItem)
            {
                MessageBox.Show("You didn't select any field to search by!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string field = SearchCombo.SelectedItem.ToString();
                string value = SearchTextBox.Text;
                ContracteeDataVM vm = (ContracteeDataVM)DataContext;

                List<ContracteeViewModel> list = vm.Search(field, value);
                contracteeDataGrid.ItemsSource = list.OfType<ContracteeViewModel>().ToList();
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
            ContracteeDataVM vM = (ContracteeDataVM)DataContext;
            vM.openEditWindow();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ContracteeDataVM vM = (ContracteeDataVM)DataContext;
            vM.openAddWindow();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ContracteeDataVM vM = (ContracteeDataVM)DataContext;
            vM.DeleteContract();
        }


        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ContracteeDataVM vM = (ContracteeDataVM)DataContext;

            vM.LoadContractees();
            contracteeDataGrid.ItemsSource = vM.ContracteesPagination;
        }
    }
}
