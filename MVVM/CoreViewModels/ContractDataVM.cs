using CsvHelper;
using KursovaWPF.MVVM.ViewModels;
using KursovaWPF.Resources;
using KursovaWPF.Resources.Repositories;
using KursovaWPF.Windows;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Xml.Serialization;
using VDS.RDF.Writing;
using VDS.RDF;

namespace KursovaWPF.MVVM.CoreViewModels
{
    public class ContractDataVM : ViewModelBase, DataGridVM, IExportDataVM
    {
        public List<String> PropList = new List<string> { "Id", "Contractee", "Description", "StartDate", "Deadline", "Status", "Cost" };
        private string _currentFilter;

        public string currentFilter
        {
            get { return _currentFilter; }
            set { 
                _currentFilter = value;
                OnPropertyChanged(nameof(currentFilter));
            }
        }

        public ObservableCollection<ContractViewModel> Contracts { get; set; }
        private ObservableCollection<ContractViewModel> _contractsPagination;
        public ContractViewModel SelectedContract { get; set; }
        public ObservableCollection<ContractViewModel> ContractsPagination
        {
            get { return _contractsPagination; }
            set { 
                _contractsPagination = value;
                OnPropertyChanged(nameof(ContractsPagination));
            }
        }
        public ContractDataVM()
        {
            LoadContracts();
            _currentFilter = "None";

        }
        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4};
        public void LoadContracts()
        {
            currentFilter = "None";
            Contracts = new ObservableCollection<ContractViewModel>(ContractRepository.LoadContracts());
            //ContractsPagination = new ObservableCollection<ContractViewModel>(Contracts.Take(numberOfRecPerPage));
            Navigate(((int)PagingMode.First));
        }

        
        
        public void Navigate(int mode)
        {
            List<ContractViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:
                    
                    if (Contracts.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (Contracts.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = Contracts.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                            
                        }
                        else
                        {
                            returnList = Contracts.Skip(pageIndex *
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
                            returnList = Contracts.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = Contracts.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (Contracts.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) ContractsPagination = new ObservableCollection<ContractViewModel>(returnList);
        }
        
        List<object> DataGridVM.Search(string field, string value)
        {
            List<object> returnList = new List<object>();
            foreach (ContractViewModel contract in Contracts)
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

        public List<object> Filter(string field, string value)
        {
            throw new NotImplementedException();    
        }

        public void openEditWindow() 
        {
            //Contract
            if (SelectedContract != null)
            {
                ContractRepository.OpenEditContractWindow(SelectedContract);
            }
            LoadContracts();
        }
        public void openAddWindow()
        {
            ContractRepository.OpenAddContractWindow();
            LoadContracts();
        }
        public void DeleteContract()
        {
            if (SelectedContract != null)
            {
                MessageBoxResult result = MessageBox.Show("This will delete the contract without any way to undo. Do you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show(ContractRepository.DeleteContract(SelectedContract),"Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadContracts();
                }
                
            }
        }
        public List<ContractViewModel> FilterContract()
        {
            ContractSearchPopup window = new ContractSearchPopup(); 
            window.ShowDialog();
            List<ContractViewModel> list = this.Contracts.ToList();
            currentFilter = string.Empty;
            if (window.FilterCost)
            {
                list = list.Where(x => x.Cost <= window.CostCeiling).Where(x => x.Cost >= window.CostFloor).ToList();
                currentFilter += " Cost: Between " + window.CostFloor +" and " + window.CostCeiling;
            }
            if (window.FilterStart)
            {
                list = list.Where(x => x.StartDate <= window.FilterStartDateCeiling).Where(x => x.StartDate >= window.FilterStartDateFloor).ToList();
                currentFilter += " Start Date: Between " + window.FilterStartDateFloor + " and " + window.FilterStartDateCeiling;
            }
            if (window.FilterDeadLine)
            {
                list = list.Where(x => x.Deadline <= window.FilterDeadlineCeiling).Where(x => x.StartDate >= window.FilterDeadlineFloor).ToList();
                currentFilter += " Deadline: Between " + window.FilterDeadlineFloor + " and " + window.FilterDeadlineCeiling;
            }
            if (String.IsNullOrEmpty(currentFilter)) currentFilter = "None";
           
            return list;
        }

        public string exportAsCSV()
        {
            var filePath = "contracts.csv";
            // Open a stream writer to write to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Write the CSV header
                    csv.WriteHeader<ContractViewModel>();
                    csv.NextRecord();

                    // Write each contract to the CSV file
                    foreach (var contract in Contracts)
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
            var filePath = "contracts.xml";
            // Create a serializer for ContractViewModel type
            XmlSerializer serializer = new XmlSerializer(typeof(List<ContractViewModel>));

            // Open a stream writer to write to the XML file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<ContractViewModel> contractList = new List<ContractViewModel>();
                contractList.AddRange(Contracts);
                // Serialize the list of contracts to XML and write it to the file
                serializer.Serialize(writer, contractList);
            }
            return "Successfully created a XML file!";
        }

        public string exportAsRDF()
        {
            var filePath = "contracts.rdf";
            // Create a new Graph
            IGraph graph = new Graph();

            // Define a namespace for your RDF vocabulary
            string ns = "http://example.org/contract/";

            // Iterate over each contract and add RDF triples to the graph
            foreach (var contract in Contracts)
            {
                // Create a new URI node for the contract
                IUriNode contractNode = graph.CreateUriNode(new Uri(ns + "contract/" + contract.ContractId));

                // Add properties to the contract node
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "contractee")), graph.CreateLiteralNode(contract.Contractee));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "description")), graph.CreateLiteralNode(contract.Description));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "startDate")), graph.CreateLiteralNode(contract.StartDate.ToString()));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "deadline")), graph.CreateLiteralNode(contract.Deadline.ToString()));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "status")), graph.CreateLiteralNode(contract.Status));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "cost")), graph.CreateLiteralNode(contract.Cost.ToString()));
            }

            // Serialize the graph to RDF/XML and write it to the file
            RdfXmlWriter rdfXmlWriter = new RdfXmlWriter();
            rdfXmlWriter.Save(graph, filePath);
            return "Successfully created a XML file!";
        }

        public string exportAsJSON()
        {
            var filePath = "contracts.json";
            // Serialize the list of contracts to JSON
            string jsonString = JsonSerializer.Serialize(Contracts);

            // Write the JSON string to the file
            File.WriteAllText(filePath, jsonString);
            return "Successfully created a JSON file!";
        }
    }
}
