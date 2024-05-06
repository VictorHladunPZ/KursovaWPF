using KursovaWPF.Models;
using KursovaWPF.MVVM.Core;
using KursovaWPF.MVVM.CoreViewModels;
using KursovaWPF.MVVM.ViewModels;
using System.ComponentModel;

namespace KursovaWPF.MVVM.CoreViewModels
{
    public class MainViewModel : ViewModelBase, DataGridVM
    {
        public string RoleName;
        public RelayCommand ContractViewCommand{ get; set; }
        public RelayCommand ProductViewCommand { get; set; }
        public RelayCommand StatViewCommand { get; set; }
        public RelayCommand ContracteeViewCommand { get; set; }
        public RelayCommand PaymentViewCommand { get; set; }
        public RelayCommand ProductsViewCommand { get; set; }
        public RelayCommand ProdDemandsViewCommand { get; set; }
        public RelayCommand ProjectsViewCommand { get; set; }
        public RelayCommand ProjTasksViewCommand { get; set; }
        public RelayCommand TeamsViewCommand { get; set; }
        public RelayCommand TeamMembersViewCommand { get; set; }
        public RelayCommand EmpPositionsViewCommand { get; set; }
        public RelayCommand EmployeesViewCommand { get; set; }
        public RelayCommand PaymentGraphViewCommand {  get; set; }
        public RelayCommand RetrospectiveeViewCommand { get; set; }
        public RelayCommand LogViewCommand { get; set; }
        public ContractDataVM ContractVM { get; set; }
        public ProjectDataVM ProductVM { get; set; }
        public ContracteeDataVM ContracteeVM { get; set; }
        public CostStatVM StatVM { get; set; }
        public EmployeeDataVM EmployeeDataVM { get; set; }
        public EmpPositionDataVM EmpPositionDataVM { get; set; }
        public EmpTeamDataVM EmpTeamVM { get; set; }
        public EmpTeamMemberDataVM EmpTeamMemberVM { get; set; }
        public PaymentDataVM PaymentDataVM { get; set; }
        public ProdDemandDataVM ProdDemandDataVM { get; set; }
        public ProductDataVM ProductDataVM { get; set; }
        public ProjectDataVM ProjectDataVM { get; set; }
        public ProjRetrospectiveDataVM ProjRetrospectiveDataVM { get; set; }
        public ProjTaskDataVM ProjTaskDataVM { get; set; }
        public PaymentGraphDataVM PaymentGraphDataVM { get; set; }
        public LogDataVM LogDataVM { get; set; }
        private object _currentView;

		public object CurrentView
		{
			get { return _currentView; }
			set {
				_currentView = value;
				OnPropertyChanged(nameof(CurrentView));
			}
		
		}
        public MainViewModel()
        {
            initModels();
            CurrentView = ContractVM;
            ContractViewCommand = new RelayCommand(o =>
            {
                CurrentView = ContractVM;
            });
            ProductViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProductVM;
            });
            StatViewCommand = new RelayCommand(o =>
            {
                CurrentView = StatVM;
            });
            ContracteeViewCommand = new RelayCommand(o =>
            {
                CurrentView = ContracteeVM;
            });
            PaymentViewCommand = new RelayCommand(o =>
            {
                CurrentView = PaymentDataVM;
            });
            ProductsViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProductDataVM;
            });
            ProdDemandsViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProdDemandDataVM;
            });
            ProjectsViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProjectDataVM;
            });
            ProjTasksViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProjTaskDataVM;
            });
            TeamsViewCommand = new RelayCommand(o =>
            {
                CurrentView = EmpTeamVM;
            });
            TeamMembersViewCommand = new RelayCommand(o =>
            {
                CurrentView = EmpTeamMemberVM;
            });
            EmpPositionsViewCommand = new RelayCommand(o =>
            {
                CurrentView = EmpPositionDataVM;
            });
            EmployeesViewCommand = new RelayCommand(o =>
            {
                CurrentView = EmployeeDataVM;
            });
            PaymentGraphViewCommand = new RelayCommand(o =>
            {
                CurrentView = PaymentGraphDataVM;
            });
            LogViewCommand = new RelayCommand(o =>
            {
                CurrentView = LogDataVM;
            });
            RetrospectiveeViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProjRetrospectiveDataVM;
            });
            RoleName = MainDBContext.RoleName;
        }

        public List<object> Search(string field, string value)
        {
            var list = ((DataGridVM)CurrentView).Search(field, value);
            return list;
        }
        private void initModels() 
        { 
            ContractVM = new ContractDataVM();
            ProductVM = new ProjectDataVM();
            StatVM = new CostStatVM();
            ContracteeVM = new ContracteeDataVM();
            EmployeeDataVM = new EmployeeDataVM();
            EmpTeamMemberVM = new EmpTeamMemberDataVM();
            EmpPositionDataVM = new EmpPositionDataVM();
            EmpTeamVM = new();
            EmpTeamMemberVM = new();
            PaymentDataVM = new PaymentDataVM();
            ProdDemandDataVM = new ProdDemandDataVM();
            ProductDataVM = new ProductDataVM();
            ProjectDataVM = new ProjectDataVM();
            ProjRetrospectiveDataVM = new ProjRetrospectiveDataVM();
            ProjTaskDataVM = new ProjTaskDataVM();
            PaymentGraphDataVM = new PaymentGraphDataVM();
            LogDataVM = new LogDataVM();
        }
        public List<object> Filter(string field, string value)
        {
            return ((DataGridVM)CurrentView).Filter(field, value);
        }
    }
}
