using KursovaWPF.MVVM.ViewModels;
using LiveCharts;
using KursovaWPF.Resources.Repositories;
using LiveCharts.Wpf;
using KursovaWPF.Resources;
namespace KursovaWPF.MVVM.CoreViewModels
{
    public class PaymentGraphDataVM : ViewModelBase, IExportDataVM
    {
        private List<String> _months;
        public List<String> Months {
            get
            {
                return _months;
            }
            set
            {
                _months = value;
                OnPropertyChanged(nameof(Months));
            }
        }
        public Func<double, string> YFormatter { get; set; }
        private SeriesCollection _PaymentSeries;

        public SeriesCollection PaymentSeries
        {
            get { return _PaymentSeries; }
            set { 
                _PaymentSeries = value;
                OnPropertyChanged(nameof(PaymentSeries));
            }
        }

        private List<decimal> _payments;
        public List<decimal> Payments
        {
            get
            {
                return _payments;
            }
            set
            {
                _payments = value;
                OnPropertyChanged(nameof(Payments));
            }
        }

        public void CalculatePaymentsByMonth(List<PaymentsViewModel> payments)
        {
            var paymentsByMonth = payments.GroupBy(p => new { p.Date.Year, p.Date.Month })
                                          .Select(group => new
                                          {
                                              Month = new DateTime(group.Key.Year, group.Key.Month, 1),
                                              TotalAmount = group.Sum(p => p.Amount)
                                          })
                                          .OrderBy(p => p.Month)
                                          .ToList();

            Months = Months = paymentsByMonth.Select(p => p.Month.ToString("MMM yyyy")).ToList(); 
            Payments = paymentsByMonth.Select(p => p.TotalAmount).ToList();
            var paymentValues = paymentsByMonth.Select(p => p.TotalAmount).ToList();
            PaymentSeries = new SeriesCollection
        {
            new ColumnSeries
            {
                Title = "Total Amount",
                Values = new ChartValues<decimal>(paymentValues)
            }
        };
            YFormatter = value => value.ToString("C");
        }

        public string exportAsCSV()
        {
            return "Cannot export this data";
        }

        public string exportAsXML()
        {
            return "Cannot export this data";
        }

        public string exportAsRDF()
        {
            return "Cannot export this data";
        }

        public string exportAsJSON()
        {
            return "Cannot export this data";
        }

        public PaymentGraphDataVM()
        {
            CalculatePaymentsByMonth(PaymentRepository.LoadPayments());
        }
    }
}
