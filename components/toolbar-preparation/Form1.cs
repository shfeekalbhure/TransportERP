using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TransportERP.Desktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void EnableClickVerification(string outputPath)
        {
            var counts = Enum.GetValues<SharedUI.Commands.UsersToolbarCommand>()
                .ToDictionary(command => command.ToString(), _ => 0);
            var bindings = new SharedUI.Commands.ToolbarCommandBindings<SharedUI.Commands.UsersToolbarCommand>();
            void Record()
            {
                File.WriteAllText(outputPath, System.Text.Json.JsonSerializer.Serialize(new
                {
                    scope = "Real UI clicks with test callbacks only; no business services",
                    counts,
                    clientSize = new { ClientSize.Width, ClientSize.Height },
                    toolbar = new { usersCommandBar1.Name, usersCommandBar1.Width, usersCommandBar1.Height, dock = usersCommandBar1.Dock.ToString() }
                }, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            }
            foreach (var command in Enum.GetValues<SharedUI.Commands.UsersToolbarCommand>())
                bindings.SetBinding(command, () =>
                {
                    counts[command.ToString()]++;
                    Text = "Toolbar verification: " + command + " = " + counts[command.ToString()];
                    Record();
                    if (command == SharedUI.Commands.UsersToolbarCommand.CloseScreen) Close();
                }, () => true);
            usersCommandBar1.SetCommandBindings(bindings);
            Shown += (_, _) => Record();
        }
    }
}
