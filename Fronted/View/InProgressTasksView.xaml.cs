using Fronted.Model;
using Fronted.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for InProgressTasksView.xaml
    /// </summary>
    public partial class InProgressTasksView : Window
    {
        UserModel user;
        InProgressTasksViewModel inProgressTasksViewModel;

        ///<summary>the constructor</summary>
        public InProgressTasksView(UserModel user, ObservableCollection<BoardModel> boards)
        {
            InitializeComponent();
            inProgressTasksViewModel = new InProgressTasksViewModel(user, boards);
            this.DataContext = inProgressTasksViewModel;            
            this.user = user;
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
            inProgressTasksViewModel.LogOut(user.Email);
            LogInView LogInView = new LogInView();
            LogInView.Show();
            this.Close();
        }

        ///<summary>open task update window, when clicking on a selected task</summary>
        private void EnterTaskUpdate(Object sender, MouseEventArgs e)
        {
            TaskModel task = inProgressTasksViewModel.SelectedTask;
            if (task != null)
            {
                BoardModel board = inProgressTasksViewModel.GetMyBoard(task.BoardId);
                UpdateTaskView updateTaskView = new UpdateTaskView(user, board, task, task.ColumnOrdinal);////ADDDING BOARD AND COLUMN
                updateTaskView.Show();
                this.Close();
            }
        }

        ///<summary>call the function that search in tasks by key word</summary>
        private void Search_By_Key_Word(object sender, RoutedEventArgs e)
        {
            if (inProgressTasksViewModel != null)
                inProgressTasksViewModel.Search_By_Key_Word(((TextBox)sender).Text);
        }

        ///<summary>order the tasks by due date</summary>
        public void DueDateOrder(object sender, MouseButtonEventArgs e)
        {

            inProgressTasksViewModel.DueDateOrder();
        }
    }
}
