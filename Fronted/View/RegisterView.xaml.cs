using Fronted.ViewModel;
using System;
using System.Collections.Generic;
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

namespace Fronted.View
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
    {
        private RegisterViewModel viewModel;
        public RegisterView()
        {
            InitializeComponent();
            this.DataContext = new RegisterViewModel();
            this.viewModel = (RegisterViewModel)DataContext;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Register_Activate();
        }
        private void Register_Activate()
        {

            bool registered = viewModel.Register();
            if (registered)
            {
                LogInView login = new LogInView();
                login.Show();
                this.Close();
            }
        }
        private void keyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                Register_Activate();
        }

        private void PackIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            viewModel.IsMouseOver = true;
        }

        private void PackIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            viewModel.IsMouseOver = false;
        }

        private void returnToLogIn_Click(object sender, RoutedEventArgs e)
        {
            LogInView login = new LogInView();
            login.Show();
            this.Close();
        }
    }
}
