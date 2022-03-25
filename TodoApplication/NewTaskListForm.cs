using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TodoApplication
{
    public partial class NewTaskListForm : Form
    {
        public NewTaskListForm()
        {
            InitializeComponent();
        }

        public string ReadTaskListName()
        {
            return taskListTextBox.Text;
        }

        public void ParseTaskListName(string name)
        {
            taskListTextBox.Text = name;
        }
    }
}
