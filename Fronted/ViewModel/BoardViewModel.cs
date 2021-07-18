using Fronted.Model;
using Frontend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Fronted.ViewModel
{
    public class BoardViewModel : NotifiableModelObject
    {
        public UserModel User { get; set; }
        public BoardModel BoardModel { get; set; }

        private string message;
        public string Message
        {
            get => message;
            set
            {
                this.message = value;
                RaisePropertyChanged("Message");
            }
        }
        public BoardViewModel(BoardModel b, UserModel u) : base (u.Controller)
        {
            this.Controller = u.Controller;
            this.User = u;
            this.BoardModel = b;
            this.Message = "";

        }
        /// <summary>
        /// logout from system
        /// </summary>
        internal void Logout()
        {
            try
            {
                User.Controller.Logout(User.Email);
                Message= "";
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        /// <summary>
        /// change column name
        /// </summary>
        /// <param name="s"></param>
        public void Change_Column_Name(string s)
        {
            Message = BoardModel.Change_Column_Name(s);
        }
        /// <summary>
        /// cahnge column limit
        /// </summary>
        /// <param name="s"></param>
        public void Change_Column_Limit(int s)
        {
            
            Message = BoardModel.Change_Column_Limit(s);

            
        }
        /// <summary>
        /// remove board
        /// </summary>
        public bool RemoveBoard()
        {
            Message = BoardModel.RemoveBoard();
            if (Message.Length != 0)
                return false;
            return true;
        }
        /// <summary>
        /// move colum right
        /// </summary>
        public void Move_Column_Right()
        {
            Message = BoardModel.Move_Column_Right();
        }
        /// <summary>
        /// move column left
        /// </summary>
        public void Move_Column_Left()
        {
            Message = BoardModel.Move_Column_Left();
        }
        /// <summary>
        /// delte column
        /// </summary>
        /// <param name="deletedColumn"></param>
        public void Delete_Column(int deletedColumn)
        {
            Message = BoardModel.Delete_Column(deletedColumn);
        }
        /// <summary>
        /// search and show all tasks inclode the keyword typed in
        /// </summary>
        /// <param name="sender"></param>
        public void Search_By_Key_Word(string sender)
        {
            BoardModel.Search_By_Key_Word(sender);
        }
        /// <summary>
        /// sort the task in specific column by due date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DueDateOrder()
        {
            BoardModel.DueDateOrder();
        }
        /// <summary>
        /// move task
        /// </summary>
        public void Move_Task()
        {
            BoardModel.Move_Task();
        }
    }
}
