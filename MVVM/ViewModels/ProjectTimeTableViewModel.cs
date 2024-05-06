using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.ViewModels
{
    public class ProjectTimeTableViewModel : ViewModelBase
    {
        private int _ProjectTimeTableId;
        public int ProjectTimeTableId
        {
            get { return _ProjectTimeTableId; }
            set
            {
                _ProjectTimeTableId = value;
                OnPropertyChanged(nameof(ProjectTimeTableId));
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
        private DateOnly _DeadLine;
        public DateOnly DeadLine
        {
            get
            {
                return _DeadLine;
            }
            set
            {
                _DeadLine = value;
                OnPropertyChanged(nameof(DeadLine));
            }

        }
    }
}
