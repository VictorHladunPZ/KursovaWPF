using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.Resources
{
    public interface IExportDataVM
    {
        public string exportAsCSV();
        public string exportAsXML();
        public string exportAsRDF();
        public string exportAsJSON();
    }
}
