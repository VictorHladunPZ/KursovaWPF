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
    /// Interaction logic for ProjectsUserControl.xaml
    /// </summary>
    public partial class ProjectsUserControl : UserControl
    {
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public ProjectsUserControl()
        {
            InitializeComponent();
            ProjectDataVM viewModel = new ProjectDataVM();
            DataContext = viewModel;
        }

        private void Pagination_First_Click(object sender, RoutedEventArgs e)
        {
            ProjectDataVM vm = (ProjectDataVM)DataContext;
            vm.Navigate((int)PagingMode.First);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void Pagination_Prev_Click(object sender, RoutedEventArgs e)
        {
            ProjectDataVM vm = (ProjectDataVM)DataContext;
            vm.Navigate((int)PagingMode.Previous);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void Pagination_Next_Click(object sender, RoutedEventArgs e)
        {
            ProjectDataVM vm = (ProjectDataVM)DataContext;
            vm.Navigate((int)PagingMode.Next);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void Pagination_Last_Click(object sender, RoutedEventArgs e)
        {
            ProjectDataVM vm = (ProjectDataVM)DataContext;
            vm.Navigate((int)PagingMode.Last);
            contractDataGrid.ItemsSource = vm.Pagination;
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectDataVM vM = (ProjectDataVM)DataContext;
            vM.openAddWindow();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectDataVM vM = (ProjectDataVM)DataContext;

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
            ProjectDataVM vM = (ProjectDataVM)DataContext;

            vM.Load();
            contractDataGrid.ItemsSource = vM.Pagination;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectDataVM vM = (ProjectDataVM)DataContext;
            vM.openEditWindow();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectDataVM vM = (ProjectDataVM)DataContext;
            vM.Delete();
        }
    }
}
