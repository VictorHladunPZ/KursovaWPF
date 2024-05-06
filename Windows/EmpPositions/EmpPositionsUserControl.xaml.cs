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
    /// Interaction logic for EmpPositionsUserControl.xaml
    /// </summary>
    public partial class EmpPositionsUserControl : UserControl
    {
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public EmpPositionsUserControl()
        {
            InitializeComponent();
            EmpPositionDataVM viewModel = new EmpPositionDataVM();
            DataContext = viewModel;
        }

        private void Pagination_First_Click(object sender, RoutedEventArgs e)
        {
            EmpPositionDataVM vm = (EmpPositionDataVM)DataContext;
            vm.Navigate((int)PagingMode.First);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void Pagination_Prev_Click(object sender, RoutedEventArgs e)
        {
            EmpPositionDataVM vm = (EmpPositionDataVM)DataContext;
            vm.Navigate((int)PagingMode.Previous);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void Pagination_Next_Click(object sender, RoutedEventArgs e)
        {
            EmpPositionDataVM vm = (EmpPositionDataVM)DataContext;
            vm.Navigate((int)PagingMode.Next);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void Pagination_Last_Click(object sender, RoutedEventArgs e)
        {
            EmpPositionDataVM vm = (EmpPositionDataVM)DataContext;
            vm.Navigate((int)PagingMode.Last);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

    }
}
