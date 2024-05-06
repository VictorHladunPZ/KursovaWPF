using KursovaWPF.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KursovaWPF.MVVM.Core
{
    public interface ISearchable
    {
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
                        if (propValue.ToString() == value)
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
