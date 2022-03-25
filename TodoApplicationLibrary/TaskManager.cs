using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplicationLibrary
{
    public class TaskManager
    {
        public static TimeZone TimeZone = TimeZone.CurrentTimeZone;

        private int maxTaskId;

        private ReadSaveToFile readSaveToFile;

        private int maxTaskListId;

        private TaskList defaultTaskList;

        private List<TaskList> taskLists;

        public int Count { get { return taskLists.Count; } }

        public TaskList? this[int index]
        {
            get
            {
                if (index < 0 || index >= taskLists.Count)
                    return null;

                return taskLists[index];
            }
        }

        public TaskManager()
        {
            readSaveToFile = new ReadSaveToFile();

            taskLists = readSaveToFile.ReadTasks();
            if (taskLists == null)
            {
                taskLists = new List<TaskList>();
                maxTaskId = 1;
                maxTaskListId = 1;  
            }
            defaultTaskList = FindTaskList("Tasks");

            if (defaultTaskList == null) defaultTaskList = CreateTaskList("Tasks");

            FindMaxIds();
        }

        public TaskList CreateTaskList(string name)
        {
            TaskList newTaskList = new TaskList(name, ++maxTaskListId);
            taskLists.Add(newTaskList);
            Save();
            return newTaskList;
        }

        public void DeleteTaskList(int taskListsId)
        {
            foreach (var task in taskLists)
            {
                if (task.Id == taskListsId)
                {
                    taskLists.Remove(task);
                    return;
                }
            }
            throw new Exception("Task with id: " + taskListsId + " does not exist!");
            Save();
        }

        public TaskList? FindTaskList(int taskListsId)
        {
            return taskLists.Find(t => t.Id == taskListsId);
        }

        public TaskList? FindTaskList(string taskListsName)
        {
            return taskLists.Find(t => t.Name == taskListsName);
        }

        public void Save()
        {
            readSaveToFile.SaveTasks(taskLists);
        }

        public Task? CreateTask(TaskList list, string title, DateTime? dueDate, string? noteText)
        {
            if (list == null) return null;

            return list.CreateTask(++maxTaskId, title, dueDate, noteText);
            Save();
        }

        public void DeleteTask(TaskList list, int taskId)
        {
            if (list == null) return;
            list.DeleteTask(taskId);
            Save();
        }

        public TaskList FindDefaultTaskList() {
            return defaultTaskList;
        }

        private void FindMaxIds()
        {
            maxTaskId = 0;
            maxTaskListId = 0;
            taskLists.ForEach(t => {
                if(t.Id > maxTaskListId)
                {
                    maxTaskListId = t.Id;
                }
                for (int i = 0; i < t.Count; i++)
                {
                    if(t[i].Id > maxTaskId)
                    {
                        maxTaskId = t[i].Id;
                    }
                }
            });
        }
    }
}
