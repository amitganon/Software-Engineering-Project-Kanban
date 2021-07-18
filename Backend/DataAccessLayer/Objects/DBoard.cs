using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{

    /*
`   The main DBoard class
    Contains all methods for performing Dboard's actions.
    */
    /// <summary>
    /// The main DBoard class.
    /// Contains all methods for performing Dboard's actions.
    /// </summary>
    /// <remarks>
    /// this class performing object board in the db.
    /// </remarks>
    //internal class DBoard : DTO
    public class DBoard : DTO
    {
        public const string BoardNameColumn = "BoardName";
        public const string BoardCreatorEmailColumn = "BoardCreatorEmail";
        public const string BoardTaskTopColumn = "TaskTop";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string name;
        public string Name { get {return name; } }
        private string creatorEmail;
        public string CreatorEmail { get {return creatorEmail; } }
        private int taskTop;
        public int TaskTop { get => taskTop; set { taskTop = value; ((DalBoardController)_controller).Update(Id, BoardTaskTopColumn, value); } }


        /// <summary>the Dboard constructor</summary>
        public DBoard(int idBoard, string name, string creatorEmail, int taskTop): base(new DalBoardController(), idBoard)
        {
            this.name = name;
            this.creatorEmail = creatorEmail;
            this.taskTop = taskTop;
            log.Info($"DBoard created, id-{idBoard}");
        }


        /// <summary>
        /// insert board to db.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown if error accured when try to insert board to DB.</exception>
        /// <returns>true or false if board was added</returns>
        public bool Insert()
        {
            if (((DalBoardController)_controller).Insert(this)) 
            {
                log.Info($"DBoard added to DB, id-{Id}");
                return true;
            }
            throw new ArgumentException($"error when try to insert board to DB, board id{Id}");
        }


        /// <summary>
        /// delete board from db.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown if error accured when try to delete board from DB.</exception>
        /// <returns>true or false if board was deleted</returns>
        public bool Delete()
        {
            if(((DalBoardController)_controller).Delete(this))
            { 
                log.Info($"DBoard removed from DB, id-{Id}");
                return true;
            }
            log.Error($"exception in Delet Dboard from the db, board id-{Id}");
            throw new ArgumentException($"exception in Delet Dboard from the db, board id-{Id}");
        }
    }
}
