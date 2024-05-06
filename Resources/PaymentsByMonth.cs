using KursovaWPF.MVVM.ViewModels;
using KursovaWPF.Resources.Repositories;
namespace KursovaWPF.Resources
{
    public class PaymentsByMonth
    {
        public DateTime Month { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PaymentsByMonth> SortedPayments { get; private set; }
        public void CalculatePaymentsByMonth(List<PaymentsViewModel> payments)
        {
            SortedPayments = payments.GroupBy(p => new { p.Date.Year, p.Date.Month })
                                       .Select(group => new PaymentsByMonth
                                       {
                                           Month = new DateTime(group.Key.Year, group.Key.Month, 1),
                                           TotalAmount = group.Sum(p => p.Amount)
                                       })
                                       .OrderBy(p => p.Month)
                                       .ToList();
        }
        public PaymentsByMonth()
        {
            CalculatePaymentsByMonth(PaymentRepository.LoadPayments());
        }
    }
}
