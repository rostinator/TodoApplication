using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplicationLibrary
{
    public enum TaskOrder
    {
        None,
        CreateDate,
        DueDate,
        Status,
        Alphabed
    }


    public static class TaskOrderInfo
    {
        public static IEnumerable Items
        {
            get
            {
                return Enum.GetValues(typeof(TaskOrder)).Cast<TaskOrder>();
            }
        }

        public static string GetName(TaskOrder taskOrder)
        {
            return taskOrder switch
            {
                TaskOrder.None => "-",
                TaskOrder.CreateDate => "Create date",
                TaskOrder.DueDate => "Due date",
                TaskOrder.Status => "Status",
                TaskOrder.Alphabed => "A - Z",
                _ => string.Empty,
            };
        }
    }
}
