using Fronted.Model;
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
    /// Interaction logic for AddNewTaskView.xaml
    /// </summary>
    public partial class AddNewTaskView : Window
    {
        private UserModel user;
        private AddNewTaskViewModel addNewTaskViewModel;
        private BoardModel board;

        ///<summary>the constructor</summary>
        public AddNewTaskView(UserModel u, BoardModel board)
        {
            InitializeComponent();
            user = u;
            this.addNewTaskViewModel = new AddNewTaskViewModel(u);
            this.DataContext = addNewTaskViewModel;
            this.board = board;
            
        }

        ///<summary>open home page window</summary>
        private void HomePage_Click(object sender, RoutedEventArgs e)
        {
            HomePageView homePage = new HomePageView(user);
            homePage.Show();
            this.Close();
        }

        ///<summary>open log in window window
        ///log out the user from the system</summary>
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            addNewTaskViewModel.LogOut(user.Email);
            LogInView LogInView = new LogInView();
            LogInView.Show();
            this.Close();
        }

        ///<summary>add a task to board, open board page window</summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            TaskModel task = addNewTaskViewModel.AddTask(board.CreatorEmail,board.Name, board.Id);
            board.Columns[0].Tasks.Add(task);
            if (task != null)
            {
                BoardPage boardPage = new BoardPage(board, user);
                boardPage.Show();
                this.Close();
            }
        }
    }
}
