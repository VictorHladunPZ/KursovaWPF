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
    public class ProjectDataVM : ViewModelBase, IExportDataVM
    {
        public List<String> PropList = new List<string> { "ProjectId", "ProductName", "Title", "StartDate", "Deadline", "Status" };
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
        public ObservableCollection<ProjectViewModel> Projects { get; set; }
        private ObservableCollection<ProjectViewModel> _Pagination;
        public ProjectViewModel Selected { get; set; }
        public ObservableCollection<ProjectViewModel> Pagination
        {
            get { return _Pagination; }
            set
            {
                _Pagination = value;
                OnPropertyChanged(nameof(Pagination));
            }
        }
        public ProjectDataVM()
        {
            currentFilter = "None";
            Load();
        }
        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public void Load()
        {
            Projects = new ObservableCollection<ProjectViewModel>(ProjectRepository.LoadProjects());
            Navigate(((int)PagingMode.First));
        }



        public void Navigate(int mode)
        {
            List<ProjectViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:

                    if (Projects.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (Projects.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = Projects.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();

                        }
                        else
                        {
                            returnList = Projects.Skip(pageIndex *
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
                            returnList = Projects.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = Projects.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (Projects.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) Pagination = new ObservableCollection<ProjectViewModel>(returnList);
        }

        List<ProjectViewModel> Search(string field, string value)
        {
            List<ProjectViewModel> returnList = new List<ProjectViewModel>();
            foreach (ProjectViewModel contract in Projects)
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

        public List<ProjectViewModel> FilterContract()
        {
            ProjectsFilterPopup window = new ProjectsFilterPopup();
            window.ShowDialog();
            currentFilter = string.Empty;
            List<ProjectViewModel> list = this.Projects.ToList();

            if (window.ByStart)
            {
                list = list.Where(x => x.StartDate <= window.StartCeiling).Where(x => x.StartDate >= window.StartFloor).ToList();
                currentFilter += " Start Date: Between " + window.StartFloor + " and " + window.StartCeiling;
            }
            if (window.ByEnd)
            {
                list = list.Where(x => x.Deadline <= window.DeadCeiling).Where(x => x.Deadline >= window.DeadFloor).ToList();
                currentFilter += " Dead Line: Between " + window.DeadFloor + " and " + window.DeadCeiling;
            }
            if (String.IsNullOrEmpty(currentFilter)) currentFilter = "None";
            return list;
        }

        internal void openAddWindow()
        {
            ProjectAddWindow projectEditWindow = new ProjectAddWindow();
            projectEditWindow.ShowDialog();
            if(projectEditWindow.viewModel != null) ProjectRepository.AddProject(projectEditWindow.viewModel);
        }

        internal void openEditWindow()
        {
            ProjectEditWindow projectEditWindow = new ProjectEditWindow(Selected);
            projectEditWindow.ShowDialog();
            if (projectEditWindow.viewModel != null) ProjectRepository.EditProject(projectEditWindow.viewModel);
        }

        internal void Delete()
        {
           
            if (Selected != null)
            {
                MessageBoxResult result = MessageBox.Show("This will delete the project without any way to undo. Do you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show(ProjectRepository.DeleteProejct(Selected), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Load();
                }

            }
        }

        public string exportAsCSV()
        {
            var filePath = "Projects.csv";
            // Open a stream writer to write to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Write the CSV header
                    csv.WriteHeader<ProjectViewModel>();
                    csv.NextRecord();

                    // Write each contract to the CSV file
                    foreach (var contract in Projects)
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
            var filePath = "Projects.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<ProjectViewModel>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<ProjectViewModel> contractList = new List<ProjectViewModel>();
                contractList.AddRange(Projects);
                serializer.Serialize(writer, contractList);
            }
            return "Successfully created a XML file!";
        }

        public string exportAsRDF()
        {
            var filePath = "Projects.rdf";
            IGraph graph = new Graph();
            string ns = "http://example.org/contract/";
            foreach (var contract in Projects)
            {
                IUriNode contractNode = graph.CreateUriNode(new Uri(ns + "project/" + contract.ProjectId));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "name")), graph.CreateLiteralNode(contract.Title));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "status")), graph.CreateLiteralNode(contract.Status));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "Start Date")), graph.CreateLiteralNode(contract.StartDate.ToString()));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "Dead Line")), graph.CreateLiteralNode(contract.Deadline.ToString()));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "Product Name")), graph.CreateLiteralNode(contract.ProductName));
            }
            RdfXmlWriter rdfXmlWriter = new RdfXmlWriter();
            rdfXmlWriter.Save(graph, filePath);
            return "Successfully created a XML file!";
        }

        public string exportAsJSON()
        {
            var filePath = "Projects.json";
            string jsonString = JsonSerializer.Serialize(Projects);
            File.WriteAllText(filePath, jsonString);
            return "Successfully created a JSON file!";
        }
    }
}
