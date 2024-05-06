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
using System.Windows.Shapes;

namespace KursovaWPF.Windows
{
    /// <summary>
    /// Interaction logic for ContractSearchPopup.xaml
    /// </summary>
    public partial class ContractSearchPopup : Window
    {
        private bool filterCost = new();
        private bool filterStart = new();
        private bool filterDeadLine = new();
        private DateOnly filterStartDateCeiling = new();
        private DateOnly filterStartDateFloor = new();
        private DateOnly filterDeadlineCeiling = new();
        private DateOnly filterDeadlineFloor = new();
        private decimal costCeiling = new();
        private decimal costFloor = new();

        public bool FilterCost { get => filterCost; set => filterCost = value; }
        public bool FilterStart { get => filterStart; set => filterStart = value; }
        public bool FilterDeadLine { get => filterDeadLine; set => filterDeadLine = value; }
        public DateOnly FilterStartDateCeiling { get => filterStartDateCeiling; set => filterStartDateCeiling = value; }
        public DateOnly FilterStartDateFloor { get => filterStartDateFloor; set => filterStartDateFloor = value; }
        public DateOnly FilterDeadlineCeiling { get => filterDeadlineCeiling; set => filterDeadlineCeiling = value; }
        public DateOnly FilterDeadlineFloor { get => filterDeadlineFloor; set => filterDeadlineFloor = value; }
        public Decimal CostCeiling { get => costCeiling; set => costCeiling = value; }
        public Decimal CostFloor { get => costFloor; set => costFloor = value; }
        public ContractSearchPopup()
        {
            InitializeComponent();
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            filterCost = (bool)CostCheckBox.IsChecked;
            filterStart = (bool)StartDateCheckBox.IsChecked;
            filterDeadLine = (bool)DeadLineCheckBox.IsChecked;

            if (filterCost)
            {
                CostCeiling = Decimal.Parse(CostCeilingTextBox.Text.ToString());
                CostFloor = Decimal.Parse(CostFloorTextBox.Text.ToString());
            }
            if (filterStart)
            {
                FilterStartDateCeiling = DateOnly.Parse(StartDateCeilingPicker.Text.ToString());
                FilterStartDateFloor = DateOnly.Parse(StartDateFloorPicker.Text.ToString());
            }
            if (filterDeadLine)
            {
                FilterDeadlineCeiling = DateOnly.Parse(DeadlineCeilingPicker.Text.ToString());
                FilterDeadlineFloor = DateOnly.Parse(DeadlineFloorPicker.Text.ToString());
            }
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
