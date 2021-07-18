using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Task
    {
        private readonly int id;
        private readonly DateTime creationTime;
        private readonly string title;
        private readonly string description;
        private readonly DateTime dueDate;
        private readonly string emailAssignee;

        internal Task(BusinessLayer.Task task)
        {
            this.id = task.Id;
            this.creationTime = task.CreationTime;
            this.title = task.Title;
            this.description = task.Description;
            this.dueDate = task.DueDate;
            this.emailAssignee = task.Assignee;
        }
        public int Id { get => id; }
        public DateTime CreationTime { get => creationTime; }
        public  string Title { get => title; }
        public  string Description { get => description; }
        public  DateTime DueDate { get => dueDate; }
        public  string EmailAssignee { get => emailAssignee; }
    }
}