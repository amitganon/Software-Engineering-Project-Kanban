using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /*
`   The main boardController class
    Contains all methods for performing boardController's actions.
    */
    /// <summary>
    /// The main boardController class.
    /// Contains all methods for performing boardController's functions.
    /// </summary>
    /// <remarks>
    /// This class holds the list of all the boards in the system, the class maintain this list(add, delete, get).
    /// </remarks>
    public class BoardController
    {
        private int boardTop;
        private UserController uc;
        private Dictionary<int,Board> boardDict; //BoardDict(id,board)
        private Dictionary<(string,string),int> IdDict;//UserBoards((creatorEmail,boardName),boardId)
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        ///<summary>the constructor of boardController.</summary>
        ///<param name="uc">the UserController.</param>
        public BoardController(UserController uc)
        {
            this.uc = uc;
            boardDict = new Dictionary<int,Board>();
            IdDict = new Dictionary<(string,string),int>();
            boardTop = 0;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("a BoardController was created");
        }

        ///<summary>
        ///Returns the boardTop field.
        ///</summary>
        public int BoardTop { get => boardTop; }


        ///<summary>
        ///Returns a the boards to which the user has subscribed.
        ///</summary>
        ///<param name="userEmail">the user email.</param>
        ///<param name="creatorEmail">the creatore's email of the board.</param>
        ///<param name="boardName">the board name.</param>
        ///<returns>return the board by user email and board Name</returns>
        public Board GetBoard(string userEmail, string creatorEmail,string boardName)
        {
            ValidLogIn(userEmail, "GetBoard");
            int id = ConvertToId(creatorEmail, boardName);
            log.Debug($"GetBoard return a board to {userEmail}, board creator email-{creatorEmail}, board name-{boardName}");
            Board b = boardDict.GetValueOrDefault(id);
            b.ValidMember(userEmail,"getBoard in BoradController");
            return b;
        }


        ///<summary>
        ///Adds a new board to boardController.
        ///</summary>
        ///<param name="creatorEmail">the user Email, the boards creator.</param>
        ///<param name="boardName">the board Name.</param>
        ///<exception cref="System.ArgumentException">Thrown when boardName input is null.</exception>
        ///<exception cref="System.ArgumentException">Thrown when the board is already in the system.</exception>
        public void AddBoard(string creatorEmail, string boardName)
        {
            ValidLogIn(creatorEmail, "AddBoard");
            if (boardName == null)
            {
                log.Error("AddBoard got a null boardName for an input");
                throw new ArgumentNullException("AddBoard got a null boardName for an input");
            }
            if (IdDict.ContainsKey((creatorEmail, boardName)))
            {
                log.Error($"can't add this board because this specific board's key is already in the system");
                throw new ArgumentException("You already added a board with this name");
            }
            Board board = new Board(boardName, boardTop, creatorEmail);
            board.ValidMember(creatorEmail, "AddBoard in BoradController");
            //update dicts
            boardDict.Add(boardTop, board);
            IdDict.Add((creatorEmail, boardName), boardTop);
            log.Debug("boardDict and IdDict were updated that a new board was created");

            //update board and user
            board.AddUserToList(creatorEmail);
            uc.GetUser(creatorEmail).AddBoardToList(boardTop);

            //update DMember
            DMember DMember = new DMember(boardTop, creatorEmail);
            DMember.Insert();

            //update DalBoard
            board.DBoard.Insert();
            log.Info($"A new board was added to the boardController. user:{creatorEmail}. boardId:{boardTop}. boardName:{boardName}");
            boardTop = boardTop + 1;
            log.Debug($"new board top: '{boardTop}'");
        }


        ///<summary>
        ///Removes a board from boardController.
        ///</summary>
        ///<param name="creatorEmail">the creatorEmail of the board.</param>
        ///<param name="boardName">the board name.</param>
        ///<param name="userEmail">the user email that try to remove the board.</param>
        ///<exception cref="System.ArgumentException">Thrown when userEmail try to remove board that he not created.</exception>
        public void RemoveBoard(string creatorEmail, string boardName,string userEmail)
        {
            int boardId=ConvertToId(creatorEmail, boardName);
            ValidLogIn(userEmail, "RemoveBoard");
            if (creatorEmail!= userEmail)
            {
                log.Error($"userEmail try to remove board that he not created, userEmail-{userEmail}, creatorEmail-{creatorEmail}, boardId-{boardId}");
                throw new ArgumentException($"userEmail try to remove board that he not created, userEmail-{userEmail}, creatorEmail-{creatorEmail}, boardId-{boardId}");
            }
            Board board = boardDict.GetValueOrDefault(boardId);
            if (board.CreatorEmail != creatorEmail)
            {
                log.Error($"Can't remove the board: email Creator input is {creatorEmail} , the creator is {board.CreatorEmail}: only the creator of the borad can delete board");
                throw new ArgumentException($"Can't remove the board: email Creator input is {creatorEmail} , the creator is {board.CreatorEmail}: only the creator of the borad can delete board");
            }

            board.DeleteColumns();

            //update DMember and user
            foreach (string email in board.MembersList)
            {
                uc.GetUser(email).DeleteBoardFromList(boardId);

            }
            DMember member = new DMember(boardId, creatorEmail);
            member.Delete();
            //update DalBoard
            board.DBoard.Delete();

            //update dict
            boardDict.Remove(boardId);
            IdDict.Remove((userEmail,boardName));
            log.Debug("boardDict and IdDict was update that the board removed");
            log.Info($"this board removed from boardController,boardId-{BoardTop}");
        }


        /// <summary>
        /// join user to a board 
        /// </summary>
        /// ///<param name="userEmail">the user email that try to join to the board.</param>
        ///<param name="creatorEmail">the creatorEmail of the board.</param>
        ///<param name="boardName">the board name.</param>        
        /// <exception cref="System.ArgumentException">Thrown when the user try to join a board that he alredy member to.</exception>
        public void JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            ValidLogIn(userEmail, "JoinBoard");
            int id=ConvertToId(creatorEmail, boardName);
            if(boardDict.GetValueOrDefault(id).MembersList.Contains(userEmail))
            {
                log.Error($"try to join a board that the user alredy member to. creatorEmail-{creatorEmail}, boardName-{boardName}");
                throw new ArgumentException("You already are a member of this board");
            }

            //update user
            uc.GetUser(userEmail).AddBoardToList(id);
            //update board
            boardDict.GetValueOrDefault(id).AddUserToList(userEmail);
            //update DMember            
            DMember DMember = new DMember(id, userEmail);
            DMember.Insert();
            log.Info($"A user has joined to the board, userEmail-{userEmail}, boardId-{id}");
        }

        /// <summary>
        /// Returns the names of all the boards to which the user has subscribed
        /// </summary>
        /// <param name="userEmail">the user email</param>
        /// <returns>return list of board names</returns>
        public IList<String> GetBoarsdNames(string userEmail)
        {
            ValidLogIn(userEmail, "GetBoardsName");
            IList<string> names = new List<string>();
            foreach (int id in uc.GetUser(userEmail).BoardList)
            {
                names.Add(boardDict.GetValueOrDefault(id).Name);
            }
            log.Info($"Succecfully commited GetBoardNames, user email:'{userEmail}'");
            return names;
        }

        /// <summary>
        /// convert user email and board name to id
        /// </summary>
        /// <param name="creatorEmail">the creator email of the board</param>
        /// <param name="boardName">the board name</param>      
        /// <exception cref="System.ArgumentNullException">Thrown when the creatorEmail that entered is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when the boardName that entered is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when ther isn't a board with this creation email or name in the system.</exception>
        /// <returns>return the ID of the board</returns>
        private int ConvertToId(string creatorEmail,string boardName)
        {
            if (creatorEmail == null)
            {
                log.Error("ConvertToId got a null userEmail for input.");
                throw new ArgumentNullException("ConvertToId got a null userEmail for input.");
            }
            if (boardName == null)
            {
                log.Error("ConvertToId got a null boardName for input.");
                throw new ArgumentNullException("ConvertToId got a null boardName for input.");
            }
            if (!IdDict.ContainsKey((creatorEmail, boardName)))
            {
                log.Error("ConvertToId got a non-existed board fields, the key isn't contained in dict");
                throw new ArgumentException("Couldn't find the board, Make sure you enetered the right details.");
            }
            int boardId = IdDict.GetValueOrDefault((creatorEmail, boardName));
            log.Debug($"ConvertToId return the Id of the board , creatorEmai:'{creatorEmail}',boardName:'{boardName}', boardId:'{boardId}'");
            return boardId;            
        }


        /// <summary>
        /// return all the tasks that at InProgress column and the user is assigned to
        /// </summary>
        /// <param name="userEmail">the user email</param>
        /// <returns>return all the tasks that at InProgress column and the user is assigned to</returns>
        public IList<Task> InProgressTasks(string userEmail)
        {
            ValidLogIn(userEmail, "InProgressTasks");
            List<int> Id = uc.GetUser(userEmail).BoardList;
            IList<Task> result = new List<Task>();
            foreach(int id in Id){
                Board b = boardDict.GetValueOrDefault(id);
                foreach (Task t in b.inProgressTasks(userEmail))
                    result.Add(t);
            }
            log.Info($"board controller InProgressTasks return a list with {result.Count} tasks by user email-{userEmail}");
            return result;
        }

        ///////////////LOADS////////////

        /// <summary>
        /// load all the boards in the db, calling LoadMembers, LoadColumns
        /// </summary>
        public void LoadBoards()
        {
            IList<DBoard> myList = new List<DBoard>();
            DalBoardController DBC = new DalBoardController();
            myList = DBC.LoadBoards();
            foreach (DBoard DBoard in myList)
            {                
                Board tmpBoard = new Board(DBoard.Name, DBoard.Id, DBoard.CreatorEmail,DBoard.TaskTop);
                tmpBoard.LoadColumns();
                //update dicts
                boardDict.Add(DBoard.Id, tmpBoard);
                IdDict.Add((DBoard.CreatorEmail, DBoard.Name),DBoard.Id);
            }
            int boartTopTemp = myList.Count;
            if (boartTopTemp  == -1)
            {
                boardTop = 0;
            }
            else boardTop = boartTopTemp;
            log.Info($"all of the boards are insert to businessLayer BoardController");
            //uploade members and tasks
            LoadBoardMembers();
        }


        /// <summary>
        /// load all the board's members in the db
        /// </summary>
        private void LoadBoardMembers()
        {
            List<DMember> DMembers = new List<DMember>();
            DMembers = new DalMemberController().LoadBoardMembers();
            foreach (DMember member in DMembers)
            {
                Board b = boardDict.GetValueOrDefault(member.Id);
                //update board
                b.AddUserToList(member.UserEmail);
                //update user
                uc.GetUser(member.UserEmail).AddBoardToList(member.Id);
            }
            log.Info("all of the boards members are insert to businessLayer BoardController");
        }



        /// <summary>
        /// delete all the board,members and tasks in the db
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when can't delete dal memebrs.</exception>
        /// <exception cref="System.ArgumentException">Thrown when can't delete dal boards.</exception>
        /// <exception cref="System.ArgumentException">Thrown when can't delete dal tasks.</exception>
        public void DeleteAllBoards()
        {
            if (!new DalMemberController().DeleteAll()) 
            {
                log.Error("cant delete from DalMemberController");
                throw new ArgumentException("");
            }
            if(!new DalBoardController().DeleteAll())
            {
                log.Error("cant delete from DalBoardController");
                throw new ArgumentException("");
            }
            if (!new DalColumnController().DeleteAll())
            {
                log.Error("cant delete from DalColumnController");
                throw new ArgumentException("");
            }
            if (!new DalTaskController().DeleteAll())
            {
                log.Error("cant delete from DalTaskController");
                throw new ArgumentException("");
            }
            log.Info("all the board, members, columns and tasks are deleted from the db");
        }

        /// <summary>
        /// Checks if the user is logged before commiting action
        /// </summary>
        /// <param name="userEmail">The email address that needs to be checked</param>
        /// <param name="funcName">The name of the function which check if the user is logged in</param>
        /// <remarks> This function uses the userController's function </remarks>
        private void ValidLogIn(string userEmail, string funcName)
        {
            if (userEmail == null)
            {
                log.Error($"{funcName} got a null userEmail for an input");
                throw new ArgumentNullException($"{funcName} got a null userEmail for an input");
            }
            if (!uc.validLogin(userEmail))
            {
                log.Error($"{funcName} action can't commit with a looged out user. The user: {userEmail}");
                throw new ArgumentException($"{userEmail} user is not logged in");
            }
        }

        /// <summary>
        /// The function receives a email and gets all the boards that the email's user is a member of
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <returns>A list of boards (of the user as a member), The response should contain a error message in case of an error</returns>
        public List<Board> GetUserBoards(string userEmail)
        {
            ValidLogIn(userEmail, "GetUserBoards");
            User myUser = uc.GetUser(userEmail);
            List<int> idBoards = myUser.BoardList;
            List<Board> allBoards = new();
            foreach(int boardId in idBoards)
            {
                allBoards.Add(boardDict.GetValueOrDefault(boardId));
            }
            return allBoards;
        }
    }
}
