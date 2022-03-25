using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplicationLibrary
{
    [Serializable]
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public TaskState State { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? DueDate { get; set; }

        public Note? Note { get; set; }

        public Task(int id, string title, TaskState state, DateTime? dueDate, Note? note)
        {
            Id = id;
            Title = title;
            State = state;
            DueDate = dueDate;
            Note = note;
            CreateDate = TaskManager.TimeZone.ToLocalTime(DateTime.Now); ;
        }
    }
}
