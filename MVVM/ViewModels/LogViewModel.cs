using KursovaWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.ViewModels
{
    public class LogViewModel : ViewModelBase
    {
        private int _logId;
        public int LogId {
            get 
            {
                return _logId;
            }
            set
            {
                _logId = value;
                OnPropertyChanged(nameof(LogId));
            }
        }
        private DateOnly _timeStamp;
        public DateOnly Timestamp {
            get
            {
                return _timeStamp;
            }
            set
            {
                _timeStamp = value;
                OnPropertyChanged(nameof(Timestamp));
            }
        }
        private string _message;
        public string Message {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
    }
}
