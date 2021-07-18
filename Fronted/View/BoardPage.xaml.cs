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
using IntroSE.Kanban.Backend.ServiceLayer;
using Fronted.ViewModel;
using Fronted.Model;

namespace Fronted.View
{
    /// <summary>
    /// Interaction logic for BoardPage.xaml
    /// </summary>
    public partial class BoardPage : Window
    {
        UserModel user;
        BoardModel b;
        public BoardViewModel viewModel;
        public BoardPage(BoardModel b , UserModel u)
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(b, u);
            this.DataContext = viewModel;
            user = u;
            this.b = b;
        }
        /// <summary>
        /// logout from the system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logout(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            LogInView logInView = new LogInView();
            logInView.Show();
            this.Close();
        }
        /// <summary>
        /// remove Board from user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Button(object sender, RoutedEventArgs e)
        {
            if (viewModel.RemoveBoard()){
                HomePageView HPW = new HomePageView(user);
                HPW.Show();
                this.Close();
            }
        }
        /// <summary>
        /// move selected column right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Move_Column_Right(object sender, RoutedEventArgs e)
        {
            if (viewModel.BoardModel.SelectedColumn != null)
            {
                viewModel.Move_Column_Right();
            }
        }
        /// <summary>
        /// move selected column left
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Move_Column_Left(object sender, RoutedEventArgs e)
        {
            if (viewModel.BoardModel.SelectedColumn != null)
            {
                viewModel.Move_Column_Left();
            }
        }
        /// <summary>
        /// change column name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_Column_Name(object sender, EventArgs e)
        {
            viewModel.Change_Column_Name(((TextBox)sender).Text);
        }
        /// <summary>
        /// change column limit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_Column_Limit(object sender, EventArgs e)
        {
            viewModel.Change_Column_Limit(int.Parse(((TextBox)sender).Text));
        }
        /// <summary>
        /// delete column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Column(object sender, RoutedEventArgs e)
        {
            if (viewModel.BoardModel.SelectedColumn != null)
            {
                int deletedColumn = viewModel.BoardModel.Columns.IndexOf(viewModel.BoardModel.SelectedColumn);
                if (deletedColumn != -1)
                {
                    viewModel.Delete_Column(deletedColumn);
                }
            }
        }
        /// <summary>
        /// sort the task in specific column by due date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DueDateOrder(object sender, EventArgs e)
        {

            viewModel.DueDateOrder();
        }
        /// <summary>
        /// add column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Column(object sender, RoutedEventArgs e)
        {
            AddColumn addColumnWindow = new(viewModel.User , viewModel.BoardModel);
            addColumnWindow.Show();
            viewModel.Message = addColumnWindow.Message;
        }

        /// <summary>
        /// search and show all tasks inclode the keyword typed in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_By_Key_Word(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            viewModel.Search_By_Key_Word(((TextBox)sender).Text);
        }
        /// <summary>
        /// move task right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Move_Task(object sender, EventArgs e)
        {
            if ((viewModel.BoardModel.ModelColumn.SelectedTask != null && checkEmailAssignee(viewModel.BoardModel.ModelColumn.SelectedTask)) || (findSelectedTaskFromProperty() != null && checkEmailAssignee(findSelectedTaskFromProperty())) )
            {
                viewModel.Move_Task();
            }
        }
        /// <summary>
        /// open window to add task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Task_Click(object sender, RoutedEventArgs e)
        {

            AddNewTaskView adt = new AddNewTaskView(viewModel.User, viewModel.BoardModel);
            adt.Show();
            this.Close();
        }
        /// <summary>
        /// return to home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomePage_Click(object sender, RoutedEventArgs e)
        {
            HomePageView homePage = new HomePageView(user);
            homePage.Show();
            this.Close();
        }
        /// <summary>
        /// open windwo to update task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterTaskUpdate(Object sender, MouseEventArgs e)
        {
            TaskModel task = findSelectedTaskFromPropertyAndColumn(); 
            if (task != null && task.ColumnOrdinal < viewModel.BoardModel.Columns.Count-1 && checkEmailAssignee(task))
            {
                ///
                UpdateTaskView updateTaskView = new UpdateTaskView(user, viewModel.BoardModel, task, task.ColumnOrdinal);
                updateTaskView.Show();
                this.Close();
            }
        }
        /// <summary>
        /// find the property of the wanted selected task
        /// </summary>ב
        /// <returns></returns>
        private TaskModel findSelectedTaskFromProperty()
        {
            foreach (ColumnModel CM in viewModel.BoardModel.Columns)
            {
                if (CM.SelectedTask != null)
                    return CM.SelectedTask;
                foreach (TaskModel TM in CM.Tasks)
                {
                    if (TM.TaskPropertySelected != null)
                    {
                        return TM;
                    }
                }
            }
            return null;
        }
        private TaskModel findSelectedTaskFromPropertyAndColumn()
        {
            foreach (ColumnModel CM in viewModel.BoardModel.Columns)
            {
                if (CM.SelectedTask != null)
                    return CM.SelectedTask;
                foreach (TaskModel TM in CM.Tasks)
                {
                    if (TM.TaskPropertySelected != null && viewModel.BoardModel.Columns.IndexOf(CM) < viewModel.BoardModel.Columns.Count - 1)
                    {
                        return TM;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// open member list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Member_Click(object sender, RoutedEventArgs e)
        {
            MemberListView member = new MemberListView(user, b);
            member.Show();
            this.Close();
        }
        private bool checkEmailAssignee(TaskModel task)
        {
            if (task.Assignee.Equals(user.Email))
                return true;
            return false;
        }
        
    }
}
