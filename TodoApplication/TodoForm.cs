using TodoApplicationLibrary;
using Task = TodoApplicationLibrary.Task;

namespace TodoApplication
{
    public partial class TodoForm : Form
    {

        private TaskManager taskManager;

        private Dictionary<Button, TaskList> taskListButtons;

        private Dictionary<Label, Task> tasks;

        private Dictionary<Panel, TaskLabel> taskLabels;

        private Task? editedTask;

        private TaskList? selectedTaskList;

        private Image doneImage;

        private Image incompletedImage;

        private Image deleteLabelImage;

        public TodoForm()
        {
            InitializeComponent();

            doneImage = Image.FromFile("../../../../Data/icons8-check-circle-50.png");
            incompletedImage = Image.FromFile("../../../../Data/icons8-circle-50.png");
            deleteLabelImage = Image.FromFile("../../../../Data/icons8-close-16.png");

            taskListButtons = new Dictionary<Button, TaskList>();
            tasks = new Dictionary<Label, Task>();
            taskLabels = new Dictionary<Panel, TaskLabel>();

            taskManager = new TaskManager();

            editTaskListNameButton.Visible = false;

            foreach (TaskOrder order in TaskOrderInfo.Items)
            {
                sortTasksComboBox.Items.Add(TaskOrderInfo.GetName(order));
            }

            sortTasksComboBox.SelectedIndexChanged += new System.EventHandler(SortTasks);


            HideRightTaskEditPanel();

            UpdateTaskListPanel();

            EnableScrolling();

            ShowHomePage();
        }

        private void SortTasks(object? sender, EventArgs e)
        {
            if(selectedTaskList != null)
            {
                TaskOrder selectedOrder = ReadSelectedTaskOrder();
                if(selectedOrder != TaskOrder.None)
                {
                    TaskManager.SortTaskList(selectedTaskList, selectedOrder);
                }
                ShowTaskList(selectedTaskList);
            }

        }

        private TaskOrder ReadSelectedTaskOrder()
        {
            return sortTasksComboBox.SelectedIndex switch
            {
                0 => TaskOrder.None,
                1 => TaskOrder.CreateDate,
                2 => TaskOrder.DueDate,
                3 => TaskOrder.Status,
                4 => TaskOrder.Alphabed,
                _ => TaskOrder.None,
            };
        }

        private static void EnableScrolling(Panel p)
        {
            p.AutoScroll = false;

            p.HorizontalScroll.Maximum = 0;
            p.HorizontalScroll.Visible = false;
            p.HorizontalScroll.Enabled = false;

            p.VerticalScroll.Maximum = 0;
            p.VerticalScroll.Visible = false;

            p.AutoScroll = true;
        }

