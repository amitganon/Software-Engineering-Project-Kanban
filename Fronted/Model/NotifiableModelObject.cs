using Fronted.Model;
using Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fronted
{
    public class NotifiableModelObject : NotifiableObject
    { 
        public BackendController Controller { get;  set; }
        protected NotifiableModelObject(BackendController controller)
        {
            this.Controller = controller;
        }
        
    }
}
