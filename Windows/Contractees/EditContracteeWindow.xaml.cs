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
    /// Interaction logic for EditContracteeWindow.xaml
    /// </summary>
    public partial class EditContracteeWindow : Window
    {
        private ContracteeViewModel selectedContract;

        public EditContracteeWindow()
        {
            InitializeComponent();
        }

        public EditContracteeWindow(ContracteeViewModel selectedContract)
        {
            InitializeComponent();
            this.selectedContract = selectedContract;
            DataContext = System.Windows.Application.Current.MainWindow.DataContext;
            NameTextBox.Text = selectedContract.Name;
            ContactTextBox.Text = selectedContract.ContactInformation;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            selectedContract.Name = NameTextBox.Text;
            selectedContract.ContactInformation = ContactTextBox.Text;

            MessageBox.Show(ContracteeRepository.EditContractee(selectedContract), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
