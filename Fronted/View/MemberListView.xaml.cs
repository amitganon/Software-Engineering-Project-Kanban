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
    /// Interaction logic for MemberListView.xaml
    /// </summary>
    public partial class MemberListView : Window
    {
        UserModel user;
        private MemberListViewModel memberListViewModel;
        private BoardModel board;

        ///<summary>the constructor </summary>
        public MemberListView(UserModel user, BoardModel board)
        {
            InitializeComponent();
            this.user = user;
            this.memberListViewModel = new MemberListViewModel(user, board);
            this.DataContext = memberListViewModel;
            this.board = board;
        }

        ///<summary>open HomePage window </summary>
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
            memberListViewModel.LogOut(user.Email);
            LogInView LogInView = new LogInView();
            LogInView.Show();
            this.Close();
        }

        ///<summary>open board page window </summary>
        private void BackToBoard_Click(object sender, RoutedEventArgs e)
        {
            BoardPage boardPage=new BoardPage (board,user);
            boardPage.Show();
            this.Close();
        }
    }
}
