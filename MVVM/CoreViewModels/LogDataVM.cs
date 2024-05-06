using KursovaWPF.MVVM.ViewModels;
using KursovaWPF.Resources.Repositories;
using KursovaWPF.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.CoreViewModels
{
    public class LogDataVM : ViewModelBase
    {

        int pageIndex = 1;
        private readonly int numberOfRecPerPage = 5;
        ObservableCollection<LogViewModel> logList = new ObservableCollection<LogViewModel>();
        private string _currentFilter;

        public string currentFilter
        {
            get { return _currentFilter; }
            set
            {
                _currentFilter = value;
                OnPropertyChanged(nameof(currentFilter));
            }
        }
        private ObservableCollection<LogViewModel> _pagination;
        public ObservableCollection<LogViewModel> Pagination
        {
            get { return _pagination; }
            set
            {
                _pagination = value;
                OnPropertyChanged(nameof(Pagination));
            }
        }
        public LogDataVM()
        {
            currentFilter = "None";
            Load();
        }
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4 };
        public void Load()
        {
            currentFilter = "None";
            logList = new ObservableCollection<LogViewModel>(LogRepository.Load());
            Pagination = new ObservableCollection<LogViewModel>(logList.Take(numberOfRecPerPage));
            pageIndex = 1;
            Navigate(((int)PagingMode.First));
        }
        public void Navigate(int mode)
        {
            List<LogViewModel> returnList = [];
            switch (mode)
            {
                case (int)PagingMode.Next:

                    if (logList.Count >= (pageIndex * numberOfRecPerPage))
                    {
                        if (logList.Skip(pageIndex *
                        numberOfRecPerPage).Take(numberOfRecPerPage).Count() == 0)
                        {
                            returnList = logList.Skip((pageIndex *
                            numberOfRecPerPage) - numberOfRecPerPage).Take(numberOfRecPerPage).ToList();

                        }
                        else
                        {
                            returnList = logList.Skip(pageIndex *
                            numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                            pageIndex++;
                        }
                    }

                    break;
                case (int)PagingMode.Previous:
                    if (pageIndex > 1)
                    {
                        pageIndex -= 1;
                        if (pageIndex == 1)
                        {
                            returnList = logList.Take(numberOfRecPerPage).ToList();
                        }
                        else
                        {
                            returnList = logList.Skip((pageIndex - 1) * numberOfRecPerPage).Take(numberOfRecPerPage).ToList();
                        }
                    }
                    break;

                case (int)PagingMode.First:
                    pageIndex = 2;
                    Navigate((int)PagingMode.Previous);
                    break;
                case (int)PagingMode.Last:
                    pageIndex = (logList.Count / numberOfRecPerPage);
                    Navigate((int)PagingMode.Next);
                    break;

            }
            if (returnList.Count != 0) Pagination = new ObservableCollection<LogViewModel>(returnList);
        }
        public List<LogViewModel> Filter(DateOnly timeFloor, DateOnly timeCeiling)
        {
            List<LogViewModel> list = this.logList.ToList();
            currentFilter = string.Empty;

            list = list.Where(x => x.Timestamp <= timeCeiling).Where(x => x.Timestamp>= timeFloor).ToList();
            currentFilter += " Timestamp: Between " + timeFloor + " and " + timeCeiling;

            return list;
        }
    }
}
