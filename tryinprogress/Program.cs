using System;
namespace tryinprogress
{
    class Program
    {
        static void Main(string[] args)
        {
            Service service = new Service();
            service.Login("test@gmail.com", "Aa1234");
            DateTime d = DateTime.Now;
            service.AddTask("test@gmail.com", "test@gmail.com", "test", "test title", "test description", d);
            service.AdvanceTask("test@gmail.com", "test@gmail.com", "test", 0, 0);
        }
    }
}
