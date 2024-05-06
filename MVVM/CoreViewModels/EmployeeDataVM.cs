using KursovaWPF.Models;
using KursovaWPF.MVVM.ViewModels;
using KursovaWPF.Resources;
using KursovaWPF.Resources.Repositories;
using KursovaWPF.Windows;
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
    public class EmployeeDataVM : ViewModelBase, IExportDataVM
    {
        public List<String> PropList = new List<string> { "EmployeeId", "LastName", "FirstName", "Salary"};
        public ObservableCollection<EmployeeViewModel> Employees { get; set; }
        private ObservableCollection<EmployeeViewModel> _employeesPagination;
        public EmployeeViewModel SelectedEmployee { get; set; }
        public ObservableCollection<EmployeeViewModel> EmployeePagination
        {
            get { return _employeesPagination; }
            set
            {
                _employeesPagination = value;
                OnPropertyChanged(nameof(EmployeePagination));
            }
        }
        public EmployeeDataVM()
        {
            LoadEmployees();
        }
        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public void LoadEmployees()
        {
            Employees = new ObservableCollection<EmployeeViewModel>(EmployeesRepository.LoadEmployees()); 
            Navigate(((int)PagingMode.First));
        }



        public void Navigate(int mode)
        {
            List<EmployeeViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:

                    if (Employees.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (Employees.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = Employees.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();

                        }
                        else
                        {
                            returnList = Employees.Skip(pageIndex *
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
                            returnList = Employees.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = Employees.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (Employees.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) EmployeePagination = new ObservableCollection<EmployeeViewModel>(returnList);
        }

        public string exportAsCSV()
        {
            var filePath = "employees.csv";
            // Open a stream writer to write to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Write the CSV header
                    csv.WriteHeader<EmployeeViewModel>();
                    csv.NextRecord();

                    // Write each contract to the CSV file
                    foreach (var contract in Employees)
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
            var filePath = "employees.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<EmployeeViewModel>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<EmployeeViewModel> contractList = new List<EmployeeViewModel>();
                contractList.AddRange(Employees);
                serializer.Serialize(writer, contractList);
            }
            return "Successfully created a XML file!";
        }

        public string exportAsRDF()
        {
            var filePath = "employee.rdf";
            IGraph graph = new Graph();
            string ns = "http://example.org/contract/";
            foreach (var contract in Employees)
            {
                IUriNode contractNode = graph.CreateUriNode(new Uri(ns + "employee/" + contract.EmployeeId));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "first name")), graph.CreateLiteralNode(contract.FirstName));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "last name")), graph.CreateLiteralNode(contract.LastName));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "salary")), graph.CreateLiteralNode(contract.Salary.ToString()));
            }
            RdfXmlWriter rdfXmlWriter = new RdfXmlWriter();
            rdfXmlWriter.Save(graph, filePath);
            return "Successfully created a XML file!";
        }

        public string exportAsJSON()
        {
            var filePath = "employees.json";
            string jsonString = JsonSerializer.Serialize(Employees);
            File.WriteAllText(filePath, jsonString);
            return "Successfully created a JSON file!";
        }
    }
}
