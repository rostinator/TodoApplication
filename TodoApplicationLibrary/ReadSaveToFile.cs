using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace TodoApplicationLibrary
{
    internal class ReadSaveToFile 
    {
        private readonly string FILE_NAME = "../../../../Data/TaskLists.bin";

        internal void SaveTasks(List<TaskList> taskLists)
        {
            using(FileStream dataStream = new(FILE_NAME, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new();

                formatter.Serialize(dataStream, taskLists);
                
            }
        }

        internal List<TaskList>? ReadTasks()
        {
            if (!File.Exists(FILE_NAME)) return null;
                
            using (FileStream dataStream = new(FILE_NAME, FileMode.Open))
            {
                BinaryFormatter formatter = new();

                return (List<TaskList>)formatter.Deserialize(dataStream);

            }
        }


    }
}
