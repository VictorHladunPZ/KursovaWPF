using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.ViewModels
{
    public class EmpPositionViewModel : ViewModelBase
    {
        private int _PositionId;
        public int PositionId
        {
            get { return _PositionId; }
            set
            {
                _PositionId = value;
                OnPropertyChanged(nameof(PositionId));
            }
        }
        private string _Emp;
        public string Emp
        {
            get
            {
                return _Emp;
            }
            set
            {
                _Emp = value;
                OnPropertyChanged(nameof(Emp));
            }
        }
        private decimal _PayFactor;
        public decimal PayFactor
        {
            get
            {
                return _PayFactor;
            }
            set
            {
                _PayFactor = value;
                OnPropertyChanged(nameof(PayFactor));
            }
        }
        private string _Role;
        public string Role
        {
            get
            {
                return _Role;
            }
            set
            {
                _Role = value;
                OnPropertyChanged(nameof(Role));
            }
        }
    }
}
