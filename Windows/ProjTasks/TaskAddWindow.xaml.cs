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
    /// Interaction logic for TaskAddWindow.xaml
    /// </summary>
    public partial class TaskAddWindow : Window
    {
        public TaskAddWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ProjTaskViewModel taskViewModel = new ProjTaskViewModel();
            taskViewModel.Description = DescriptionTextBox.Text.ToString();
            taskViewModel.ProjectId = ProjectRepository.GetByName(OwnerNameTextBox.Text.ToString()).ProjectId;
            ProjectTasksRepository.AddTask(taskViewModel);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
