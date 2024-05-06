using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.ViewModels
{
    public class ProdDemandViewModel : ViewModelBase
    {
        private int _DemandsId;
        public int DemandsId
        {
            get
            {
                return _DemandsId;
            }
            set
            {
                if (_DemandsId != value)
                {
                    _DemandsId = value;
                    OnPropertyChanged(nameof(DemandsId));
                }
            }
        }
        private string _Product;
        public string Product
        {
            get
            {
                return _Product;
            }
            set
            {
                _Product = value;
                OnPropertyChanged(nameof(Product));
            }
        }
        private string _Demand;
        public string Demand
        {
            get
            {
                return _Demand;
            }
            set
            {
                _Demand = value;
                OnPropertyChanged(nameof(Demand));
            }
        }
    }
}
