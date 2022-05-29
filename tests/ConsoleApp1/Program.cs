using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("start test");
            ///  DalUserController duc = new DalUserController("Users");
            //DUser d = new DUser("tal9363@gmail.com", "318416575", 1);
            // duc.InsertUser(d); // work

            Service service = new();
            service.Register("tal9363@gmail.com", "Tal318416575");
            Console.WriteLine("finish test");
        }
    }
}
