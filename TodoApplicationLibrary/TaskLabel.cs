using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplicationLibrary
{
    public enum TaskLabel
    {
        Inportant,
        OnHold,
        HighPriority,
        LowPriority,
        InProgress,
        School,
        Job
    }

    public static class TaskLabelInfo
    {
        public static IEnumerable Items
        {
            get
            {
                return Enum.GetValues(typeof(TaskLabel)).Cast<TaskLabel>();
            }
        }

        public static string GetName(TaskLabel label)
        {
            return label switch
            {
                TaskLabel.Inportant => "Inportant",
                TaskLabel.OnHold => "On hold",
                TaskLabel.HighPriority => "High priority",
                TaskLabel.LowPriority => "Low priority",
                TaskLabel.InProgress => "In progress",
                TaskLabel.School => "School",
                TaskLabel.Job => "Job",
                _ => "",
            };
        }

        public static TaskLabel ParseLabel(string label)
        {
            return label switch
            {
                "Inportant" => TaskLabel.Inportant,
                "On hold" => TaskLabel.OnHold,
                "High priority" => TaskLabel.HighPriority,
                "Low priority" => TaskLabel.LowPriority,
                "In progress" => TaskLabel.InProgress,
                "School" => TaskLabel.School,
                "Job" => TaskLabel.Job,
                _ => throw new NotImplementedException(),
            };
        }

        public static Color GetColor(TaskLabel label)
        {
            return label switch
            {
                TaskLabel.Inportant => Color.FromArgb(192, 57, 43),
                TaskLabel.OnHold => Color.FromArgb(212, 172, 13),
                TaskLabel.HighPriority => Color.FromArgb(52, 152, 219),
                TaskLabel.LowPriority => Color.FromArgb(40, 180, 99),
                TaskLabel.InProgress => Color.FromArgb(142, 68, 173),
                TaskLabel.School => Color.FromArgb(46, 64, 83),
                TaskLabel.Job => Color.FromArgb(112, 123, 124),
                _ => Color.FromArgb(38, 38, 38),
            };
        }
    }




}
