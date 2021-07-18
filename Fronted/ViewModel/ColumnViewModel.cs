using Fronted.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronted.ViewModel
{
    public class ColumnViewModel : NotifiableModelObject
    {
        public ColumnModel columnModel { get; set; }

        public ColumnViewModel(UserModel u) : base(u.Controller)
        {
            this.Controller = u.Controller;
            columnModel = new(u.Controller,"",-1,0);

        }
        public string Create_Column(string UserEmail , BoardModel b)
        {
            string m = columnModel.Create_Column(UserEmail, b.CreatorEmail, b.Name);
            b.Add_Column(columnModel);
            return m;
             
        }
    }
}
