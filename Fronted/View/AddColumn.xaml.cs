using Fronted.Model;
using Fronted.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations.Model;
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
    /// Interaction logic for AddColumn.xaml
    /// </summary>
    public partial class AddColumn : Window
    {
        public ColumnViewModel viewModel;
        public BoardModel b { get; set; }
        private string BoardCreatorEmail { get; set; }
        private string BoardName { get; set; }
        private string UserEmail { get; set; }
        public string Message { get; set; }
        public AddColumn(UserModel u , BoardModel b)
        {
            InitializeComponent();
            this.viewModel = new(u);
            this.DataContext = viewModel;
            this.BoardCreatorEmail = b.CreatorEmail;
            this.BoardName = b.Name;
            this.UserEmail = u.Email;
            this.b = b;
            this.Message = "";
        }

        private void CreateC_Click(object sender, RoutedEventArgs e)
        {
            Message = viewModel.Create_Column(UserEmail,b);
            this.Close();
            
        }
    }
}
