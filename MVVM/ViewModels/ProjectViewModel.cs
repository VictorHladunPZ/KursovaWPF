using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {
        public bool Search(string field, string value)
        {
            // Get the type of the ContractViewModel
            Type type = typeof(ProjectViewModel);

            // Get all properties of the ContractViewModel
            PropertyInfo[] properties = type.GetProperties();

            // Iterate over each property
            foreach (PropertyInfo property in properties)
            {
                // Check if the property name matches the specified field
                if (property.Name == field)
                {
                    // Get the value of the property for the current instance
                    object propValue = property.GetValue(this);

                    // If the property value is a string, perform case-insensitive comparison
                    if (propValue is string stringValue)
                    {
                        if (stringValue.Contains(value, StringComparison.OrdinalIgnoreCase))
                        {
                            return true; // Found a match
                        }
                    }
                    else // For other types, just compare using ToString()
                    {
                        if (propValue.ToString() == value)
                        {
                            return true; // Found a match
                        }
                    }
                }
            }

            return false; // No match found
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
        private string _ProductName;
        public string ProductName
        {
            get
            {
                return _ProductName;
            }
            set
            {
                _ProductName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }

        private string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        private DateOnly _StartDate;
        public DateOnly StartDate
        {
            get
            {
                return _StartDate;
            }
            set
            {
                _StartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        private DateOnly _Deadline;
        public DateOnly Deadline
        {
            get
            {
                return _Deadline;
            }
            set
            {
                _Deadline = value;
                OnPropertyChanged(nameof(Deadline));
            }
        }
        private string _Status;
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
    }
}
