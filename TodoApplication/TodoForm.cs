using TodoApplicationLibrary;
using Task = TodoApplicationLibrary.Task;

namespace TodoApplication
{
    public partial class TodoForm : Form
    {

        private TaskManager taskManager;

        private Dictionary<Button, TaskList> taskListButtons;

        private Dictionary<Label, Task> taskLabels;

        private Task? editedTask;

        private TaskList? selectedTaskList;

        private Image doneImage;

        private Image incompletedImage;

        public TodoForm()
        {
            InitializeComponent();

            doneImage = Image.FromFile("../../../../Data/icons8-check-circle-50.png");
            incompletedImage = Image.FromFile("../../../../Data/icons8-circle-50.png");

            taskListButtons = new Dictionary<Button, TaskList>();
            taskLabels = new Dictionary<Label, Task>();

            taskManager = new TaskManager();

            editTaskListNameButton.Visible = false;

            taskListNameLabel.Text = "All tasks";

            HideRightTaskEditPanel();

            UpdateTaskListPanel();

            EnableScrolling();
        }

        private void EnableScrolling()
        {
            taskListListPanel.AutoScroll = false;

            taskListListPanel.HorizontalScroll.Maximum = 0;
            taskListListPanel.HorizontalScroll.Visible = false;
            taskListListPanel.HorizontalScroll.Enabled = false;

            taskListListPanel.VerticalScroll.Maximum = 0;
            taskListListPanel.VerticalScroll.Visible = false;

            taskListListPanel.AutoScroll = true;

            taskListPanel.AutoScroll = false;

            taskListPanel.HorizontalScroll.Maximum = 0;
            taskListPanel.HorizontalScroll.Visible = false;

            taskListPanel.VerticalScroll.Maximum = 0;
            taskListPanel.VerticalScroll.Visible = false;

            taskListPanel.AutoScroll = true;
        }

        private void ShowRightTaskEditPanel()
        {
            mainTableLayoutPanel.ColumnStyles[2].Width = 25;
            taskEditTableLayoutPanel.Visible = true;
            taskEditTableLayoutPanel.Width = 350;
        }

        private void HideRightTaskEditPanel()
        {
            mainTableLayoutPanel.ColumnStyles[2].Width = 0;
        }

        private void ShowHomePage()
        {
            taskListNameLabel.Text = "All tasks";
            taskListPanel.Controls.Clear();
            taskLabels.Clear();

            taskEditTableLayoutPanel.Visible = false;

        }

        private void UpdateTaskListPanel()
        {
            taskListListPanel.Controls.Clear();
            taskListButtons.Clear();
            for (int i = 0; i < taskManager.Count; i++)
            {
                if (taskManager[i] != taskManager.FindDefaultTaskList())
                {
                    taskListListPanel.Controls.Add(CreateTaskListBtn(taskManager[i]));
                }
            }
        }

        private Button CreateTaskListBtn(TaskList? taskList)
        {
            if (taskList == null) return null;

            Button btn = new Button();
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Dock = DockStyle.Top;
            btn.Text = taskList.Name;
            btn.FlatAppearance.BorderSize = 0;
            btn.Height = 50;
            btn.Padding = new Padding(20,0,0,0);
            btn.Click += new EventHandler(taskBtn_Click);

            taskListButtons[btn] = taskList;
            return btn;
        }

        private void taskBtn_Click(object sender,EventArgs e)
        {
            Button? btn = sender as Button;

            if (btn == null) return;


            ShowTaskList(taskListButtons[btn]);

            editTaskListNameButton.Visible = true;

            taskEditTableLayoutPanel.Visible = false;
        }

        private void ShowTaskList(TaskList? taskList)
        {
            if (taskList == null) return;

            selectedTaskList = taskList;
            taskListNameLabel.Text = taskList.Name;

            taskLabels.Clear();
            taskListPanel.Controls.Clear();

            for (int i = 0; i < selectedTaskList.Count; i++)
            {
                taskListPanel.Controls.Add(CreateTaskLabel(selectedTaskList[i]));
            }
        }

