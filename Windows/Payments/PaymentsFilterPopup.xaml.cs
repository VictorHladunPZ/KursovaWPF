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
    /// Interaction logic for PaymentsFilterPopup.xaml
    /// </summary>
    public partial class PaymentsFilterPopup : Window
    {
        public Decimal AmountCeiling { get; set; }
        public Decimal AmountFloor { get; set; }
        public PaymentsFilterPopup()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            AmountCeiling = Decimal.Parse(AmountHighTextBox.Text.ToString());
            AmountFloor = Decimal.Parse(AmountLowTextBox.Text.ToString());
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
