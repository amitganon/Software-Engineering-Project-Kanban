using Fronted.Model;
using Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fronted.ViewModel
{
    class RegisterViewModel : NotifiableObject
    {
        public BackendController controller { get; private set; }
        private string message;
        private bool _isMouseOver;
        private string userEmail;
        private string password;
        public bool IsMouseOver { get => _isMouseOver; set { _isMouseOver = value; RaisePropertyChanged("IsMouseOver"); } }
        public RegisterViewModel()
        {
            controller = new BackendController();
            _isMouseOver = false;
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

        public string UserEmail
        {
            get => userEmail;
            set
            {
                this.userEmail = value;
                RaisePropertyChanged("UserEmail");
            }
        }

        public string Password
        {
            get => password;
            set
            {
                this.password = value;
                RaisePropertyChanged("Password");
            }
        }

        ///<summary>
        ///Register a new user to the system. calling the BackendController to pass the action.
        ///</summary>
        ///<returns>return true if registered successfully</returns>
        public bool Register()
        {
            Message = "";
            try
            {
                controller.Register(UserEmail, Password);
                MessageBox.Show("Registered Successfully!", "Welcome to TAY-Kanban!", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
        }
    }
}
