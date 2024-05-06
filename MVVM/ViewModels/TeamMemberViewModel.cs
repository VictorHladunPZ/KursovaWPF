namespace KursovaWPF.MVVM.ViewModels
{
    public class TeamMemberViewModel : ViewModelBase
    {
        private int _TeamMemberId;
        public int TeamMemberId
        {
            get
            {
                return _TeamMemberId;
            }
            set
            {
                _TeamMemberId = value;
                OnPropertyChanged(nameof(TeamMemberId));
            }
        }
        private int _TeamId;

        public int TeamId
        {
            get
            {
                return _TeamId;
            }
            set
            {
                _TeamId = value;
                OnPropertyChanged(nameof(TeamId));
            }
        }
        private int _EmpId;
        public int EmpId
        {
            get
            {
                return _EmpId;
            }
            set
            {
                _EmpId = value;
                OnPropertyChanged(nameof(EmpId));
            }
        }
    }
}
