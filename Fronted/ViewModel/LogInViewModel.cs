using Fronted.Model;
using Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Fronted.ViewModel
{
    class LogInViewModel : NotifiableObject
    {
        public BackendController controller { get; private set; }
        private string message;
        public string title;
        public string Title   { get => title; set { title = value; RaisePropertyChanged("Title"); } } 
        private string userName;
        private string password;
        private string passwordLength;
        private bool isEyeClicked;
        public bool IsEyeClicked { get => isEyeClicked; set { isEyeClicked = value; RaisePropertyChanged("IsEyeClicked"); RaisePropertyChanged("TextBoxColor"); RaisePropertyChanged("TextBlockColor"); } }
        private bool isMouseOverEye;
        public bool IsMouseOverEye { get => isMouseOverEye; set { isMouseOverEye = value; RaisePropertyChanged("IsMouseOverEye"); } }
        public SolidColorBrush TextBlockColor { get { return IsEyeClicked ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Colors.Black); } }

        public SolidColorBrush TextBoxColor { get { return IsEyeClicked ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.Transparent); } }

        ///<summary>
        ///lthe constructor
        ///</summary>
        public LogInViewModel()
        {
            try
            {
                controller = new BackendController();
                Title = "Welcome to Yoav Amit Tal Kanban";
                UserName = "u2@gmail.com";
                Password = "123123Aa";
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error Occured while loading the Program");
            }
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

        public string Password
        {
            get => password;
            set
            {
                this.password = value;
                RaisePropertyChanged("Password");
                RaisePropertyChanged("PasswordLength");
            }
        }

        public string PasswordLength
        {
            get 
            {
                string result = "";
                for (int i = 0; i < password.Length; i++)
                    result += "*";
                return result; 
            } 
        }

        public string Message
        {
            get => message;
            set
            {
                this.message = value;
                RaisePropertyChanged("Message");
            }
        }

        ///<summary>
        ///log in to the system
        ///</summary>
        public UserModel LogIn()
        {
            Message = "";
            try
            {
                return controller.LogIn(userName, password);
            }
            catch(Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
    }
}
