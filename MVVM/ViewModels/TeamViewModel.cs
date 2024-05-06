using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.ViewModels
{
    public class TeamViewModel : ViewModelBase
    {
        private int _TeamId;
        public int TeamId
        {
            get { return _TeamId; }
            set
            {
                _TeamId = value;
                OnPropertyChanged(nameof(TeamId));
            }
        }
        private int _ProjectId;
        public int ProjectId
        {
            get { return _ProjectId; }
            set
            {
                _ProjectId = value;
                OnPropertyChanged(nameof(ProjectId));
            }
        }
        private string _TeamName;
        public string TeamName
        {
            get { return _TeamName; }
            set
            {
                _TeamName = value;
                OnPropertyChanged(nameof(TeamName));
            }
        }
    }
}
