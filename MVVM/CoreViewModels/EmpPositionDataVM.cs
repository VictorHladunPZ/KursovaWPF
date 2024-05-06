using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using KursovaWPF.Resources;
using KursovaWPF.Resources.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VDS.RDF.Writing;
using VDS.RDF;
using System.Text.Json;

namespace KursovaWPF.MVVM.CoreViewModels
{
    public class EmpPositionDataVM : ViewModelBase, IExportDataVM
    {
        public ObservableCollection<EmpPositionViewModel> Positions { get; set; }
        private ObservableCollection<EmpPositionViewModel> _pagination;
        public ObservableCollection<EmpPositionViewModel> Pagination
        {
            get { return _pagination; }
            set
            {
                _pagination = value;
                OnPropertyChanged(nameof(Pagination));
            }
        }
        public EmpPositionDataVM()
        {
            Load();
        }
        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public void Load()
        {
            Positions = new ObservableCollection<EmpPositionViewModel>(PositionsRepository.LoadPositions());
            Navigate(((int)PagingMode.First));
        }



        public void Navigate(int mode)
        {
            List<EmpPositionViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:

                    if (Positions.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (Positions.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = Positions.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();

                        }
                        else
                        {
                            returnList = Positions.Skip(pageIndex *
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
                            returnList = Positions.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = Positions.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (Positions.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) Pagination = new ObservableCollection<EmpPositionViewModel>(returnList);
        }


        public string exportAsCSV()
        {
            var filePath = "positions.csv";
            // Open a stream writer to write to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Write the CSV header
                    csv.WriteHeader<EmpPositionViewModel>();
                    csv.NextRecord();

                    // Write each contract to the CSV file
                    foreach (var contract in Positions)
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
            var filePath = "positions.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<EmpPositionViewModel>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<EmpPositionViewModel> contractList = new List<EmpPositionViewModel>();
                contractList.AddRange(Positions);
                serializer.Serialize(writer, contractList);
            }
            return "Successfully created a XML file!";
        }

        public string exportAsRDF()
        {
            var filePath = "positions.rdf";
            IGraph graph = new Graph();
            string ns = "http://example.org/contract/";
            foreach (var contract in Positions)
            {
                IUriNode contractNode = graph.CreateUriNode(new Uri(ns + "position/" + contract.PositionId));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "payfactor")), graph.CreateLiteralNode(contract.PayFactor.ToString()));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "employee")), graph.CreateLiteralNode(contract.Emp));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "role")), graph.CreateLiteralNode(contract.Role));
            }
            RdfXmlWriter rdfXmlWriter = new RdfXmlWriter();
            rdfXmlWriter.Save(graph, filePath);
            return "Successfully created a XML file!";
        }

        public string exportAsJSON()
        {
            var filePath = "positions.json";
            string jsonString = JsonSerializer.Serialize(Positions);
            File.WriteAllText(filePath, jsonString);
            return "Successfully created a JSON file!";
        }
    }
}
