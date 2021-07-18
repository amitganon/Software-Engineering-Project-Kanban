using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Test
    {
        public static void Main(string[] args)
        {
            /*/
             
            Service s = new Service();
            s.DeleteData();
            s.Register("amit@gmail.com", "Aa1234");
            s.Register("amit2@gmail.com", "Aa1234");
            s.Login("amit@gmail.com", "Aa1234");
            s.AddBoard("amit@gmail.com", "testBoard");
            s.Logout("amit@gmail.com");
            s.Login("amit2@gmail.com", "Aa1234");
            s.JoinBoard("amit2@gmail.com","amit@gmail.com","testBoard");
            /*
                Console.WriteLine("helloosd,jgdsa;lkdgmsudaohmasdlhsdlhgasdgdsagdsagsda");
                /*
                //Service c = new Service();
                //Response r=c.Register("amit6021521@gmail.com", "a1525B");
                //Console.WriteLine(r);
                //Response r2= c.Login("amit6021521@gmail.com", "a1525B");
                //Console.WriteLine(r2);
                //Response r3 = c.AddBoard("amit6021521@gmail.com","testBoard");
                //Console.WriteLine(r3);
                //DateTime d=new DateTime(2021,4,10);
                //Response r4 = c.AddTask("amit6021521@gmail.com", "testBoard","testTask","hellow world",d);
                //Console.WriteLine(r4);

                //chaking if advance task working
                Response r5 = c.AdvanceTask("amit6021521@gmail.com", "testBoard", 0,0);
                Console.WriteLine(r5);
                Response r6 = c.AdvanceTask("amit6021521@gmail.com", "testBoard", 1, 0);
                Console.WriteLine(r6);
                //Response r7 = c.AdvanceTask("amit6021521@gmail.com", "testBoard", 2, 0);
                //Console.WriteLine(r7);

                //we have 1 task with id - 0 id column -done
                //chaking if updateTask working
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(c.AddTask("amit6021521@gmail.com", "testBoard", "testTask2", "hellow",d));
                Console.WriteLine(c.UpdateTaskDescription("amit6021521@gmail.com", "testBoard", 0, 1,""));
                Console.WriteLine(c.UpdateTaskTitle("amit6021521@gmail.com", "testBoard", 0, 1, "`title 2"));
                DateTime d2 = new DateTime(2000, 4, 10);
                Console.WriteLine(c.UpdateTaskDueDate("amit6021521@gmail.com", "testBoard", 0, 1, d2));

                Console.WriteLine(c.AdvanceTask("amit6021521@gmail.com", "testBoard", 0, 1));
                Console.WriteLine(c.UpdateTaskDescription("amit6021521@gmail.com", "testBoard", 1, 1, "asdg"));
                Console.WriteLine(c.UpdateTaskTitle("amit6021521@gmail.com", "testBoard", 1, 1, "`title 3"));
                Console.WriteLine(c.UpdateTaskDueDate("amit6021521@gmail.com", "testBoard", 1, 1, d));

                Console.WriteLine(c.AdvanceTask("amit6021521@gmail.com", "testBoard", 1, 1));
                Console.WriteLine(c.UpdateTaskDescription("amit6021521@gmail.com", "testBoard", 2, 1, "asdg"));
                Console.WriteLine(c.UpdateTaskTitle("amit6021521@gmail.com", "testBoard", 2, 1, "`title 3"));
                Console.WriteLine(c.UpdateTaskDueDate("amit6021521@gmail.com", "testBoard", 2, 1, d));

                //we have 2 tasks, id=0,1 id column done;
                //cheking if addTask working
                Console.WriteLine(c.AddTask("amit6021521@gmail.com", "testBoard", "testTask2", "hellow", d));
                //Console.WriteLine(c.AddTask("amit6021521@gmail.com", "td", "testTask2", "hellow", d));
                Console.WriteLine(c.AddTask("amit6021521@gmail.com", "testBoard", "", "sdagasdg", d));

                //

                DBoardController DBoardController = new DBoardController();
                DBoard Dboard = new DBoard(1,"test","amitganon@gmail.com",10,35);
                DBoardController.Insert(Dboard);

            */
        }
    }
}
