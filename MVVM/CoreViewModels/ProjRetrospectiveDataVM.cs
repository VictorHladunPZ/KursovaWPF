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
using System.Windows;

namespace KursovaWPF.MVVM.CoreViewModels
{
    public class ProjRetrospectiveDataVM : ViewModelBase, IExportDataVM
    {
        public ObservableCollection<ProjectsBacklogViewModel> Retrospectives { get; set; }
        private ObservableCollection<ProjectsBacklogViewModel> _pagination;
        public ProjectsBacklogViewModel Selected;
        public ObservableCollection<ProjectsBacklogViewModel> Pagination
        {
            get { return _pagination; }
            set
            {
                _pagination = value;
                OnPropertyChanged(nameof(Pagination));
            }
        }
        public ProjRetrospectiveDataVM()
        {
            Load();
        }
        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public void Load()
        {
            Retrospectives = new ObservableCollection<ProjectsBacklogViewModel>(BacklogRepository.LoadBacklogs());
            Navigate(((int)PagingMode.First));
        }



        public void Navigate(int mode)
        {
            List<ProjectsBacklogViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:

                    if (Retrospectives.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (Retrospectives.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = Retrospectives.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();

                        }
                        else
                        {
                            returnList = Retrospectives.Skip(pageIndex *
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
                            returnList = Retrospectives.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = Retrospectives.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (Retrospectives.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) Pagination = new ObservableCollection<ProjectsBacklogViewModel>(returnList);
        }

        internal void openAddWindow()
        {
            RetrospectiveAddWindow window = new RetrospectiveAddWindow();
            window.ShowDialog();
            Load();
        }

        internal void Delete()
        {
            if (Selected != null)
            {
                MessageBoxResult result = MessageBox.Show("This will delete the product without any way to undo. Do you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show(BacklogRepository.RemoveBacklog(Selected), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Load();
                }

            }
            
        }

        public string exportAsCSV()
        {
            var filePath = "Retrospectives.csv";
            // Open a stream writer to write to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Write the CSV header
                    csv.WriteHeader<ProjectsBacklogViewModel>();
                    csv.NextRecord();

                    // Write each contract to the CSV file
                    foreach (var contract in Retrospectives)
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
            var filePath = "Retrospectives.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<ProjectsBacklogViewModel>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<ProjectsBacklogViewModel> contractList = new List<ProjectsBacklogViewModel>();
                contractList.AddRange(Retrospectives);
                serializer.Serialize(writer, contractList);
            }
            return "Successfully created a XML file!";
        }

        public string exportAsRDF()
        {
            var filePath = "Retrospectives.rdf";
            IGraph graph = new Graph();
            string ns = "http://example.org/contract/";
            foreach (var contract in Retrospectives)
            {
                IUriNode contractNode = graph.CreateUriNode(new Uri(ns + "retrospective/" + contract.BacklogId));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "project")), graph.CreateLiteralNode(contract.Project));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "description")), graph.CreateLiteralNode(contract.ProjectRetrospective));
            }
            RdfXmlWriter rdfXmlWriter = new RdfXmlWriter();
            rdfXmlWriter.Save(graph, filePath);
            return "Successfully created a XML file!";
        }

        public string exportAsJSON()
        {
            var filePath = "Retrospectives.json";
            string jsonString = JsonSerializer.Serialize(Retrospectives);
            File.WriteAllText(filePath, jsonString);
            return "Successfully created a JSON file!";
        }
    }
}
