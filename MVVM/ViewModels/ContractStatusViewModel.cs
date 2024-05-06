using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.ViewModels
{
    public class ContractStatusViewModel : ViewModelBase
    {
        private int _ContractStatusId;
        public int ContractStatusId
        {
            get
            {
                return _ContractStatusId;
            }
            set
            {
                _ContractStatusId = value;
                OnPropertyChanged(nameof(ContractStatusId));
            }
        }
        private string _StatusName;
        public string StatusName
        {
            get
            {
                return _StatusName;
            }
            set
            {
                _StatusName = value;
                OnPropertyChanged(nameof(StatusName));
            }
        }
    }
}
