using Fronted.Model;
using Fronted.View;
using Frontend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Fronted.ViewModel
{
    class HomePageViewModel : NotifiableObject
    {
        public BackendController controller { get; private set; }
        private string userEmailShow;
        public string UserEmailShow { get => userEmailShow; set { userEmailShow = value; RaisePropertyChanged("UserEmailShow"); }}
        private UserModel user;
        public UserModel User { get => user; set { user = value; } }
        private bool joinBoardClicked = false;
        public bool JoinBoardClicked { get => joinBoardClicked; set { joinBoardClicked = value; RaisePropertyChanged("JoinBoardClicked"); } }
        private bool addBoardClicked = false;
        public bool AddBoardClicked { get => addBoardClicked; set { addBoardClicked = value; RaisePropertyChanged("AddBoardClicked"); } }
        private string newBoardName;
        public string NewBoardName { get => newBoardName; set { newBoardName = value; RaisePropertyChanged("NewBoardName"); } }
        private string joinedBoardName;
        public string JoinedBoardName { get => joinedBoardName; set { joinedBoardName = value; RaisePropertyChanged("JoinedBoardName"); } }
        private string joinedBoardEmail;
        public string JoinedBoardEmail { get => joinedBoardEmail; set { joinedBoardEmail = value; RaisePropertyChanged("JoinedBoardEmail"); } }
        private BoardModel selectedBoard;
        public BoardModel SelectedBoard { get => selectedBoard; set { selectedBoard = value; RaisePropertyChanged("SelectedBoard"); } }
        private ObservableCollection<BoardModel> userBoards;
        public ObservableCollection<BoardModel> UserBoards { get => userBoards; }

        public HomePageViewModel(UserModel user)
        {
            controller = user.Controller;
            User = user;
            UserEmailShow = user.Email;
            //boardHolder = new BoardHolderModel(user);
            userBoards = new ObservableCollection<BoardModel>(controller.GetUserBoards(user));
        }

        ///<summary>Logout a user. calling the BackendController to pass the action.</summary>
        public void LogOut()
        {
            controller.Logout(User.Email);
        }

        ///<summary>Add a new board to the system. calling the BackendController to pass the action.Then adds it to the observerable userBoards. </summary>
        public void AddBoard()
        {
            try
            {
                controller.AddBoard(User.Email, NewBoardName);
                UserBoards.Add(controller.GetBoard(User, User.Email, NewBoardName));
                MessageBox.Show(NewBoardName, "Added a new Board!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "There was a problem", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        ///<summary>Join a board. calling the BackendController to pass the action.Then adds it to the observerable userBoards. </summary>
        public void JoinBoard()
        {
            try
            {
                controller.JoinBoard(User.Email, JoinedBoardName, JoinedBoardEmail);
                UserBoards.Add(controller.GetBoard(User, JoinedBoardEmail, JoinedBoardName));
                MessageBox.Show(JoinedBoardName, "Joined a new Board!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "There was a problem", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
