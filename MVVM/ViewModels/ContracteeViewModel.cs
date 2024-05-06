using KursovaWPF.MVVM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.ViewModels
{
    public class ContracteeViewModel : ViewModelBase
    {
        public bool Search(string field, string value)
        {
            // Get the type of the ContractViewModel
            Type type = typeof(ContracteeViewModel);

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
        private int _ContracteeId;
        public int ContracteeId
        {
            get
            {
                return _ContracteeId;
            }
            set
            {
                _ContracteeId = value;
                OnPropertyChanged(nameof(ContracteeId));
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
        private string _ContactInformation;
        public string ContactInformation
        {
            get
            {
                return _ContactInformation;
            }
            set
            {
                _ContactInformation = value;
                OnPropertyChanged(nameof(ContactInformation));
            }
        }
    }
}
