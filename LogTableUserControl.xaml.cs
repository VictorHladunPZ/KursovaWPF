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
    /// Interaction logic for LogTableUserControl.xaml
    /// </summary>
    public partial class LogTableUserControl : UserControl
    {
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public LogTableUserControl()
        {
            InitializeComponent();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            LogDataVM vM = (LogDataVM)DataContext;
            if (floorPicker.SelectedDate == null
                || ceilingPicker.SelectedDate == null
                )
            {
                MessageBox.Show("No input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var list = vM.Filter(
                DateOnly.FromDateTime((DateTime)floorPicker.SelectedDate),
                DateOnly.FromDateTime((DateTime)ceilingPicker.SelectedDate)
                );
                displayGrid.ItemsSource = list;
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LogDataVM vM = (LogDataVM)DataContext;

            vM.Load();
            displayGrid.ItemsSource = vM.Pagination;
        }

        private void Pagination_First_Click(object sender, RoutedEventArgs e)
        {
            LogDataVM vm = (LogDataVM)DataContext;
            vm.Navigate((int)PagingMode.First);
        }

        private void Pagination_Prev_Click(object sender, RoutedEventArgs e)
        {
            LogDataVM vm = (LogDataVM)DataContext;
            vm.Navigate((int)PagingMode.Previous);
        }

        private void Pagination_Next_Click(object sender, RoutedEventArgs e)
        {
            LogDataVM vm = (LogDataVM)DataContext;
            vm.Navigate((int)PagingMode.Next);
        }

        private void Pagination_Last_Click(object sender, RoutedEventArgs e)
        {
            LogDataVM vm = (LogDataVM)DataContext;
            vm.Navigate((int)PagingMode.Last);
        }
    }
}
