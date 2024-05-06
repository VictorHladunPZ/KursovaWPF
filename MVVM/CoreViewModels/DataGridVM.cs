using KursovaWPF.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.CoreViewModels
{
    interface DataGridVM
    {
        public List<object> Search(string field, string  value);
        public List<object> Filter(string field, string value);
    }
}
