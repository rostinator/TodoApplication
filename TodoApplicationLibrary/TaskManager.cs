using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplicationLibrary
{
    public class TaskManager
    {

        private int maxTaskId;

        private readonly ReadSaveToFile readSaveToFile;

        private int maxTaskListId;

        private TaskList? defaultTaskList;

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

            try
            {
                taskLists = readSaveToFile.ReadTasks();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
            TaskList newTaskList = new(name, ++maxTaskListId);
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
                    Save();
                    return;
                }
            }
            throw new TaskManagerException("Task with id: " + taskListsId + " does not exist!");
        }

        public TaskList FindTaskList(int taskListsId)
        {
            TaskList? taskList = taskLists.Find(t => t.Id == taskListsId);
            if (taskList is null) throw new TaskManagerException("Failed to find task with id: " + taskListsId);
            return taskList;
        }

        public TaskList? FindTaskList(string taskListsName)
        {
            return taskLists.Find(t => t.Name == taskListsName);
        }

        public TaskList ReadAllTasks()
        {
            TaskList allTasks = new("All Tasks", -1);
            taskLists.ForEach(t =>
            {
                for (int i = 0; i < t.Count; i++)
                {
                    allTasks.AddTask(t[i]);
                }
            });

            return allTasks;
        }

        public void Save()
        {
            try
            {
                readSaveToFile.SaveTasks(taskLists);
            } catch (Exception ex)
            {
                throw new TaskManagerException("Failed to save tasks! + " + ex.Message);
            }
        }

        public Task? CreateTask(TaskList? list, string title, DateTime? dueDate, List<TaskLabel>? labels, string? noteText)
        {
            if (list == null) throw new TaskManagerException("Failed to create new task");

            Task t = list.CreateTask(++maxTaskId, title, dueDate, labels, noteText);
            Save();
            return t;
        }

        public void DeleteTask(TaskList list, int taskId)
        {
            if (list == null) throw new TaskManagerException("Failed delete task: " + taskId);
            if(list.IsContains(taskId))
            {
                list.DeleteTask(taskId);
                Save();
            } else
            {
                throw new TaskManagerException("Failed delete task: invalid id");
            }
        }

        public TaskList? FindDefaultTaskList() {
            return defaultTaskList;
        }

        public static void SortTaskList(TaskList taskList, TaskOrder taskOrder)
        {
            switch(taskOrder) {
                case TaskOrder.CreateDate:
                    taskList.Sort(SortTaskByCreateDate);
                    break;
                case TaskOrder.DueDate:
                    taskList.Sort(SortTaskByDueDate);
                    break;
                case TaskOrder.Status:
                    taskList.Sort(SortTaskByStatus);
                    break;

                case TaskOrder.Alphabed:
                    taskList.Sort(SortTaskByName);
                    break;
                default:
                    taskList.Sort(SortTaskByCreateDate);
                    break;
            }
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
                    if(t[i]?.Id > maxTaskId)
                    {
                        maxTaskId = t[i].Id;
                    }
                }
            });
        }

        private static bool SortTaskByCreateDate(Task? task1, Task? task2)
        {
            return task1?.CreateDate > task2?.CreateDate;
        }

        private static bool SortTaskByDueDate(Task? task1, Task? task2)
        {
            if(task1?.DueDate == null) return false;
            if (task2?.DueDate == null) return true;
            return task1?.DueDate > task2?.DueDate;
        }

        private static bool SortTaskByStatus(Task? task1, Task? task2)
        {
            if(task1?.State == TaskState.DONE && task2?.State == TaskState.INCOMPLETE)
            {
                return true;
            }
            return false;
        }
        private static bool SortTaskByName(Task? task1, Task? task2)
        {
            return task1?.Title.CompareTo(task2?.Title) < 0;
        }
    }
}
