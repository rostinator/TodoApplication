using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplicationLibrary
{

    public delegate bool CompareTasksCallback(Task? task1, Task? task2);

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

            set
            {
                tasks[index] = value;
            }
        }
        internal TaskList(string name, int id)
        {
            Id = id;
            Name = name;
            tasks = new List<Task>();
        }

        public List<Task> SearchTask(string text)
        {
            List<Task> searchTasks = new();

            tasks.ForEach(t =>
            {
                if (t.Title.ToLower().StartsWith(text.ToLower()))
                {
                    searchTasks.Add(t);
                }
            });

            return searchTasks;
        }

        internal Task CreateTask(int id, string title, DateTime? dueDate,List<TaskLabel>? labels, string? noteText)
        {
            Task newTask = new(id, title, TaskState.INCOMPLETE, dueDate, labels, (noteText == null || noteText.Length < 1) ? null : new Note(noteText));
            tasks.Add(newTask);
            return newTask;
        }

        internal void Sort(CompareTasksCallback callback)
        {
            Task? tmp;
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < Count - 1; j++)
                {
                    if (callback(this[j], this[j + 1]))
                    {
                        tmp = this[j + 1];
                        this[j + 1] = this[j];
                        this[j] = tmp;
                    }
                }
            }
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

        internal void AddTask(Task task)
        {
            tasks.Add(task);
        }

        internal bool IsContains(int id)
        {
            foreach(var task in tasks)
                if (task.Id == id)
                    return true;
            return false;
        }

    }
}
