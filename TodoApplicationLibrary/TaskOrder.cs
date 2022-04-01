using System;
using System.Collections;
using System.Collections.Generic;
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
                return Enum.GetValues(typeof(TaskOrder)).Cast<TaskOrder>(); ;
            }
        }

        public static string GetName(TaskOrder taskOrder)
        {
            switch (taskOrder)
            {
                case TaskOrder.None: return "-";
                case TaskOrder.CreateDate: return "Create date";
                case TaskOrder.DueDate: return "Due date";
                case TaskOrder.Status: return "Status";
                case TaskOrder.Alphabed: return "A - Z";
                default: return string.Empty;
            }
        }
    }
}
