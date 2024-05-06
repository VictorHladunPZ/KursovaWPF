using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.ViewModels
{
    public class ProjectsBacklogViewModel : ViewModelBase
    {
        private int _BacklogId;
        public int BacklogId
        {
            get { return _BacklogId; }
            set
            {
                _BacklogId = value;
                OnPropertyChanged(nameof(BacklogId));
            }
        }
        private string _ProjectRetrospective;

        public string ProjectRetrospective
        {
            get { return _ProjectRetrospective; }
            set
            {
                _ProjectRetrospective = value;
                OnPropertyChanged(nameof(ProjectRetrospective));
            }
        }
        private string _Project;
        public string Project
        {
            get
            {
                return _Project;
            }
            set
            {
                _Project = value;
                OnPropertyChanged(nameof(Project));
            }
        }
    }
}