        private void EnableScrolling()
        {
            EnableScrolling(taskListListPanel);
            EnableScrolling(taskListPanel);
            EnableScrolling(labelsFlowLayoutPanel);
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
            tasks.Clear();

            taskEditTableLayoutPanel.Visible = false;

            ShowTaskList(taskManager.ReadAllTasks());
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

        private Button? CreateTaskListBtn(TaskList? taskList)
        {
            if (taskList == null) return null;

            Button btn = new();
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Dock = DockStyle.Top;
            btn.Text = taskList.Name;
            btn.FlatAppearance.BorderSize = 0;
            btn.Height = 50;
            btn.Padding = new Padding(20,0,0,0);
            btn.Click += new EventHandler(TaskBtn_Click);

            taskListButtons[btn] = taskList;
            return btn;
        }

        private void TaskBtn_Click(object sender,EventArgs e)
        {
            if (sender is not Button btn) return;


            ShowTaskList(taskListButtons[btn]);
            sortTasksComboBox.SelectedIndex = 0;

            editTaskListNameButton.Visible = true;

            taskEditTableLayoutPanel.Visible = false;
        }

        private void ShowTaskList(TaskList? taskList)
        {
            if (taskList == null) return;

            selectedTaskList = taskList;
            taskListNameLabel.Text = taskList.Name;

            tasks.Clear();
            taskListPanel.Controls.Clear();

            for (int i = 0; i < selectedTaskList.Count; i++)
            {
                taskListPanel.Controls.Add(CreateTaskLabel(selectedTaskList[i]));
            }
        }

        private void ShowSearchList(List<Task> tasks)
        {
            this.tasks.Clear();
            taskListPanel.Controls.Clear();
            for (int i = 0; i < tasks.Count; i++)
            {
                taskListPanel.Controls.Add(CreateTaskLabel(tasks[i]));
            }
        }

        private Panel CreateTaskLabel(Task? task)
        { 
            TableLayoutPanel panel = new();
            panel.ColumnCount = 2;
            panel.RowCount = 1;
            panel.Width = 20;
            panel.Dock = DockStyle.Top;
            panel.Padding = new Padding(0,0,0,30);


            Button button = new();
            panel.Controls.Add(button, 0, 0);

            button.Margin = new Padding(0);
            button.BackColor = Color.FromArgb(38, 38, 38);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Dock = DockStyle.Fill;
            button.Image = task?.State == TaskState.DONE ? doneImage : incompletedImage;
            button.Click += new System.EventHandler(DoneLabelButton_Click);


            Label lbl = new();
            panel.Controls.Add(lbl,1,0);

            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Padding = new Padding(15,0,0,0);
            lbl.BackColor = Color.FromArgb(38,38,38);
            string text = task.Title.Replace('\n', ' ');
            text = text.Replace('\r', ' ');
            lbl.Text = text;
            lbl.Click += new EventHandler(TaskLabel_Click);

            tasks[lbl] = task;
            
            return panel;
        }

        private void DoneLabelButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.Image = button.Image == doneImage ? incompletedImage : doneImage;
                if (button.Parent is TableLayoutPanel panel)
                {
                    if (panel.GetControlFromPosition(1, 0) is Label l)
                    {
                        tasks[l].State = button.Image == doneImage ? TaskState.DONE : TaskState.INCOMPLETE;
                        SaveData();
                    }
                }
            }
        }

        private void TaskLabel_Click(object? sender, EventArgs e)
        {
            if (sender is not Label label) return;

            Task selectedTask = tasks[label];

            editedTask = selectedTask;

            ParseDataToTaskPanel();
        }

