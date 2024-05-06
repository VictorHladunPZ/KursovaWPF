using KursovaWPF.MVVM.CoreViewModels;
using System.Windows;
using System.Windows.Controls;

namespace KursovaWPF.Resources
{
    public class ControlPanelTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NormalTemplate { get; set; }
        public DataTemplate PremiumTemplate { get; set; }
        public DataTemplate AdminTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is MainViewModel viewModel)
            {
                if (viewModel.RoleName == "NormalUser")
                {
                    return NormalTemplate;
                }

                else if (viewModel.RoleName == "PremiumUser")
                {
                    return PremiumTemplate;
                }
                else if (viewModel.RoleName == "AdminUser")
                {
                    return AdminTemplate;
                }
            }

            // Default template
            return base.SelectTemplate(item, container);
        }
    }
}
