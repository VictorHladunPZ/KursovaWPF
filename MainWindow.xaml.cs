using KursovaWPF.Models;
using KursovaWPF.MVVM.CoreViewModels;
using KursovaWPF.Resources;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KursovaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double HeightParam = 0;
        double WidthParam = 0;
        public MainWindow()
        {
            InitializeComponent();
            Logger.LogAction(string.Format(
                "Login performed by user {0}, role obtained: {1}",
                MainDBContext.Login, MainDBContext.RoleName
                ));
            WidthParam = this.Width;
            HeightParam = this.Height;
        }
        public MainWindow(String login, String password)
        {
            InitializeComponent();
            Logger.LogAction(string.Format(
                "Login performed by user {0}, role obtained: {1}",
                MainDBContext.Login,MainDBContext.RoleName
                ));

            WidthParam = this.Width;
            HeightParam = this.Height;
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Height = HeightParam;
                    this.Width = WidthParam;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized= true;
                }
            }
        }

        private void ExportFileButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)this.DataContext;
            var currView = (IExportDataVM)vm.CurrentView;
            MessageBox.Show(currView.exportAsCSV(), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            MessageBox.Show(currView.exportAsJSON(), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            MessageBox.Show(currView.exportAsXML(), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            MessageBox.Show(currView.exportAsRDF(), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}