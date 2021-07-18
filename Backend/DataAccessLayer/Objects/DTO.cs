using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /*
    The main DTO class
    the class DTO is abstract to all Objects in Data Accses Layer
    */
    /// <summary>
    /// The main DTO class.
    /// </summary>
    /// <remarks>
    /// Holds Id and Controller
    /// </remarks>
    public abstract class DTO
    {
        public const string IDColumnName = "ID";
        private int id;
        public int Id { get => id; }
        protected DalController _controller;
        public DalController _Controller { get; }
        public DTO(DalController controller, int id)
        {
            this.id = id;
            _controller = controller;
        }
    }
}