        private void ParseDataToTaskPanel()
        {
            if (editedTask == null) return;

            ShowRightTaskEditPanel();

            taskCreatedDateLabel.Text = "Created: " + editedTask?.CreateDate.ToString();

            taskTitleTextBox.Text = editedTask?.Title;

            ParseLabels();

            if (editedTask?.DueDate != null)
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
            } else
            {
                noteTextBox.Text = "";
                noteEditDateLabel.Text = "";
            }
        }

        private void ParseLabels()
        {
            labelsFlowLayoutPanel.Controls.Clear();
            taskLabels.Clear();
            if (editedTask?.Labels != null)
            {
                foreach (var label in editedTask.Labels)
                {
                    Panel labelPanel = new();
                    Label labelName = new();
                    Button deleteBtn = new();
                    deleteBtn.Dock = DockStyle.Right;
                    deleteBtn.FlatStyle = FlatStyle.Flat;
                    deleteBtn.FlatAppearance.BorderSize = 0;
                    deleteBtn.Image = deleteLabelImage;
                    deleteBtn.MaximumSize = new Size(30, 30);
                    deleteBtn.Click += new EventHandler(DeleteLabelButton_Click);

                    labelName.Dock = DockStyle.Fill;
                    labelName.MinimumSize = new Size(100, 30);
                    labelName.Text = TaskLabelInfo.GetName(label);
                    labelName.TextAlign = ContentAlignment.MiddleLeft;

                    labelPanel.BackColor = TaskLabelInfo.GetColor(label);
                    labelPanel.MaximumSize = new Size(135, 30);
                    labelPanel.Controls.Add(labelName);
                    labelPanel.Controls.Add(deleteBtn);
                    labelsFlowLayoutPanel.Controls.Add(labelPanel);

                    taskLabels.Add(labelPanel, label);
                }
            }
        }

        private void DeleteLabelButton_Click(object sender, EventArgs e)
        {
            if (sender is Button deleteBnt)
            {
                if (deleteBnt.Parent is Panel panel)
                {
                    editedTask?.RemoveLabel(taskLabels[panel]);
                    labelsFlowLayoutPanel.Controls.Remove(panel);
                    taskLabels.Remove(panel);
                }
            }
        }

        private void SaveData()
        {
            try
            {
                taskManager.Save();
            }catch(TaskManagerException ex)
            {
                ShowInfoDialog(ex.Message);
            }
        }

        private static void ShowInfoDialog(String msg)
        {
            InfoDialogForm infoDialog = new();
            infoDialog.EditInfoMessage(msg);
            infoDialog.ShowDialog();
        }

        private void CloseRightPanelButton_Click(object sender, EventArgs e)
        {
            HideRightTaskEditPanel();
            editedTask = null;
        }

        private void DeadlineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(deadlineCheckBox.Checked)
                deadlineDateTimePicker.Enabled = true;
            else
                deadlineDateTimePicker.Enabled = false;
        }

        private void SaveTaskButton_Click(object sender, EventArgs e)
        {
            if (selectedTaskList == null || editedTask == null) return;

            string errorMsg = "";
            if(taskTitleTextBox.Text == null || taskTitleTextBox.Text.Length < 1)
            {
                errorMsg += "Missing task name\n";
            }

            if(errorMsg.Length > 0)
            {
                InfoDialogForm infoDialog = new();
                infoDialog.EditInfoMessage(errorMsg);
                infoDialog.ShowDialog();
            } else {
                    editedTask.Title = taskTitleTextBox.Text;

                    if (editedTask.DueDate == null && deadlineCheckBox.Checked)
                    {
                        editedTask.DueDate = TimeZone.CurrentTimeZone.ToLocalTime(deadlineDateTimePicker.Value);
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
                        if(editedTask.Note.Text.CompareTo(noteTextBox.Text) != 0)
                        {
                        editedTask.Note.Text = noteTextBox.Text;
                        }
                    }

                    if (editedTask.Note != null && (noteTextBox.Text == null || noteTextBox.Text.Length < 1))
                    {
                        editedTask.Note = null;
                    }

                Label label = tasks.FirstOrDefault(x => x.Value == editedTask).Key;
                if (label.Parent is TableLayoutPanel panel)
                {
                    if (panel.GetControlFromPosition(0, 0) is Button b)
                    {
                        b.Image = editedTask.State == TaskState.DONE ? doneImage : incompletedImage;
                    }
                }
                label.Text = editedTask?.Title?.Replace('\n', ' ');

                if (editedTask?.Note != null)
                {
                    noteTextBox.Text = editedTask.Note.Text;
                    noteEditDateLabel.Text = "Edited: " + editedTask.Note.EditDate.ToString();
                }

                SaveData();
            }
        }

        private void DeleteTaskButton_Click(object sender, EventArgs e)
        {
            if (editedTask == null || selectedTaskList == null) return;

            DeleteDialogForm d = new();
            d.ShowDialog();

            if(d.DialogResult.Equals(DialogResult.OK)) {
                try
                {
                    taskManager.DeleteTask(selectedTaskList, editedTask.Id);

                    Label label = tasks.FirstOrDefault(x => x.Value == editedTask).Key;
                    if (label.Parent is Panel panel) { taskListPanel.Controls.Remove(panel); }

                    editedTask = null;
                    HideRightTaskEditPanel();
                } catch (TaskManagerException ex)
                {
                    ShowInfoDialog(ex.Message);
                }
            }
        }

        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            if(newTaskTextBox.Text == null || newTaskTextBox.Text.Length < 1 || selectedTaskList == null) return;

            try
            {
                if(selectedTaskList.Id < 1)
                {
                    editedTask = taskManager.CreateTask(taskManager.FindDefaultTaskList(), newTaskTextBox.Text, null, null, null);
                } else
                {
                    editedTask = taskManager.CreateTask(selectedTaskList, newTaskTextBox.Text, null, null, null);
                }

                newTaskTextBox.Text = "";

                taskListPanel.Controls.Add(CreateTaskLabel(editedTask));

                ParseDataToTaskPanel();
            } catch(TaskManagerException ex)
            {
                ShowInfoDialog(ex.Message);
            }
        }

        private void AddTaskListButton_Click(object sender, EventArgs e)
        {
            NewTaskListForm taskListForm = new();
            taskListForm.ShowDialog();
            if (taskListForm.DialogResult.Equals(DialogResult.OK))
            {
                string taskListName = taskListForm.ReadTaskListName();
                if(taskListName != null && taskListName.Length > 0)
                {
                    try
                    {
                        selectedTaskList = taskManager.CreateTaskList(taskListName);
                        taskListListPanel.Controls.Add(CreateTaskListBtn(selectedTaskList));
                        HideRightTaskEditPanel();
                        ShowTaskList(selectedTaskList);
                    } catch(TaskManagerException ex)
                    {
                        ShowInfoDialog(ex.Message);
                    }
                }
            }
        }

        private void DeleteTaskListButton_Click(object sender, EventArgs e)
        {
            if (selectedTaskList == null || selectedTaskList == taskManager.FindDefaultTaskList() || selectedTaskList.Id < 1) return;

            DeleteDialogForm deleteDialogForm = new();
            deleteDialogForm.ShowDialog();
            if(deleteDialogForm.DialogResult.Equals(DialogResult.OK))
            {
                try
                {
                    taskManager.DeleteTaskList(selectedTaskList.Id);
                    Button button = taskListButtons.FirstOrDefault(x => x.Value == selectedTaskList).Key;
                    if (button != null) { taskListListPanel.Controls.Remove(button); }

                    selectedTaskList = null;
                } catch(TaskManagerException ex)
                {
                    ShowInfoDialog(ex.Message);
                }
                ShowHomePage();
            }
        }

        private void TasksButton_Click(object sender, EventArgs e)
        {

            ShowTaskList(taskManager.FindDefaultTaskList());
            HideRightTaskEditPanel();
            editTaskListNameButton.Visible = false;
        }

        private void EditTaskListNameButton_Click(object sender, EventArgs e)
        {
            if (selectedTaskList == null) return;
            NewTaskListForm taskListForm = new();
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
                    SaveData();
                }
            }
        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            ShowHomePage();
            HideRightTaskEditPanel();
            editTaskListNameButton.Visible = false;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if(selectedTaskList == null) return;

            SearchDialogForm searchDialogForm = new();
            searchDialogForm.ShowDialog();
            if (searchDialogForm.DialogResult.Equals(DialogResult.OK))
            {
                String term = searchDialogForm.ReadSearchText();
                if(term != null && term.Length > 0)
                {
                    List<Task> terms = selectedTaskList.SearchTask(searchDialogForm.ReadSearchText());
                    ShowSearchList(terms);
                }
            }
        }

        private void EditLabelsButton_Click(object sender, EventArgs e)
        {
            AddLabelDialogForm addLabelDialog = new();
            addLabelDialog.RemoveActualLabels(editedTask?.Labels);
            addLabelDialog.ShowDialog();
            if(addLabelDialog.DialogResult.Equals(DialogResult.OK))
            {
                editedTask?.AddTaskLabel(addLabelDialog.AddedLabel);
                ParseLabels();
            }
        }
    }
}