using KursovaWPF.Models;
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

namespace KursovaWPF.MVVM.CoreViewModels
{
    public class ProdDemandDataVM : ViewModelBase, IExportDataVM
    {
        public ObservableCollection<ProdDemandViewModel> Demands { get; set; }
        private ObservableCollection<ProdDemandViewModel> _pagination;
        public ObservableCollection<ProdDemandViewModel> Pagination
        {
            get { return _pagination; }
            set
            {
                _pagination = value;
                OnPropertyChanged(nameof(Pagination));
            }
        }
        public ProdDemandDataVM()
        {
            Load();
        }
        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public void Load()
        {
            Demands = new ObservableCollection<ProdDemandViewModel>(DemandsRepository.LoadDemands());
            Navigate(((int)PagingMode.First));
        }



        public void Navigate(int mode)
        {
            List<ProdDemandViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:

                    if (Demands.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (Demands.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = Demands.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();

                        }
                        else
                        {
                            returnList = Demands.Skip(pageIndex *
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
                            returnList = Demands.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = Demands.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (Demands.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) Pagination = new ObservableCollection<ProdDemandViewModel>(returnList);
        }

        public string exportAsCSV()
        {
            var filePath = "prodDemand.csv";
            // Open a stream writer to write to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Write the CSV header
                    csv.WriteHeader<ProdDemandViewModel>();
                    csv.NextRecord();

                    // Write each contract to the CSV file
                    foreach (var contract in Demands)
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
            var filePath = "demands.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<ProdDemandViewModel>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<ProdDemandViewModel> contractList = new List<ProdDemandViewModel>();
                contractList.AddRange(Demands);
                serializer.Serialize(writer, contractList);
            }
            return "Successfully created a XML file!";
        }

        public string exportAsRDF()
        {
            var filePath = "demands.rdf";
            IGraph graph = new Graph();
            string ns = "http://example.org/contract/";
            foreach (var contract in Demands)
            {
                IUriNode contractNode = graph.CreateUriNode(new Uri(ns + "demand/" + contract.DemandsId));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "description")), graph.CreateLiteralNode(contract.Demand));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "product information")), graph.CreateLiteralNode(contract.Product));
            }
            RdfXmlWriter rdfXmlWriter = new RdfXmlWriter();
            rdfXmlWriter.Save(graph, filePath);
            return "Successfully created a XML file!";
        }

        public string exportAsJSON()
        {
            var filePath = "demand.json";
            string jsonString = JsonSerializer.Serialize(Demands);
            File.WriteAllText(filePath, jsonString);
            return "Successfully created a JSON file!";
        }

        internal void openAddWindow()
        {
            DemandsRepository.OpenAddWindow();
            Load();
        }
    }
}
