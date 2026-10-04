namespace TransportERP.Desktop
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            usersCommandBar1 = new UsersCommandBar();
            SuspendLayout();
            // 
            // usersCommandBar1
            // 
            usersCommandBar1.Dock = DockStyle.Top;
            usersCommandBar1.Location = new Point(18, 11);
            usersCommandBar1.Name = "usersCommandBar1";
            usersCommandBar1.RightToLeft = RightToLeft.Yes;
            usersCommandBar1.Size = new Size(1473, 58);
            usersCommandBar1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1509, 971);
            Controls.Add(usersCommandBar1);
            Name = "Form1";
            Padding = new Padding(18, 11, 18, 11);
            RightToLeft = RightToLeft.Yes;
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private UsersCommandBar usersCommandBar1;
    }
}