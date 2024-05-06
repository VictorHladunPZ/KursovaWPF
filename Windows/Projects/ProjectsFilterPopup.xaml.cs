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
    /// Interaction logic for ProjectsFilterPopup.xaml
    /// </summary>
    public partial class ProjectsFilterPopup : Window
    {
        public bool ByStart = false, ByEnd = false;
        public DateOnly StartCeiling,StartFloor,DeadCeiling,DeadFloor;

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ByStart = (bool)ByStartDate.IsChecked;
            ByEnd = (bool)ByDeadline.IsChecked;
            if (ByStart)
            {
                StartCeiling = DateOnly.Parse(HigherStartDatePicker.Text.ToString());
                StartFloor = DateOnly.Parse(LowerStartDatePicker.Text.ToString());
            }
            if (ByEnd)
            {
                DeadCeiling = DateOnly.Parse(HigherDeadlinePicker.Text.ToString());
                DeadFloor = DateOnly.Parse(LowerDeadlinePicker.Text.ToString());
            }
            this.Close();
        }

        public ProjectsFilterPopup()
        {
            InitializeComponent();
        }
    }
}
