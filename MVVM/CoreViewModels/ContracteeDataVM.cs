using KursovaWPF.MVVM.ViewModels;
using KursovaWPF.Resources;
using KursovaWPF.Resources.Repositories;
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
    public class ContracteeDataVM : ViewModelBase, IExportDataVM
    {
        public List<String> PropList = new List<string> { "Id", "Name", "ContactInformation"};
        public ObservableCollection<ContracteeViewModel> Contractees { get; set; }
        private ObservableCollection<ContracteeViewModel> _contraceetsPagination;
        public ContracteeViewModel SelectedContractee { get; set; }
        public ObservableCollection<ContracteeViewModel> ContracteesPagination
        {
            get { return _contraceetsPagination; }
            set
            {
                _contraceetsPagination = value;
                OnPropertyChanged(nameof(ContracteesPagination));
            }
        }
        public ContracteeDataVM()
        {
            LoadContractees();
        }
        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public void LoadContractees()
        {
            Contractees = new ObservableCollection<ContracteeViewModel>(ContracteeRepository.LoadContractees());
            Navigate(((int)PagingMode.First));
        }



        public void Navigate(int mode)
        {
            List<ContracteeViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:

                    if (Contractees.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (Contractees.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = Contractees.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();

                        }
                        else
                        {
                            returnList = Contractees.Skip(pageIndex *
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
                            returnList = Contractees.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = Contractees.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (Contractees.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) ContracteesPagination = new ObservableCollection<ContracteeViewModel>(returnList);
        }

        public List<ContracteeViewModel> Search(string field, string value)
        {
            List<ContracteeViewModel> returnList = new List<ContracteeViewModel>();
            foreach (ContracteeViewModel contractee in Contractees)
            {
                // Check if the current contract matches the search criteria
                if (contractee.Search(field, value))
                {
                    // If it matches, add it to the list of matching contracts
                    returnList.Add(contractee);
                }
            }
            return returnList;
        }

        public List<object> Filter(string field, string value)
        {
            throw new NotImplementedException();
        }

        public void openEditWindow()
        {
            //Contract
            if (SelectedContractee != null)
            {
                ContracteeRepository.OpenEditContracteeWindow(SelectedContractee);
            }
            LoadContractees();
        }
        public void openAddWindow()
        {
            ContracteeRepository.OpenAddContracteeWindow();
            LoadContractees();
        }
        public void DeleteContract()
        {
            if (SelectedContractee != null)
            {
                MessageBoxResult result = MessageBox.Show("This will delete the contractee without any way to undo. Do you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show(ContracteeRepository.DeleteContractee(SelectedContractee), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadContractees();
                }
                
            }
            LoadContractees();
        }

        public string exportAsCSV()
        {
            var filePath = "contractees.csv";
            // Open a stream writer to write to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Write the CSV header
                    csv.WriteHeader<ContracteeViewModel>();
                    csv.NextRecord();

                    // Write each contract to the CSV file
                    foreach (var contract in Contractees)
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
            var filePath = "contractees.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<ContracteeViewModel>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<ContracteeViewModel> contractList = new List<ContracteeViewModel>();
                contractList.AddRange(Contractees);
                serializer.Serialize(writer, contractList);
            }
            return "Successfully created a XML file!";
        }

        public string exportAsRDF()
        {
            var filePath = "contractees.rdf";
            IGraph graph = new Graph();
            string ns = "http://example.org/contract/";
            foreach (var contract in Contractees)
            {
                IUriNode contractNode = graph.CreateUriNode(new Uri(ns + "contractee/" + contract.ContracteeId));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "name")), graph.CreateLiteralNode(contract.Name));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "contact information")), graph.CreateLiteralNode(contract.ContactInformation));
            }
            RdfXmlWriter rdfXmlWriter = new RdfXmlWriter();
            rdfXmlWriter.Save(graph, filePath);
            return "Successfully created a XML file!";
        }

        public string exportAsJSON()
        {
            var filePath = "contractees.json";
            string jsonString = JsonSerializer.Serialize(Contractees);
            File.WriteAllText(filePath, jsonString);
            return "Successfully created a JSON file!";
        }
    }
}
