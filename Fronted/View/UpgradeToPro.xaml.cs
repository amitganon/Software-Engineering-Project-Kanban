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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Fronted.View
{
    /// <summary>
    /// Interaction logic for UpgradeToPro.xaml
    /// </summary>
    public partial class UpgradeToPro : Window
    {
        DoubleAnimation da = new DoubleAnimation();
        private UpgradeToProViewModel viewModel;
        public UpgradeToPro(LogInView logIn)
        {
            InitializeComponent();
            this.DataContext = new UpgradeToProViewModel();
            this.viewModel = (UpgradeToProViewModel)DataContext;
            da.From = 20;
            da.To = 40;
            da.AutoReverse = true;
            da.RepeatBehavior = RepeatBehavior.Forever;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            
            textBlock.BeginAnimation(TextBlock.FontSizeProperty, da);
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            LogInView login = new LogInView();
            login.Show();
            this.Close();
        }

        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            viewModel.IsClicked = true;
        }

        private void CloseImage_Click(object sender, RoutedEventArgs e)
        {
            viewModel.IsClicked = false;
        }
    }
}
