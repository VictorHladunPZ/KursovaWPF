namespace KursovaWPF.MVVM.ViewModels
{
    public class EmpRoleViewModel : ViewModelBase
    {
        private int _EmpRoleId;
        public int EmpRoleId
        {
            get
            {
                return _EmpRoleId;
            }
            set
            {
                _EmpRoleId = value;
                OnPropertyChanged(nameof(EmpRoleId));
            }
        }
        private string _RoleName;
        public string RoleName
        {
            get
            {
                return _RoleName;
            }
            set
            {
                _RoleName = value;
                OnPropertyChanged(nameof(RoleName));
            }
        }
    }
}
