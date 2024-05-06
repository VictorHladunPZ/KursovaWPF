namespace KursovaWPF.MVVM.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        private int _ProductId;
        public int ProductId
        {
            get { return _ProductId; }
            set
            {
                _ProductId = value;
                OnPropertyChanged(nameof(ProductId));
            }
        }
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private int _ContractId;
        public int ContractId
        {
            get
            {
                return _ContractId;
            }
            set
            {
                _ContractId = value;
                OnPropertyChanged(nameof(ContractId));
            }
        }
    }
}
