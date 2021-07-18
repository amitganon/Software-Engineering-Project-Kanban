using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /*
`   The main DColumn class
Contains all methods for performing DColumn's actions.
*/
    /// <summary>
    /// The main DColumn class.
    /// Contains all methods for performing DColumn's actions.
    /// </summary>
    /// <remarks>
    /// this class performing object column in the db.
    /// </remarks>
    //internal class DBoard : DTO
    public class DColumn : DTO
    {
        public const string ColumnOrdinalColumn = "columnOrdinal";
        public const string ColumnNameColumn = "name";
        public const string TaskLimitColumn = "taskLimit";


        private int _columnOrdinal;
        public int ColumnOrdinal { get => _columnOrdinal; set {  ((DalColumnController)_controller).Update(Id, _columnOrdinal, ColumnOrdinalColumn,Name, value); _columnOrdinal = value; } }

        private string _name;
        public string Name { get => _name; set { _name = value; ((DalColumnController)_controller).Update(Id, _columnOrdinal, ColumnNameColumn, value); } }

        private int _taskLimit;
        public int TaskLimit { get => _taskLimit; set { _taskLimit = value; ((DalColumnController)_controller).Update(Id, _columnOrdinal, TaskLimitColumn, Name, value); } }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>the Dcolumn constructor</summary>
        public DColumn (int id , int columnOrdinal , string name , int taskLimit) : base(new DalColumnController(), id)
        {
            this._columnOrdinal = columnOrdinal;
            this._name = name;
            this._taskLimit = taskLimit;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        /// <summary>
        /// insert column to db.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown if error accured when try to insert column to DB.</exception>
        /// <returns>true or false if column was added</returns>
        public bool Insert()
        {
            if (((DalColumnController)_controller).Insert(this))
            {
                log.Info($"DColumn added to DB, column Oridnal is -{_columnOrdinal} , boardid {Id}");
                return true;
            }
            throw new ArgumentException($"error when try to insert column to DB, column Oridnal is -{_columnOrdinal} and board id{Id}");
        }


        /// <summary>
        /// delete column from db.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown if error accured when try to delete column from DB.</exception>
        /// <returns>true or false if column was deleted</returns>

        public bool Delete()
        {
            if (((DalColumnController)_controller).DeleteC(this))
            {
                log.Info($"DColumn removed from DB, column Oridnal is -{_columnOrdinal} , boardid {Id}");
                return true;
            }
            log.Error($"exception in Delet Dboard from the db, column Oridnal is -{_columnOrdinal} , boardid {Id}");
            throw new ArgumentException($"exception in Delet Dboard from the db, column Oridnal is -{_columnOrdinal} , boardid {Id}");
        }
    }
}
