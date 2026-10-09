namespace TransportERP.Desktop
{
    partial class UsersCommandBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            flpActions = new FlowLayoutPanel();
            btnNew = new Button();
            btnEdit = new Button();
            btnSave = new Button();
            btnDisable = new Button();
            btnResetPassword = new Button();
            btnClose = new Button();
            flpActions.SuspendLayout();
            SuspendLayout();
            // 
            // flpActions
            // 
            flpActions.Controls.Add(btnNew);
            flpActions.Controls.Add(btnEdit);
            flpActions.Controls.Add(btnSave);
            flpActions.Controls.Add(btnDisable);
            flpActions.Controls.Add(btnResetPassword);
            flpActions.Controls.Add(btnClose);
            flpActions.Dock = DockStyle.Fill;
            flpActions.Location = new Point(0, 0);
            flpActions.Margin = new Padding(3, 4, 3, 4);
            flpActions.Name = "flpActions";
            flpActions.Size = new Size(1473, 58);
            flpActions.TabIndex = 1;
            flpActions.WrapContents = false;
            // 
            // btnNew
            // 
            btnNew.AccessibleName = "إضافة";
            btnNew.Image = Properties.Resources.onyx_S01;
            btnNew.Location = new Point(1384, 4);
            btnNew.Margin = new Padding(3, 4, 3, 4);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(86, 31);
            btnNew.TabIndex = 0;
            // 
            // btnEdit
            // 
            btnEdit.AccessibleName = "تعديل";
            btnEdit.Enabled = false;
            btnEdit.Image = Properties.Resources.onyx_S02;
            btnEdit.Location = new Point(1292, 4);
            btnEdit.Margin = new Padding(3, 4, 3, 4);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(86, 31);
            btnEdit.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.AccessibleName = "حفظ";
            btnSave.Enabled = false;
            btnSave.Image = Properties.Resources.onyx_S10;
            btnSave.Location = new Point(1200, 4);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(86, 31);
            btnSave.TabIndex = 2;
            // 
            // btnDisable
            // 
            btnDisable.Enabled = false;
            btnDisable.Location = new Point(1108, 4);
            btnDisable.Margin = new Padding(3, 4, 3, 4);
            btnDisable.Name = "btnDisable";
            btnDisable.Size = new Size(86, 31);
            btnDisable.TabIndex = 3;
            btnDisable.Text = "إيقاف";
            // 
            // btnResetPassword
            // 
            btnResetPassword.Enabled = false;
            btnResetPassword.Location = new Point(891, 4);
            btnResetPassword.Margin = new Padding(3, 4, 3, 4);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.Size = new Size(211, 31);
            btnResetPassword.TabIndex = 4;
            btnResetPassword.Text = "إعادة تعيين كلمة المرور";
            // 
            // btnClose
            // 
            btnClose.AccessibleName = "خروج";
            btnClose.Image = Properties.Resources.onyx_S13;
            btnClose.Location = new Point(799, 4);
            btnClose.Margin = new Padding(3, 4, 3, 4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(86, 31);
            btnClose.TabIndex = 5;
            // 
            // UsersCommandBar
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flpActions);
            Name = "UsersCommandBar";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1473, 58);
            flpActions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flpActions;
        private Button btnNew;
        private Button btnEdit;
        private Button btnSave;
        private Button btnDisable;
        private Button btnResetPassword;
        private Button btnClose;
    }
}
