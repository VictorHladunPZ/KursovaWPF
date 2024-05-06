using KursovaWPF.MVVM.ViewModels;
using KursovaWPF.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KursovaWPF.MVVM.CoreViewModels
{
    public class CostStatVM : ViewModelBase, IExportDataVM
    {
        public string exportAsCSV()
        {
            return "Not able to save this data.";
        }

        public string exportAsJSON()
        {
            return "Not able to save this data.";
        }

        public string exportAsRDF()
        {
            return "Not able to save this data.";
        }

        public string exportAsXML()
        {
            return "Not able to save this data.";
        }
    }
}
