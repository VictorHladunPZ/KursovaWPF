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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace KursovaWPF
{
    /// <summary>
    /// Interaction logic for TeamsUserControl.xaml
    /// </summary>
    public partial class TeamsUserControl : UserControl
    {
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public TeamsUserControl()
        {
            InitializeComponent();
            EmpTeamDataVM viewModel = new EmpTeamDataVM();
            DataContext = viewModel;
        }



        private void Pagination_First_Click(object sender, RoutedEventArgs e)
        {
            EmpTeamDataVM vm = (EmpTeamDataVM)DataContext;
            vm.Navigate((int)PagingMode.First);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void Pagination_Prev_Click(object sender, RoutedEventArgs e)
        {
            EmpTeamDataVM vm = (EmpTeamDataVM)DataContext;
            vm.Navigate((int)PagingMode.Previous);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void Pagination_Next_Click(object sender, RoutedEventArgs e)
        {
            EmpTeamDataVM vm = (EmpTeamDataVM)DataContext;
            vm.Navigate((int)PagingMode.Next);
            contractDataGrid.ItemsSource = vm.Pagination;
        }

        private void Pagination_Last_Click(object sender, RoutedEventArgs e)
        {
            EmpTeamDataVM vm = (EmpTeamDataVM)DataContext;
            vm.Navigate((int)PagingMode.Last);
            contractDataGrid.ItemsSource = vm.Pagination;
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            EmpTeamDataVM vM = (EmpTeamDataVM)DataContext;
            vM.openAddWindow();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            EmpTeamDataVM vM = (EmpTeamDataVM)DataContext;
            vM.Delete();
        }
    }
}
