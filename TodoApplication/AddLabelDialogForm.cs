using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TodoApplicationLibrary;

namespace TodoApplication
{
    public partial class AddLabelDialogForm : Form
    {
        public TaskLabel AddedLabel { 
            get {
                return TaskLabelInfo.ParseLabel((string)labelsTypeComboBox.SelectedItem);
            }
        }
        public AddLabelDialogForm()
        {
            InitializeComponent();
        }

        public void RemoveActualLabels(List<TaskLabel>? actualLabels)
        {
            foreach (TaskLabel label in TaskLabelInfo.Items)
            {
                if(actualLabels == null || !actualLabels.Contains(label))
                {
                    labelsTypeComboBox.Items.Add(TaskLabelInfo.GetName(label));
                }
            }
        }
    }
}
