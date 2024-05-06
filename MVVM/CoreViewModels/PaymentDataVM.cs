using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using KursovaWPF.Resources;
using KursovaWPF.Resources.Repositories;
using KursovaWPF.Windows;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using VDS.RDF.Writing;
using VDS.RDF;
using System.Text.Json;
using System.Windows;

namespace KursovaWPF.MVVM.CoreViewModels
{
    public class PaymentDataVM : ViewModelBase, IExportDataVM
    {
        public List<String> PropList = new List<string> { "Id", "Contractee", "Description", "StartDate", "Deadline", "Status", "Cost" };
        private string _currentFilter;

        public string currentFilter
        {
            get { return _currentFilter; }
            set
            {
                _currentFilter = value;
                OnPropertyChanged(nameof(currentFilter));
            }
        }
        public ObservableCollection<PaymentsViewModel> Payments { get; set; }
        private ObservableCollection<PaymentsViewModel> _Pagination;
        public PaymentsViewModel Selected { get; set; }
        public ObservableCollection<PaymentsViewModel> Pagination
        {
            get { return _Pagination; }
            set
            {
                _Pagination = value;
                OnPropertyChanged(nameof(Pagination));
            }
        }
        public PaymentDataVM()
        {
            currentFilter = "None";
            Load();
        }
        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public void Load()
        {
            Payments = new ObservableCollection<PaymentsViewModel>(PaymentRepository.LoadPayments());
            Navigate(((int)PagingMode.First));
        }



        public void Navigate(int mode)
        {
            List<PaymentsViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:

                    if (Payments.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (Payments.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = Payments.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();

                        }
                        else
                        {
                            returnList = Payments.Skip(pageIndex *
                            numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                            pageIndex++;
                        }
                    }

                    break;
                case (int)PagingMode.Previous:
                    if (pageIndex > 1)
                    {
                        pageIndex -= 1;
                        if (pageIndex == 1)
                        {
                            returnList = Payments.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = Payments.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (Payments.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) Pagination = new ObservableCollection<PaymentsViewModel>(returnList);
        }

        List<PaymentsViewModel> Search(string field, string value)
        {
            List<PaymentsViewModel> returnList = new List<PaymentsViewModel>();
            foreach (PaymentsViewModel contract in Payments)
            {
                // Check if the current contract matches the search criteria
                if (contract.Search(field, value))
                {
                    // If it matches, add it to the list of matching contracts
                    returnList.Add(contract);
                }
            }
            return returnList;
        }

        public void openAddWindow()
        {
            PaymentRepository.OpenAddWindow();
            Load();
        }
        public void DeletePayment()
        {
            if (Selected != null)
            {
                MessageBoxResult result = MessageBox.Show("This will delete the payment without any way to undo. Do you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show(PaymentRepository.RemovePayment(Selected), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Load();
                }
                
            }
            Load();
        }
        public List<PaymentsViewModel> Filter()
        {
            PaymentsFilterPopup window = new PaymentsFilterPopup();
            window.ShowDialog();
            List<PaymentsViewModel> list = this.Payments.ToList();
            currentFilter = string.Empty;
            list = list.Where(x => x.Amount <= window.AmountCeiling).Where(x => x.Amount >= window.AmountFloor).ToList();
            currentFilter += " Amount: Between " + window.AmountFloor + " and " + window.AmountCeiling;
            if (String.IsNullOrEmpty(currentFilter)) currentFilter = "None";
            return list;
        }

        public string exportAsCSV()
        {
            var filePath = "payments.csv";
            // Open a stream writer to write to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Write the CSV header
                    csv.WriteHeader<PaymentsViewModel>();
                    csv.NextRecord();

                    // Write each contract to the CSV file
                    foreach (var contract in Payments)
                    {
                        csv.WriteRecord(contract);
                        csv.NextRecord();
                    }
                }
            }
            return "Successfully created a CSV file!";
        }

        public string exportAsXML()
        {
            var filePath = "payments.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<PaymentsViewModel>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<PaymentsViewModel> contractList = new List<PaymentsViewModel>();
                contractList.AddRange(Payments);
                serializer.Serialize(writer, contractList);
            }
            return "Successfully created a XML file!";
        }

        public string exportAsRDF()
        {
            var filePath = "payments.rdf";
            IGraph graph = new Graph();
            string ns = "http://example.org/contract/";
            foreach (var contract in Payments)
            {
                IUriNode contractNode = graph.CreateUriNode(new Uri(ns + "payment/" + contract.PaymentId));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "amount")), graph.CreateLiteralNode(contract.Amount.ToString()));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "contract information")), graph.CreateLiteralNode(contract.ContractId.ToString()));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "contract information")), graph.CreateLiteralNode(contract.Date.ToString()));

            }
            RdfXmlWriter rdfXmlWriter = new RdfXmlWriter();
            rdfXmlWriter.Save(graph, filePath);
            return "Successfully created a XML file!";
        }

        public string exportAsJSON()
        {
            var filePath = "payments.json";
            string jsonString = JsonSerializer.Serialize(Payments);
            File.WriteAllText(filePath, jsonString);
            return "Successfully created a JSON file!";
        }
    }
}
