using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.ViewModels
{
    public class ContractTimeTableViewModel : ViewModelBase
    {
        private int _TimeTableId;
        public int TimeTableId
        {
            get
            {
                return _TimeTableId;
            }
            set
            {
                _TimeTableId = value;
                OnPropertyChanged(nameof(TimeTableId));
            }
        }
        private DateOnly _StartDate;
        public DateOnly StartDate
        {
            get
            {
                return _StartDate;
            }
            set
            {
                _StartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        private DateOnly _Deadline;
        public DateOnly Deadline
        {
            get
            {
                return _Deadline;
            }
            set
            {
                _Deadline = value;
                OnPropertyChanged(nameof(Deadline));
            }
        }
    }
}
