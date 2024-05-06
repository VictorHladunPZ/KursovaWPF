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
    public  class ProductDataVM : ViewModelBase, IExportDataVM
    {
        public ObservableCollection<ProductViewModel> Products { get; set; }
        private ObservableCollection<ProductViewModel> _pagination;
        public ProductViewModel Selected { get; set; }
        public ObservableCollection<ProductViewModel> Pagination
        {
            get { return _pagination; }
            set
            {
                _pagination = value;
                OnPropertyChanged(nameof(Pagination));
            }
        }
        public ProductDataVM()
        {
            Load();
        }
        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public void Load()
        {
            Products = new ObservableCollection<ProductViewModel>(ProductsRepository.LoadProducts());
            Navigate(((int)PagingMode.First));
        }



        public void Navigate(int mode)
        {
            List<ProductViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:

                    if (Products.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (Products.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = Products.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();

                        }
                        else
                        {
                            returnList = Products.Skip(pageIndex *
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
                            returnList = Products.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = Products.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (Products.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) Pagination = new ObservableCollection<ProductViewModel>(returnList);
        }

        internal void openAddWindow()
        {
            ProductAddWindow window = new ProductAddWindow();
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
                    MessageBox.Show(ProductsRepository.RemoveProduct(Selected), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Load();
                }

            }
            
        }

        public string exportAsCSV()
        {
            var filePath = "product.csv";
            // Open a stream writer to write to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    // Write the CSV header
                    csv.WriteHeader<ProductViewModel>();
                    csv.NextRecord();

                    // Write each contract to the CSV file
                    foreach (var contract in Products)
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
            var filePath = "Products.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<ProductViewModel>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                List<ProductViewModel> contractList = new List<ProductViewModel>();
                contractList.AddRange(Products);
                serializer.Serialize(writer, contractList);
            }
            return "Successfully created a XML file!";
        }

        public string exportAsRDF()
        {
            var filePath = "Products.rdf";
            IGraph graph = new Graph();
            string ns = "http://example.org/contract/";
            foreach (var contract in Products)
            {
                IUriNode contractNode = graph.CreateUriNode(new Uri(ns + "product/" + contract.ProductId));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "name")), graph.CreateLiteralNode(contract.Name));
                graph.Assert(contractNode, graph.CreateUriNode(new Uri(ns + "contract information")), graph.CreateLiteralNode(contract.ContractId.ToString()));
            }
            RdfXmlWriter rdfXmlWriter = new RdfXmlWriter();
            rdfXmlWriter.Save(graph, filePath);
            return "Successfully created a XML file!";
        }

        public string exportAsJSON()
        {
            var filePath = "Products.json";
            string jsonString = JsonSerializer.Serialize(Products);
            File.WriteAllText(filePath, jsonString);
            return "Successfully created a JSON file!";
        }
    }
}
