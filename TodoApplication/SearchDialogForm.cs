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
    public partial class SearchDialogForm : Form
    {
        public SearchDialogForm()
        {
            InitializeComponent();
        }

        public string ReadSearchText()
        {
            return searchTermTextBox.Text;
        }
    }
}
