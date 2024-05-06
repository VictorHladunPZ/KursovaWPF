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
    /// Interaction logic for RetrospectiveAddWindow.xaml
    /// </summary>
    public partial class RetrospectiveAddWindow : Window
    {
        public RetrospectiveAddWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectsBacklogViewModel viewModel = new ProjectsBacklogViewModel();
            viewModel.Project = OwnerNameTextBox.Text.ToString();
            viewModel.ProjectRetrospective = DescriptionTextBox.Text.ToString();
            MessageBox.Show(BacklogRepository.AddBacklog(viewModel), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
