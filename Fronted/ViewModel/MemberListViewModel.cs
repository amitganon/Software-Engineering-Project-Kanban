using Fronted.Model;
using Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronted.ViewModel
{
    class MemberListViewModel:NotifiableObject
    {
        public BackendController controller { get; private set; }
        private string userName;
        private BoardModel board;
        public string title;
        public string Title { get => title; set { title = value; RaisePropertyChanged("Title"); } }
        public BoardModel Board { get=>board; }

        ///<summary>
        ///the constructor
        ///</summary>
        public MemberListViewModel(UserModel user, BoardModel board)
        {
            title = "Board Member's List";
            this.board = board;
            UserName = user.Email;
            controller = user.Controller;
        }

        public string UserName
        {
            get => userName;
            set
            {
                this.userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        ///<summary>
        ///log out from the sistem
        ///</summary>
        ///<param name="userEmail">the user email</param>
        public void LogOut(string userEmail)
        {
            controller.Logout(userEmail);
        }
    }
}
