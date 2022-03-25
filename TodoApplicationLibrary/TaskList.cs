using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplicationLibrary
{
    [Serializable]
    public class TaskList
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Count { get { return tasks.Count; } }

        private List<Task> tasks;

        public Task? this[int index]
        {
            get
            {
                if (index < 0 || index >= tasks.Count)
                    return null;

                return tasks[index];
            }
        }
        internal TaskList(string name, int id)
        {
            Id = id;
            Name = name;
            tasks = new List<Task>();
        }

        internal Task CreateTask(int id, string title, DateTime? dueDate, string? noteText)
        {
            Task newTask = new Task(id, title, TaskState.INCOMPLETE, dueDate, (noteText == null || noteText.Length < 1) ? null : new Note(noteText));
            tasks.Add(newTask);
            return newTask;
        }

        internal void DeleteTask(int taskId)
        {
            foreach (var task in tasks)
            {
                if (task.Id == taskId)
                {
                    tasks.Remove(task);
                    return;
                }
            }
            throw new Exception("Task with id: " + taskId + " does not exist!");
        }

        internal bool isContains(int id)
        {
            foreach(var task in tasks)
                if (task.Id == id)
                    return true;
            return false;
        }

    }
}
