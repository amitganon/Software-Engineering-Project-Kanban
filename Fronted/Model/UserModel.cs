using Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Fronted.Model
{
    public class UserModel : NotifiableModelObject
    {
        private string userEmail;
        public string Email
        {
            get => userEmail;
            set
            {
                userEmail = value;
                RaisePropertyChanged("Email");
            }
        }
       public UserModel(BackendController c, string userEmail) : base(c)
       {
            this.userEmail = userEmail;
       }
    }
}
