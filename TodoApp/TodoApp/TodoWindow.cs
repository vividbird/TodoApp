using System;
using Gtk;

namespace TodoApp
{
    public class TodoWindow : Window
    { 
        private Button AddButton;
        private Button RemoveButton;
        private Entry taskEntry;
        private ListStore store;
        private TreeView treeView;
        private VBox viewBox;

        public TodoWindow() : base("Todo app")
        {
            InitializeGraphicComponents();
        }
       
        #region Methods
        private void InitializeGraphicComponents()
        {
            Label label = new Label("Things to do");
            
            //Create a schroll window
            ScrolledWindow schrollWindow = new ScrolledWindow();
            schrollWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            
            //Add Button
            this.AddButton = new Button();
            this.AddButton.SetSizeRequest(10, 10);
            this.AddButton.Label = "Add";
            this.AddButton.Clicked += AddButton_OnClick;
            
            //Remove Button
            this.RemoveButton = new Button();
            this.RemoveButton.SetSizeRequest(10, 10);
            this.RemoveButton.Label = "Remove";
            this.RemoveButton.Clicked += RemoveButton_OnClick;
            
            //Adding entry
            this.taskEntry = new Entry();
            this.taskEntry.PlaceholderText = "Enter task";
            this.taskEntry.SetSizeRequest(150, 10);
            
            // Initializing treeView and liststore
            store = CreateModel();
            
            treeView = new TreeView(store);
            AddColumnsToTreeView(treeView);
            
            schrollWindow.Add(treeView);
            
            // Adding some of the components to a fixed view
            Fixed fixedView = new Fixed();
            fixedView.SetSizeRequest(300, 10);
            fixedView.Put(taskEntry, 5, 5);
            fixedView.Put(AddButton, 180, 5);
            fixedView.Put(RemoveButton, 250, 5);
 
            // Initializing the view box
            viewBox = new VBox(false, 8);
            
            viewBox.PackStart(label, false, false, 4);
            viewBox.PackStart(schrollWindow, true, true, 0);
            viewBox.PackStart(fixedView, false, false, 0);
            
            // Adding the view to the window
            this.Add(viewBox);
            
            // Window properties
            this.SetDefaultSize(350, 300);
            this.Resizable = false;
            
            // Adding event handlers to the window
            this.Destroyed+=Destroy;

        }
        private void AddColumnsToTreeView(TreeView treeView)
        {
            TreeViewColumn column;

            CellRendererToggle rendererToggle = new CellRendererToggle();
            rendererToggle.Toggled += new ToggledHandler(Toggle);
            column = new TreeViewColumn("Completed", rendererToggle, "active", Columns.IsCompleted);
            column.Sizing = TreeViewColumnSizing.Fixed;
            column.FixedWidth = 85;
            treeView.AppendColumn(column);

            CellRendererText rendererText = new CellRendererText();
            column = new TreeViewColumn("Task", rendererText, "text", Columns.Name);
            column.SortColumnId = (int) (Columns.Name);
            treeView.AppendColumn(column);
        }
        #endregion
        
        #region Events and event handlers
        
        private void AddButton_OnClick(object sender, EventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(this.taskEntry.Text))
            {
                Task task = new Task(this.taskEntry.Text, false);
                store.AppendValues(task.IsCompleted,task.Name);
                taskEntry.Text = string.Empty;
            }
            else
            {
                Gtk.MessageDialog md = new MessageDialog
                    (this,DialogFlags.Modal,MessageType.Info,ButtonsType.Close,"Cannot add an empty task");
                md.Run();
                md.Destroy();
            }   
        }

        private void RemoveButton_OnClick(object sender, EventArgs args)
        {
            Gtk.TreeIter selected;
            
            if (treeView.Selection.GetSelected(out selected))
            {
                store.Remove(ref selected);
            }
            else
            {
                Gtk.MessageDialog md = new MessageDialog
                    (this,DialogFlags.Modal,MessageType.Info,ButtonsType.Close,"Task is not chosen");
                md.Run();
                md.Destroy();
            }
        }
        
        private void Toggle(object o, ToggledArgs args)
        {
            Gtk.TreeIter iter;
            if (store.GetIterFromString (out iter, args.Path))
            {
                bool val = (bool) store.GetValue (iter, 0);
                store.SetValue (iter, 0, !val);
            }
        }
        private void Destroy(object o, EventArgs args)
        {
            Console.WriteLine("OnDestroy");
            Application.Quit();
        }
        #endregion
        
        private ListStore CreateModel()
        {
            return new ListStore(typeof(bool), typeof(string));
        }
    }
}