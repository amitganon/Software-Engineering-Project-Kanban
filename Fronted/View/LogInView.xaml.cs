using Fronted.Model;
using Fronted.ViewModel;
using IntroSE.Kanban.Backend.ServiceLayer;
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
    /// Interaction logic for LogInView.xaml
    /// </summary>
    partial class LogInView : Window
    {
        private LogInViewModel logInViewModel;

        ///<summary>the constructor </summary>
        public LogInView()
        {
            InitializeComponent();
            this.DataContext = new LogInViewModel();
            this.logInViewModel = (LogInViewModel)DataContext;
        }

        ///<summary>open home page window </summary>
        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = logInViewModel.LogIn();
            if(user!=null)
            {
                HomePageView UserHomePage = new HomePageView(user);                
                UserHomePage.Show();
                this.Close();
            }
        }

        ///<summary>open Register window </summary>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterView register = new RegisterView();
            register.Show();
            this.Close();
        }

        ///<summary>open ProButton window </summary>
        private void ProButton_Click(object sender, RoutedEventArgs e)
        {
            UpgradeToPro pro = new UpgradeToPro(this);
            pro.Show();
            this.Close();
        }

        ///<summary> all this method determine if to show password 
        /// if IsEyeClicked = true / false
        /// if IsMouseOverEye = true / false</summary>
        private void ShowShowenPassword(object sender, MouseEventArgs e)
        {
            logInViewModel.IsEyeClicked = true;
        }

        private void ShowHiddenPassword(object sender, MouseEventArgs e)
        {
            logInViewModel.IsEyeClicked = false;
        }

        private void Eye_MouseEnter(object sender, MouseEventArgs e)
        {
            logInViewModel.IsMouseOverEye = true;
        }

        private void Eye_MouseLeave(object sender, MouseEventArgs e)
        {
            logInViewModel.IsMouseOverEye = false;
            logInViewModel.IsEyeClicked = false;
        }
    }
}
