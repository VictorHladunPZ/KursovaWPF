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
    public class EmpTeamDataVM : ViewModelBase, IExportDataVM
    {
        public ObservableCollection<TeamViewModel> Teams { get; set; }
        public TeamViewModel Selected { get; set; }
        private ObservableCollection<TeamViewModel> _pagination;
        public ObservableCollection<TeamViewModel> Pagination
        {
            get { return _pagination; }
            set
            {
                _pagination = value;
                OnPropertyChanged(nameof(Pagination));
            }
        }
        public EmpTeamDataVM()
        {
            Load();
        }
        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public void Load()
        {
            Teams = new ObservableCollection<TeamViewModel>(TeamsRepository.LoadTeams());
            Navigate(((int)PagingMode.First));
        }



        public void Navigate(int mode)
        {
            List<TeamViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:

                    if (Teams.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (Teams.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = Teams.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();

                        }
                        else
                        {
                            returnList = Teams.Skip(pageIndex *
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
                            returnList = Teams.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = Teams.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (Teams.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) Pagination = new ObservableCollection<TeamViewModel>(returnList);
        }

        internal void openAddWindow()
        {
            AddTeamWindow addTeamWindow = new AddTeamWindow();
            addTeamWindow.ShowDialog();
            Load();
        }

        internal void Delete()
        {
            if (Selected != null)
            {
                MessageBoxResult result = MessageBox.Show("This will delete the emp team without any way to undo. Do you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show(TeamsRepository.RemoveTeam(Selected), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Load();
                }

            }
        }


        public string exportAsCSV()
        {
            var filePath = "EmpTeams.csv";
            // Open a stream writer to write to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Write the CSV header
                    csv.WriteHeader<TeamViewModel>();
                    csv.NextRecord();

                    // Write each contract to the CSV file
                    foreach (var contract in Teams)
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
            var filePath = "Teams.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<TeamViewModel>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<TeamViewModel> contractList = new List<TeamViewModel>();
                contractList.AddRange(Teams);
                serializer.Serialize(writer, contractList);
            }
            return "Successfully created a XML file!";
        }

        public string exportAsRDF()
        {
            var filePath = "Teams.rdf";
            IGraph graph = new Graph();
            string ns = "http://example.org/contract/";
            foreach (var contract in Teams)
            {
                IUriNode contractNode = graph.CreateUriNode(new Uri(ns + "team/" + contract.TeamId));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "name")), graph.CreateLiteralNode(contract.TeamName));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "project information")), graph.CreateLiteralNode(contract.ProjectId.ToString()));
            }
            RdfXmlWriter rdfXmlWriter = new RdfXmlWriter();
            rdfXmlWriter.Save(graph, filePath);
            return "Successfully created a XML file!";
        }

        public string exportAsJSON()
        {
            var filePath = "contractees.json";
            string jsonString = JsonSerializer.Serialize(Teams);
            File.WriteAllText(filePath, jsonString);
            return "Successfully created a JSON file!";
        }
    }
}
