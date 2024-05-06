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
    /// Interaction logic for ProjectAddWindow.xaml
    /// </summary>
    public partial class ProjectAddWindow : Window
    {
        public ProjectViewModel viewModel = new ProjectViewModel();
        public ProjectAddWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Status = (bool)StatusCheckbox.IsChecked ? "Completed" : "Uncompleted";
            viewModel.Deadline = DateOnly.Parse(DeadlinePicker.Text.ToString());
            viewModel.StartDate = DateOnly.Parse(StartDatePicker.Text.ToString());
            viewModel.ProductName = OwnerNameTextBox.Text.ToString();
            viewModel.Title = ProductTextBox.Text.ToString();
            MessageBox.Show(ProjectRepository.AddProject(viewModel), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel = null;
            this.Close();
        }
    }
}
