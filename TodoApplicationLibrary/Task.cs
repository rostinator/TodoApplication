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

        public List<TaskLabel> Labels { get; set; }

        public Note? Note { get; set; }

        public Task(int id, string title, TaskState state, DateTime? dueDate, List<TaskLabel>? labels, Note? note)
        {
            Id = id;
            Title = title;
            State = state;
            DueDate = dueDate;
            Note = note;
            Labels = labels ?? new List<TaskLabel>();
            CreateDate = TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Now); ;
        }

        public void AddTaskLabel(TaskLabel taskLabel)
        {
            if (Labels == null) Labels = new();
            if (!Labels.Contains(taskLabel)) Labels.Add(taskLabel);
        }

        public void RemoveLabel(TaskLabel taskLabel)
        {
            Labels.Remove(taskLabel);
        }

    }
}
