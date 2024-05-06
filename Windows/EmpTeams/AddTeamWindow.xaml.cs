using KursovaWPF.MVVM.ViewModels;
using KursovaWPF.Resources.Repositories;
using System.Windows;

namespace KursovaWPF
{
    /// <summary>
    /// Interaction logic for AddTeamWindow.xaml
    /// </summary>
    public partial class AddTeamWindow : Window
    {
        public AddTeamWindow()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            TeamViewModel vm = new TeamViewModel();
            vm.ProjectId = int.Parse(ProjectIdTextBox.Text.ToString());
            vm.TeamName = TeamNameTextBox.Text.ToString();
            MessageBox.Show(TeamsRepository.AddTeam(vm), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
