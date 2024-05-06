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
    public class EmpTeamMemberDataVM : ViewModelBase, IExportDataVM
    {
        public ObservableCollection<TeamMemberViewModel> TeamMembers { get; set; }
        private ObservableCollection<TeamMemberViewModel> _pagination;
        public ObservableCollection<TeamMemberViewModel> Pagination
        {
            get { return _pagination; }
            set
            {
                _pagination = value;
                OnPropertyChanged(nameof(Pagination));
            }
        }
        public EmpTeamMemberDataVM()
        {
            Load();
        }
        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public void Load()
        {
            TeamMembers = new ObservableCollection<TeamMemberViewModel>(TeamMembersRepository.LoadTeamMembers());
            Navigate(((int)PagingMode.First));
        }



        public void Navigate(int mode)
        {
            List<TeamMemberViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:

                    if (TeamMembers.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (TeamMembers.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = TeamMembers.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();

                        }
                        else
                        {
                            returnList = TeamMembers.Skip(pageIndex *
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
                            returnList = TeamMembers.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = TeamMembers.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (TeamMembers.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) Pagination = new ObservableCollection<TeamMemberViewModel>(returnList);
        }

        internal void openAddWindow()
        {
            TeamMembersRepository.OpenAddWindow();
            Load();
        }


        public string exportAsCSV()
        {
            var filePath = "teammember.csv";
            // Open a stream writer to write to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Write the CSV header
                    csv.WriteHeader<TeamMemberViewModel>();
                    csv.NextRecord();

                    // Write each contract to the CSV file
                    foreach (var contract in TeamMembers)
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
            XmlSerializer serializer = new XmlSerializer(typeof(List<TeamMemberViewModel>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<TeamMemberViewModel> contractList = new List<TeamMemberViewModel>();
                contractList.AddRange(TeamMembers);
                serializer.Serialize(writer, contractList);
            }
            return "Successfully created a XML file!";
        }

        public string exportAsRDF()
        {
            var filePath = "teammembers.rdf";
            IGraph graph = new Graph();
            string ns = "http://example.org/contract/";
            foreach (var contract in TeamMembers)
            {
                IUriNode contractNode = graph.CreateUriNode(new Uri(ns + "team member/" + contract.TeamMemberId));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "Team ID")), graph.CreateLiteralNode(contract.TeamId.ToString()));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "Emp ID")), graph.CreateLiteralNode(contract.EmpId.ToString()));
            }
            RdfXmlWriter rdfXmlWriter = new RdfXmlWriter();
            rdfXmlWriter.Save(graph, filePath);
            return "Successfully created a XML file!";
        }

        public string exportAsJSON()
        {
            var filePath = "teammembers.json";
            string jsonString = JsonSerializer.Serialize(TeamMembers);
            File.WriteAllText(filePath, jsonString);
            return "Successfully created a JSON file!";
        }
    }
}
