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
using IntroSE.Kanban.Backend.BusinessLayer;

namespace Fronted.View
{
    /// <summary>
    /// Interaction logic for UpdateTaskView.xaml
    /// </summary>
    public partial class UpdateTaskView : Window
    {
        UserModel user;
        private UpdateTaskViewModel updateTaskViewModel;
        private int columnOrdinal;
        private BoardModel board;
        private TaskModel THETASK;
        public UpdateTaskView(UserModel u, BoardModel board, TaskModel task, int columnOrdinal)
        {
            InitializeComponent();
            user = u;
            this.updateTaskViewModel = new UpdateTaskViewModel(u, task);
            this.DataContext = updateTaskViewModel;
            this.board = board;
            this.columnOrdinal = columnOrdinal;
            this.THETASK = task;
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
            updateTaskViewModel.LogOut(user.Email);
            LogInView LogInView = new LogInView();
            LogInView.Show();
            this.Close();
        }

        ///<summary>update the task fileds, open board page window</summary>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            TaskModel task = updateTaskViewModel.UpdateTask(board.CreatorEmail, board.Name, columnOrdinal, board.Id);
            ColumnModel SAVECM = null;
            if (task != null)
            {
                foreach (ColumnModel CM in board.Columns)
                {
                    foreach (TaskModel TM in CM.Tasks)
                    {
                        if (TM == THETASK)
                        {
                            SAVECM = CM;
                        }
                    }
                }

                SAVECM.Tasks.Remove(THETASK);
                IntroSE.Kanban.Backend.ServiceLayer.Task Tasksk = user.Controller.service.GetTask(user.Email, board.CreatorEmail, board.Name, columnOrdinal, task.Id).Value;
                SAVECM.Tasks.Add(new TaskModel(Tasksk, user.Email, user.Controller, board.Id, columnOrdinal));
                BoardPage boardPage = new BoardPage(board, user);
                this.Close();
                boardPage.Show();                
                
            }            
        }
    }
}
