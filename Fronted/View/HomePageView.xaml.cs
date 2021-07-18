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
    /// Interaction logic for HomePageView.xaml
    /// </summary>
    public partial class HomePageView : Window
    {
        private UserModel user;
        public UserModel User { get=> user; set { user = value; } }
        private HomePageViewModel homePageViewModel;
        public bool addBoardNameFieldPressed = false;
        public bool joinBoardNameFieldPressed = false;
        public bool joinBoardEmailFieldPressed = false;
        public HomePageView(UserModel u)
        {
            InitializeComponent();
            homePageViewModel = new HomePageViewModel(u);
            this.DataContext = homePageViewModel;
            User = u;
            AddBoardText.Text = "Enter a board name:";
            JoinBoardNameText.Text = "Enter the board's name:";
            CreatorEmailText.Text = "Enter the board's creator Email:";
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            homePageViewModel.LogOut();
            LogInView login = new LogInView();
            login.Show();
            this.Close();
        }
        private void OnWindowClick(Object sender, MouseEventArgs e)
        {
            if (!AddBoardText.IsMouseOver)
            {
                EmptyTXT.Focus();
            }
        }

        private void EnterBoardClick(Object sender, MouseEventArgs e)
        {
            if (homePageViewModel.SelectedBoard != null) 
            {
                BoardPage board = new BoardPage(homePageViewModel.SelectedBoard, User);
                board.Show();
                this.Close();
            }
        }

        private void AddBoard_Click(object sender, RoutedEventArgs e)
        {
            if (homePageViewModel.AddBoardClicked)
                homePageViewModel.AddBoardClicked = false;
            else
                homePageViewModel.AddBoardClicked = true;
        }

        private void AddBoardText_Click(object sender, RoutedEventArgs e)
        {
            addBoardNameFieldPressed = true;

            if (AddBoardText.Text == "Enter a board name:")
            {
                if (addBoardNameFieldPressed == true)
                {
                    AddBoardText.Text = "";
                }
            }
        }

        private void AddBoardText_Leave(object sender, RoutedEventArgs e)
        {
            addBoardNameFieldPressed = false;
            if (AddBoardText.Text == "")
                AddBoardText.Text = "Enter a board name:";
        }

        private void AddSubmit_Click(object sender, RoutedEventArgs e)
        {
            homePageViewModel.AddBoard();
        }

        private void JoinBoard_Click(object sender, RoutedEventArgs e)
        {
            if (homePageViewModel.JoinBoardClicked)
                homePageViewModel.JoinBoardClicked = false;
            else
                homePageViewModel.JoinBoardClicked = true;
        }

        private void JoinBoardNameText_Click(object sender, RoutedEventArgs e)
        {
            joinBoardNameFieldPressed = true;

            if (JoinBoardNameText.Text == "Enter the board's name:")
            {
                if (joinBoardNameFieldPressed == true)
                {
                    JoinBoardNameText.Text = "";
                }
            }
        }
        private void JoinBoardNameText_Leave(object sender, RoutedEventArgs e)
        {
            joinBoardNameFieldPressed = false;
            if (JoinBoardNameText.Text == "")
                JoinBoardNameText.Text = "Enter the board's name:";
        }

        private void CreatorEmailText_Click(object sender, RoutedEventArgs e)
        {
            joinBoardEmailFieldPressed = true;

            if (CreatorEmailText.Text == "Enter the board's creator Email:")
            {
                if (joinBoardEmailFieldPressed == true)
                {
                    CreatorEmailText.Text = "";
                }
            }
        }

        private void CreatorEmailText_Leave(object sender, RoutedEventArgs e)
        {
            joinBoardEmailFieldPressed = false;
            if (CreatorEmailText.Text == "")
                CreatorEmailText.Text = "Enter the board's creator Email:";
        }

        private void JoinSubmit_Click(object sender, RoutedEventArgs e)
        {
            homePageViewModel.JoinBoard();
        }

        private void AddKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                homePageViewModel.AddBoard();
        }

        private void JoinKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                homePageViewModel.JoinBoard();
        }

        private void InProgress_Click(object sender, RoutedEventArgs e)
        {
            InProgressTasksView inProgress = new InProgressTasksView(User, homePageViewModel.UserBoards);
            inProgress.Show();
            this.Close();
        }

    }
}