        private Panel CreateTaskLabel(Task? task)
        { 
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.ColumnCount = 2;
            panel.RowCount = 1;
            panel.Width = 20;
            panel.Dock = DockStyle.Top;
            panel.Padding = new Padding(0,0,0,30);


            Button button = new Button();
            panel.Controls.Add(button, 0, 0);

            button.Margin = new Padding(0);
            button.BackColor = Color.FromArgb(38, 38, 38);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Dock = DockStyle.Fill;
            button.Image = task?.State == TaskState.DONE ? doneImage : incompletedImage;
            button.Click += new EventHandler(doneLabelButton_Click);


            Label lbl = new Label();
            panel.Controls.Add(lbl,1,0);

            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Padding = new Padding(15,0,0,0);
            lbl.BackColor = Color.FromArgb(38,38,38);
            lbl.Text = task?.Title;
            lbl.Click += new EventHandler(taskLabel_Click);

            taskLabels[lbl] = task;
            
            return panel;
        }

        private void doneLabelButton_Click(object? sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                button.Image = button.Image == doneImage ? incompletedImage : doneImage;
                TableLayoutPanel? panel = button.Parent as TableLayoutPanel;
                if (panel != null) {
                    Label l = panel.GetControlFromPosition(1, 0) as Label;
                    if(l!= null)
                    {
                        taskLabels[l].State = button.Image == doneImage ? TaskState.DONE : TaskState.INCOMPLETE;
                        taskManager.Save();
                    }
                }
            }
        }

        private void taskLabel_Click(object? sender, EventArgs e)
        {
            Label label = sender as Label;

            if(label == null) return;

            Task selectedTask = taskLabels[label];

            editedTask = selectedTask;

            ParseDataToTaskPanel();
        }

        private void ParseDataToTaskPanel()
        {
            if (editedTask == null) return;

            ShowRightTaskEditPanel();

            taskTitleTextBox.Text = editedTask.Title;

            taskStateButton.Image = editedTask?.State == TaskState.DONE ? doneImage : incompletedImage;
            taskCreatedDateLabel.Text = "Created: " + editedTask?.CreateDate.ToString();

            if(editedTask?.DueDate != null)
            {
                deadlineCheckBox.Checked = true;
                deadlineDateTimePicker.Value = (DateTime)editedTask.DueDate;
            } else
            {
                deadlineCheckBox.Checked= false;
            }

            if(editedTask?.Note != null)
            {
                noteTextBox.Text = editedTask.Note.Text;
                noteEditDateLabel.Text = "Edited: " + editedTask.Note.EditDate.ToString();
            }
        }

        private void closeRightPanelButton_Click(object sender, EventArgs e)
        {
            HideRightTaskEditPanel();
            editedTask = null;
        }

