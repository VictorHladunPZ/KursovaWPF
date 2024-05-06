using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        private int _EmployeeId;
        public int EmployeeId
        {
            get
            {
                return _EmployeeId;
            }
            set
            {
                _EmployeeId = value;
                OnPropertyChanged(nameof(EmployeeId));
            }
        }
        private string _LastName;
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        private string _FirstName;
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        private decimal _Salary;
        public decimal Salary
        {
            get
            {
                return _Salary;
            }
            set
            {
                _Salary = value;
                OnPropertyChanged(nameof(Salary));
            }
        }
    }
}
