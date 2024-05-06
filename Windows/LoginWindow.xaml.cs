using KursovaWPF.Models;
using KursovaWPF.MVVM.CoreViewModels;
using KursovaWPF.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KursovaWPF.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public String Login { get; set; }
        public String Password { get; set; }
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
      
            try
            {
                Login = LoginBox.Text.ToString();
                Password = PassBox.Password.ToString();
                MainDBContext.Login = Login;
                MainDBContext.Password = Password;
                Logger.LogAction(string.Format
                    (
                    "Login from user with credentials {0} as login and {1} as password"
                    , Login, Password
                    )
                    );
                MainWindow mainWindow = new MainWindow();

                mainWindow.Show();
                this.Close();
            }
            catch( Exception ex ) 
            {

                MessageBox.Show("Login failed! Please check if your login and password are correct, and then try again!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
