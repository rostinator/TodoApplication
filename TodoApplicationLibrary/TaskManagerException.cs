using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplicationLibrary
{
    public class TaskManagerException : Exception
    {
        public TaskManagerException()
        {

        }

        public TaskManagerException(string? message) : base(message)
        {

        }
    }
}