        private void deadlineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(deadlineCheckBox.Checked)
                deadlineDateTimePicker.Enabled = true;
            else
                deadlineDateTimePicker.Enabled = false;
        }

        private void doneButton_Click(object sender, EventArgs e) 
        { 
            taskStateButton.Image = taskStateButton.Image == doneImage ? incompletedImage : doneImage;
        }

        private void saveTaskButton_Click(object sender, EventArgs e)
        {
            if (selectedTaskList == null || editedTask == null) return;

            string errorMsg = "";
            if(taskTitleTextBox.Text == null || taskTitleTextBox.Text.Length < 1)
            {
                errorMsg += "Missing task name\n";
            }

            if(errorMsg.Length > 0)
            {
                InfoDialogForm infoDialog = new InfoDialogForm();
                infoDialog.EditInfoMessage(errorMsg);
                infoDialog.ShowDialog();
            } else {
                    editedTask.Title = taskTitleTextBox.Text;
                    editedTask.State = taskStateButton.Image == doneImage ? TaskState.DONE : TaskState.INCOMPLETE;

                    if (editedTask.DueDate == null && deadlineCheckBox.Checked)
                    {
                        editedTask.DueDate = TaskManager.TimeZone.ToLocalTime(deadlineDateTimePicker.Value);
                    }

                    if (editedTask.DueDate != null && !deadlineDateTimePicker.Checked)
                    {
                        editedTask.DueDate = null;
                    }

                    if (editedTask.Note == null && (noteTextBox.Text != null && noteTextBox.Text.Length > 0))
                    {
                        editedTask.Note = new Note(noteTextBox.Text);
                    }

                    if (editedTask.Note != null && (noteTextBox.Text != null && noteTextBox.Text.Length > 0))
                    {
                        editedTask.Note.Text = noteTextBox.Text;
                    }

                    if (editedTask.Note != null && (noteTextBox.Text == null || noteTextBox.Text.Length < 1))
                    {
                        editedTask.Note = null;
                    }

                Label label = taskLabels.FirstOrDefault(x => x.Value == editedTask).Key;
                TableLayoutPanel panel = label.Parent as TableLayoutPanel;
                if(panel != null)
                {
                    Button b = panel.GetControlFromPosition(0, 0) as Button;
                    if(b != null)
                    {
                        b.Image = editedTask.State == TaskState.DONE ? doneImage : incompletedImage;
                    }
                }
                label.Text = editedTask.Title;

                taskManager.Save();
            }


        }

        private void deleteTaskButton_Click(object sender, EventArgs e)
        {
            if (editedTask == null || selectedTaskList == null) return;

            DeleteDialogForm d = new DeleteDialogForm();
            d.ShowDialog();

            if(d.DialogResult.Equals(DialogResult.OK)) {
                taskManager.DeleteTask(selectedTaskList, editedTask.Id);

                Label label = taskLabels.FirstOrDefault(x => x.Value == editedTask).Key;
                Panel panel = label.Parent as Panel;
                if (panel != null) { taskListPanel.Controls.Remove(panel); }

                editedTask = null;
                HideRightTaskEditPanel();
            }
        }

        private void addTaskButton_Click(object sender, EventArgs e)
        {
            if(newTaskTextBox.Text == null || newTaskTextBox.Text.Length < 1 || selectedTaskList == null) return;

            editedTask = taskManager.CreateTask(selectedTaskList, newTaskTextBox.Text, null, null);
            newTaskTextBox.Text = "";

            taskListPanel.Controls.Add(CreateTaskLabel(editedTask));

            ParseDataToTaskPanel();
        }

        private void addTaskListButton_Click(object sender, EventArgs e)
        {
            NewTaskListForm taskListForm = new NewTaskListForm();
            taskListForm.ShowDialog();
            if (taskListForm.DialogResult.Equals(DialogResult.OK))
            {
                string taskListName = taskListForm.ReadTaskListName();
                if(taskListName != null && taskListName.Length > 0)
                {
                    selectedTaskList = taskManager.CreateTaskList(taskListName);
                    taskListListPanel.Controls.Add(CreateTaskListBtn(selectedTaskList));
                    HideRightTaskEditPanel();
                    ShowTaskList(selectedTaskList);
                }
            }
        }

        private void deleteTaskListButton_Click(object sender, EventArgs e)
        {
            if (selectedTaskList == null || selectedTaskList == taskManager.FindDefaultTaskList()) return;

            DeleteDialogForm deleteDialogForm = new DeleteDialogForm();
            deleteDialogForm.ShowDialog();
            if(deleteDialogForm.DialogResult.Equals(DialogResult.OK))
            {
                taskManager.DeleteTaskList(selectedTaskList.Id);
                Button button = taskListButtons.FirstOrDefault(x => x.Value == selectedTaskList).Key;
                if (button != null) { taskListListPanel.Controls.Remove(button); }

                selectedTaskList = null;
                ShowHomePage();
            }
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            ShowHomePage();
            HideRightTaskEditPanel();
            editTaskListNameButton.Visible = false; 
        }

        private void tasksButton_Click(object sender, EventArgs e)
        {

            ShowTaskList(taskManager.FindDefaultTaskList());
            HideRightTaskEditPanel();
            editTaskListNameButton.Visible = false;
        }

        private void editTaskListNameButton_Click(object sender, EventArgs e)
        {
            if (selectedTaskList == null) return;
            NewTaskListForm taskListForm = new NewTaskListForm();
            taskListForm.ParseTaskListName(selectedTaskList.Name);
            taskListForm.ShowDialog();
            if (taskListForm.DialogResult.Equals(DialogResult.OK))
            {
                string taskListName = taskListForm.ReadTaskListName();
                if (taskListName != null && taskListName.Length > 0)
                {
                    selectedTaskList.Name = taskListName;
                    Button button = taskListButtons.FirstOrDefault(x => x.Value == selectedTaskList).Key;
                    if (button != null) { button.Text = selectedTaskList.Name; }
                    taskListNameLabel.Text = selectedTaskList.Name;
                    taskManager.Save();
                }
            }
        }
    }
}