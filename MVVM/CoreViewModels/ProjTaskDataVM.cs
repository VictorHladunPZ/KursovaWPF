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
    public class ProjTaskDataVM : ViewModelBase, IExportDataVM
    {
        public ObservableCollection<ProjTaskViewModel> Tasks { get; set; }
        private ObservableCollection<ProjTaskViewModel> _pagination;
        public ProjTaskViewModel Selected;
        public ObservableCollection<ProjTaskViewModel> Pagination
        {
            get { return _pagination; }
            set
            {
                _pagination = value;
                OnPropertyChanged(nameof(Pagination));
            }
        }
        public ProjTaskDataVM()
        {
            Load();
        }
        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public void Load()
        {
            Tasks = new ObservableCollection<ProjTaskViewModel>(ProjectTasksRepository.LoadTasks());
            Navigate(((int)PagingMode.First));
        }



        public void Navigate(int mode)
        {
            List<ProjTaskViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:

                    if (Tasks.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (Tasks.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = Tasks.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();

                        }
                        else
                        {
                            returnList = Tasks.Skip(pageIndex *
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
                            returnList = Tasks.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = Tasks.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (Tasks.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) Pagination = new ObservableCollection<ProjTaskViewModel>(returnList);
        }

        internal void openAddWindow()
        {
            TaskAddWindow window = new TaskAddWindow();
            window.ShowDialog();
            this.Load();
        }

        internal void Delete()
        {
            if (Selected != null)
            {
                MessageBoxResult result = MessageBox.Show("This will delete the project task without any way to undo. Do you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show(ProjectTasksRepository.RemoveProjTask(Selected), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Load();
                }

            }
            
        }

        public string exportAsCSV()
        {
            var filePath = "Tasks.csv";
            // Open a stream writer to write to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Write the CSV header
                    csv.WriteHeader<ProjTaskViewModel>();
                    csv.NextRecord();

                    // Write each contract to the CSV file
                    foreach (var contract in Tasks)
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
            var filePath = "Tasks.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<ProjTaskViewModel>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<ProjTaskViewModel> contractList = new List<ProjTaskViewModel>();
                contractList.AddRange(Tasks);
                serializer.Serialize(writer, contractList);
            }
            return "Successfully created a XML file!";
        }

        public string exportAsRDF()
        {
            var filePath = "Tasks.rdf";
            IGraph graph = new Graph();
            string ns = "http://example.org/contract/";
            foreach (var contract in Tasks)
            {
                IUriNode contractNode = graph.CreateUriNode(new Uri(ns + "task/" + contract.TaskId));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "Project Information")), graph.CreateLiteralNode(contract.ProjectId.ToString()));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "Description")), graph.CreateLiteralNode(contract.Description));
            }
            RdfXmlWriter rdfXmlWriter = new RdfXmlWriter();
            rdfXmlWriter.Save(graph, filePath);
            return "Successfully created a XML file!";
        }

        public string exportAsJSON()
        {
            var filePath = "Tasks.json";
            string jsonString = JsonSerializer.Serialize(Tasks);
            File.WriteAllText(filePath, jsonString);
            return "Successfully created a JSON file!";
        }
    }
}
