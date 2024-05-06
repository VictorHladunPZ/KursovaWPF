using System.Reflection;

namespace KursovaWPF.MVVM.ViewModels
{
    public class ContractViewModel : ViewModelBase
    {
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

        private string _OwnerName;
        public string Contractee
        {
            get
            {
                return _OwnerName;
            }
            set
            {
                _OwnerName = value;
                OnPropertyChanged(nameof(Contractee));
            }
        }
        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged(nameof(Description));
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
        private DateOnly _DeadLine;
        public DateOnly Deadline
        {
            get
            {
                return _DeadLine;
            }
            set
            {
                _DeadLine = value;
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
        private decimal _Cost;
        public decimal Cost
        {
            get
            {
                return _Cost;
            }
            set
            {
                _Cost = value;
                OnPropertyChanged(nameof(Cost));
            }
        }
        public bool Search(string field, string value)
        {
            // Get the type of the ContractViewModel
            Type type = typeof(ContractViewModel);

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
                        var value1 = propValue.ToString();
                        var value2  = value.ToString();
                        if (propValue.ToString().Equals(value.ToString()))
                        {
                            return true; // Found a match
                        }
                    }
                }
            }

            return false; // No match found
        }

    }
}
