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
    /// Interaction logic for ProjectEditWindow.xaml
    /// </summary>
    public partial class ProjectEditWindow : Window
    {
        public ProjectViewModel viewModel;
        public ProjectEditWindow()
        {
            InitializeComponent();
        }
        public ProjectEditWindow(ProjectViewModel model)
        {
            InitializeComponent();
            viewModel = model;
            StatusCheckbox.IsChecked  = viewModel.Status.Equals("Completed") ? true : false;
            DeadlinePicker.Text = viewModel.Deadline.ToString();
            StartDatePicker.Text = viewModel.StartDate.ToString();
            OwnerNameTextBox.Text = viewModel.ProductName ;
            ProductTextBox.Text = viewModel.Title;
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Status = (bool)StatusCheckbox.IsChecked ? "Completed" : "Uncompleted";
            viewModel.Deadline = DateOnly.Parse(DeadlinePicker.Text.ToString());
            viewModel.StartDate = DateOnly.Parse(StartDatePicker.Text.ToString());
            viewModel.ProductName = OwnerNameTextBox.Text.ToString();
            viewModel.Title = ProductTextBox.Text.ToString();

            MessageBox.Show(ProjectRepository.EditProject(viewModel), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
