using System;
using Gtk;

namespace TodoApp
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("org.TodoApp.TodoApp", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new TodoWindow();
            app.AddWindow(win);

            win.ShowAll();
            Application.Run();
        }
    }
}