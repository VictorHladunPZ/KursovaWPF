using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.ViewModels
{
    public class ProjTaskViewModel : ViewModelBase
    {
        private int _TaskId;
        public int TaskId
        {
            get { return _TaskId; }
            set
            {
                _TaskId = value;
                OnPropertyChanged(nameof(TaskId));
            }
        }
        private int _ProjectId;
        public int ProjectId
        {
            get
            {
                return _ProjectId;
            }
            set
            {
                _ProjectId = value;
                OnPropertyChanged(nameof(ProjectId));
            }
        }
        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
    }
}
