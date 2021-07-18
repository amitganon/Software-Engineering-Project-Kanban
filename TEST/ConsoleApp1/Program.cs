using System;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {

        public static void Main(string[] args)
        {
            
            Console.WriteLine("start test");
            Service myService = new();
            myService.DeleteData();
            /*
            myService.Register("yoav@gmail.com", "Aa1234");
            myService.Login("yoav@gmail.com", "Aa1234");
            myService.Register("amit@gmail.com", "Aa1234");
            myService.Login("amit@gmail.com", "Aa1234");
            myService.AddBoard("yoav@gmail.com", "testBoard");
            DateTime date = new DateTime(2025, 3, 1, 7, 0, 0);
            myService.AddTask("yoav@gmail.com", "yoav@gmail.com", "testBoard", "testTask", "", date);
            Response<IList<IntroSE.Kanban.Backend.ServiceLayer.Task>> r = myService.InProgressTasks("amit@gmail.com");
            myService.JoinBoard("amit@gmail.com", "yoav@gmail.com", "testBoard");
            //myService.AdvanceTask("amit@gmail.com", "yoav@gmail.com", "testBoard", 0, 0);
            myService.AdvanceTask("yoav@gmail.com", "yoav@gmail.com", "testBoard", 0, 0);
            Response<IList<IntroSE.Kanban.Backend.ServiceLayer.Task>> re = myService.InProgressTasks("yoav@gmail.com");
            Console.WriteLine(re.Value.Count != 0);
            Console.WriteLine("end test");

            /*
            Console.WriteLine("start test");
            Service myService = new();
            myService.LoadData();
            myService.Login("yoav@gmail.com", "Aa1234");
            myService.AssignTask("yoav@gmail.com", "noa@gmail.com", "noa Board 1", 0, 6, "yoav@gmail.com");
            Console.WriteLine();
            /*
//"yoav@gmail.com"
//"yair@gmail.com"
//"et@gmail.com"
//"noa@gmail.com"
/*
            myService.Register("yoav@gmail.com", "Aa1234");
            myService.Register("yair@gmail.com", "Bb1234");
            myService.Register("et@gmail.com", "Cc1234");
            myService.Register("noa@gmail.com", "Dd1234");
            myService.Login("yoav@gmail.com", "Aa1234");
            myService.Login("yair@gmail.com", "Bb1234");
            myService.Login("et@gmail.com", "Cc1234");
            myService.Login("noa@gmail.com", "Dd1234");
            myService.Logout("yair@gmail.com");
            myService.AddBoard("yoav@gmail.com", "yoav Board 1");
            myService.AddBoard("yoav@gmail.com", "yoav Board 2");
            myService.AddBoard("yoav@gmail.com", "yoav Board 3");
            myService.AddBoard("yair@gmail.com", "yair Board 1");
            myService.AddBoard("yair@gmail.com", "yair Board 2");
            myService.AddBoard("yair@gmail.com", "yair Board 3"); 
            myService.AddBoard("et@gmail.com", "et Board 1");
            myService.AddBoard("et@gmail.com", "et Board 2");
            myService.AddBoard("et@gmail.com", "et Board 3"); 
            myService.AddBoard("noa@gmail.com", "noa Board 1");
            myService.AddBoard("noa@gmail.com", "noa Board 2");
            myService.AddBoard("noa@gmail.com", "noa Board 3");
            myService.AddTask("noa@gmail.com", "noa@gmail.com", "noa Board 1", "noa Task 1", "blabla", DateTime.Parse("30-05-21"));
            myService.AddTask("noa@gmail.com", "noa@gmail.com", "noa Board 1", "noa Task 1", "blabla", DateTime.Parse("14-05-20"));
            myService.AddTask("noa@gmail.com", "noa@gmail.com", "noa Board 1", "noa Task 1", "", DateTime.Parse("30-05-21"));
            myService.AddTask("noa@gmail.com", "noa@gmail.com", "noa Board 1", "", "blabla", DateTime.Parse("30-05-21"));
            myService.AddTask("noa@gmail.com", "noa@gmail.com", "noa", "noa Task 1", "blabla", DateTime.Parse("30-05-21"));
            myService.AddTask("noa@gmail.com", "noa1@gmail.com", "noa Board 1", "noa Task 1", "blabla", DateTime.Parse("30-05-21"));
            myService.AddTask("noa1@gmail.com", "noa@gmail.com", "noa Board 1", "noa Task 1", "blabla", DateTime.Parse("30-05-21"));

            myService.Register("yoav@gmail.com", "Aa1234");
            myService.Register("yair@gmail.com", "Bb1234");
            myService.Login("yoav@gmail.com", "Aa1234");
            myService.AddBoard("yoav@gmail.com", "yoav Board 1");
            myService.Login("yair@gmail.com", "Bb1234");
            myService.AddBoard("yair@gmail.com", "yair Board 1");
            myService.JoinBoard("yair@gmail.com", "yoav@gmail.com", "yoav Board 1");
            
            Console.WriteLine("end test");
            */
        }
    }
}
