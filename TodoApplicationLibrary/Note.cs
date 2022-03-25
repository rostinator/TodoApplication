using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplicationLibrary
{
    [Serializable]
    public class Note
    {

        private string text;
        public string Text {
            get
            {
                return text;
            }
            set {
                text = value;
                EditDate = TaskManager.TimeZone.ToLocalTime(DateTime.Now);
            }
        }

        public DateTime? EditDate { get; set; }

        public Note(string text)
        {
            Text = text;
        }
    }
}
