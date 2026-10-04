namespace TransportERP.Desktop.البوالص_والشحن.العمليات.توريد_مخزني
{
    partial class UcWarehouseReceiptOrder
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
            pnlMain = new Panel();
            tcReceiver = new TabControl();
            tabReceiverData = new TabPage();
            flpWaybills = new FlowLayoutPanel();
            tabReceiverAdditionalData = new TabPage();
            tlpReceiverAdditionalData = new TableLayoutPanel();
            comboBox9 = new ComboBox();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            textBox6 = new TextBox();
            label15 = new Label();
            dateTimePicker1 = new DateTimePicker();
            label16 = new Label();
            comboBox10 = new ComboBox();
            label17 = new Label();
            label18 = new Label();
            label19 = new Label();
            comboBox11 = new ComboBox();
            label21 = new Label();
            label22 = new Label();
            label23 = new Label();
            textBox7 = new TextBox();
            panel1 = new Panel();
            pnlHeader = new Panel();
            pnlActions = new Panel();
            btnNew = new Button();
            lblTitle = new Label();
            tlpAuditInfo = new TableLayoutPanel();
            lblPrintCount = new Label();
            lblLastPrintedAt = new Label();
            lblEditCount = new Label();
            lblModifiedAt = new Label();
            lblModifiedBy = new Label();
            lblCreatedAt = new Label();
            lblCreatedBy = new Label();
            pnlMain.SuspendLayout();
            tcReceiver.SuspendLayout();
            tabReceiverData.SuspendLayout();
            tabReceiverAdditionalData.SuspendLayout();
            tlpReceiverAdditionalData.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlActions.SuspendLayout();
            tlpAuditInfo.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(tcReceiver);
            pnlMain.Controls.Add(panel1);
            pnlMain.Controls.Add(pnlHeader);
            pnlMain.Controls.Add(tlpAuditInfo);
            pnlMain.Dock = DockStyle.Left;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Margin = new Padding(0);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(1594, 842);
            pnlMain.TabIndex = 16;
            // 
            // tcReceiver
            // 
            tcReceiver.Controls.Add(tabReceiverData);
            tcReceiver.Controls.Add(tabReceiverAdditionalData);
            tcReceiver.Dock = DockStyle.Top;
            tcReceiver.Font = new Font("Tahoma", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tcReceiver.HotTrack = true;
            tcReceiver.ItemSize = new Size(130, 32);
            tcReceiver.Location = new Point(0, 286);
            tcReceiver.Margin = new Padding(0);
            tcReceiver.Name = "tcReceiver";
            tcReceiver.Padding = new Point(12, 3);
            tcReceiver.RightToLeft = RightToLeft.Yes;
            tcReceiver.RightToLeftLayout = true;
            tcReceiver.SelectedIndex = 0;
            tcReceiver.Size = new Size(1594, 143);
            tcReceiver.SizeMode = TabSizeMode.Fixed;
            tcReceiver.TabIndex = 25;
            // 
            // tabReceiverData
            // 
            tabReceiverData.AutoScroll = true;
            tabReceiverData.BackColor = Color.White;
            tabReceiverData.Controls.Add(flpWaybills);
            tabReceiverData.Location = new Point(4, 36);
            tabReceiverData.Name = "tabReceiverData";
            tabReceiverData.Size = new Size(1586, 103);
            tabReceiverData.TabIndex = 2;
            tabReceiverData.Text = "بيانات المستلم";
            // 
            // flpWaybills
            // 
            flpWaybills.AllowDrop = true;
            flpWaybills.BackColor = Color.Silver;
            flpWaybills.Dock = DockStyle.Fill;
            flpWaybills.FlowDirection = FlowDirection.TopDown;
            flpWaybills.Location = new Point(0, 0);
            flpWaybills.Name = "flpWaybills";
            flpWaybills.Size = new Size(1586, 103);
            flpWaybills.TabIndex = 0;
            flpWaybills.WrapContents = false;
            // 
            // tabReceiverAdditionalData
            // 
            tabReceiverAdditionalData.AutoScroll = true;
            tabReceiverAdditionalData.BackColor = Color.White;
            tabReceiverAdditionalData.Controls.Add(tlpReceiverAdditionalData);
            tabReceiverAdditionalData.Location = new Point(4, 36);
            tabReceiverAdditionalData.Name = "tabReceiverAdditionalData";
            tabReceiverAdditionalData.Padding = new Padding(3);
            tabReceiverAdditionalData.Size = new Size(1586, 103);
            tabReceiverAdditionalData.TabIndex = 3;
            tabReceiverAdditionalData.Text = "بيانات اظافية";
            // 
            // tlpReceiverAdditionalData
            // 
            tlpReceiverAdditionalData.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpReceiverAdditionalData.BackColor = Color.WhiteSmoke;
            tlpReceiverAdditionalData.ColumnCount = 2;
            tlpReceiverAdditionalData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tlpReceiverAdditionalData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tlpReceiverAdditionalData.Controls.Add(comboBox9, 1, 4);
            tlpReceiverAdditionalData.Controls.Add(textBox4, 1, 3);
            tlpReceiverAdditionalData.Controls.Add(textBox5, 1, 2);
            tlpReceiverAdditionalData.Controls.Add(textBox6, 1, 7);
            tlpReceiverAdditionalData.Controls.Add(label15, 0, 7);
            tlpReceiverAdditionalData.Controls.Add(dateTimePicker1, 1, 6);
            tlpReceiverAdditionalData.Controls.Add(label16, 0, 6);
            tlpReceiverAdditionalData.Controls.Add(comboBox10, 1, 5);
            tlpReceiverAdditionalData.Controls.Add(label17, 0, 5);
            tlpReceiverAdditionalData.Controls.Add(label18, 0, 3);
            tlpReceiverAdditionalData.Controls.Add(label19, 0, 4);
            tlpReceiverAdditionalData.Controls.Add(comboBox11, 1, 1);
            tlpReceiverAdditionalData.Controls.Add(label21, 0, 1);
            tlpReceiverAdditionalData.Controls.Add(label22, 0, 2);
            tlpReceiverAdditionalData.Controls.Add(label23, 0, 0);
            tlpReceiverAdditionalData.Controls.Add(textBox7, 1, 0);
            tlpReceiverAdditionalData.Dock = DockStyle.Left;
            tlpReceiverAdditionalData.Location = new Point(3, 3);
            tlpReceiverAdditionalData.Margin = new Padding(0);
            tlpReceiverAdditionalData.Name = "tlpReceiverAdditionalData";
            tlpReceiverAdditionalData.Padding = new Padding(10);
            tlpReceiverAdditionalData.RightToLeft = RightToLeft.Yes;
            tlpReceiverAdditionalData.RowCount = 8;
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.Size = new Size(401, 97);
            tlpReceiverAdditionalData.TabIndex = 19;
            // 
            // comboBox9
            // 
            comboBox9.Dock = DockStyle.Fill;
            comboBox9.FormattingEnabled = true;
            comboBox9.Location = new Point(13, 173);
            comboBox9.Name = "comboBox9";
            comboBox9.Size = new Size(245, 29);
            comboBox9.TabIndex = 171;
            // 
            // textBox4
            // 
            textBox4.Dock = DockStyle.Left;
            textBox4.Location = new Point(15, 135);
            textBox4.Margin = new Padding(5);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(241, 28);
            textBox4.TabIndex = 170;
            // 
            // textBox5
            // 
            textBox5.Dock = DockStyle.Left;
            textBox5.Location = new Point(15, 95);
            textBox5.Margin = new Padding(5);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(241, 28);
            textBox5.TabIndex = 169;
            // 
            // textBox6
            // 
            textBox6.Dock = DockStyle.Left;
            textBox6.Location = new Point(15, 295);
            textBox6.Margin = new Padding(5);
            textBox6.Multiline = true;
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(241, 30);
            textBox6.TabIndex = 168;
            // 
            // label15
            // 
            label15.Dock = DockStyle.Fill;
            label15.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label15.Location = new Point(264, 290);
            label15.Name = "label15";
            label15.Size = new Size(124, 40);
            label15.TabIndex = 167;
            label15.Text = "ملاحظات المرسل";
            label15.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Dock = DockStyle.Fill;
            dateTimePicker1.Location = new Point(13, 253);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(245, 28);
            dateTimePicker1.TabIndex = 166;
            // 
            // label16
            // 
            label16.Dock = DockStyle.Fill;
            label16.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label16.Location = new Point(264, 250);
            label16.Name = "label16";
            label16.Size = new Size(124, 40);
            label16.TabIndex = 165;
            label16.Text = "تاريخ الاصدار :";
            label16.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboBox10
            // 
            comboBox10.Dock = DockStyle.Fill;
            comboBox10.FormattingEnabled = true;
            comboBox10.Location = new Point(13, 213);
            comboBox10.Name = "comboBox10";
            comboBox10.Size = new Size(245, 29);
            comboBox10.TabIndex = 164;
            // 
            // label17
            // 
            label17.Dock = DockStyle.Fill;
            label17.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label17.Location = new Point(264, 210);
            label17.Name = "label17";
            label17.Size = new Size(124, 40);
            label17.TabIndex = 163;
            label17.Text = "جهة الاصدار";
            label17.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            label18.Dock = DockStyle.Fill;
            label18.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label18.Location = new Point(264, 130);
            label18.Name = "label18";
            label18.Size = new Size(124, 40);
            label18.TabIndex = 162;
            label18.Text = "رقم الهوية";
            label18.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            label19.Dock = DockStyle.Fill;
            label19.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label19.Location = new Point(264, 170);
            label19.Name = "label19";
            label19.Size = new Size(124, 40);
            label19.TabIndex = 155;
            label19.Text = "نوعها";
            label19.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboBox11
            // 
            comboBox11.Dock = DockStyle.Fill;
            comboBox11.FormattingEnabled = true;
            comboBox11.Location = new Point(13, 53);
            comboBox11.Name = "comboBox11";
            comboBox11.Size = new Size(245, 29);
            comboBox11.TabIndex = 62;
            // 
            // label21
            // 
            label21.Dock = DockStyle.Fill;
            label21.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label21.Location = new Point(264, 50);
            label21.Name = "label21";
            label21.Size = new Size(124, 40);
            label21.TabIndex = 59;
            label21.Text = "ف";
            label21.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            label22.Dock = DockStyle.Fill;
            label22.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label22.Location = new Point(264, 90);
            label22.Name = "label22";
            label22.Size = new Size(124, 40);
            label22.TabIndex = 53;
            label22.Text = "ف";
            label22.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            label23.Dock = DockStyle.Fill;
            label23.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold);
            label23.Location = new Point(264, 10);
            label23.Name = "label23";
            label23.Size = new Size(124, 40);
            label23.TabIndex = 0;
            label23.Text = "ع";
            label23.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBox7
            // 
            textBox7.Dock = DockStyle.Left;
            textBox7.Location = new Point(15, 15);
            textBox7.Margin = new Padding(5);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(241, 28);
            textBox7.TabIndex = 151;
            // 
            // panel1
            // 
            panel1.AccessibleName = "بنال بين الحاويات العلويه والسفلية";
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 49);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1594, 237);
            panel1.TabIndex = 19;
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(pnlActions);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1594, 49);
            pnlHeader.TabIndex = 18;
            // 
            // pnlActions
            // 
            pnlActions.Controls.Add(btnNew);
            pnlActions.Location = new Point(1210, 2);
            pnlActions.Name = "pnlActions";
            pnlActions.Size = new Size(250, 60);
            pnlActions.TabIndex = 2;
            // 
            // btnNew
            // 
            btnNew.Location = new Point(78, 12);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(94, 29);
            btnNew.TabIndex = 2;
            btnNew.Text = "جديد";
            btnNew.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(1480, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(136, 28);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "ترحيل الشحنات";
            // 
            // tlpAuditInfo
            // 
            tlpAuditInfo.AccessibleName = "البوليصه";
            tlpAuditInfo.ColumnCount = 7;
            tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
            tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            tlpAuditInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            tlpAuditInfo.Controls.Add(lblPrintCount, 6, 0);
            tlpAuditInfo.Controls.Add(lblLastPrintedAt, 5, 0);
            tlpAuditInfo.Controls.Add(lblEditCount, 4, 0);
            tlpAuditInfo.Controls.Add(lblModifiedAt, 3, 0);
            tlpAuditInfo.Controls.Add(lblModifiedBy, 2, 0);
            tlpAuditInfo.Controls.Add(lblCreatedAt, 1, 0);
            tlpAuditInfo.Controls.Add(lblCreatedBy, 0, 0);
            tlpAuditInfo.Dock = DockStyle.Bottom;
            tlpAuditInfo.Location = new Point(0, 809);
            tlpAuditInfo.Margin = new Padding(0);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.RightToLeft = RightToLeft.Yes;
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpAuditInfo.Size = new Size(1594, 33);
            tlpAuditInfo.TabIndex = 17;
            // 
            // lblPrintCount
            // 
            lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
            lblPrintCount.Dock = DockStyle.Fill;
            lblPrintCount.Font = new Font("Microsoft Sans Serif", 9F);
            lblPrintCount.Location = new Point(3, 0);
            lblPrintCount.Name = "lblPrintCount";
            lblPrintCount.RightToLeft = RightToLeft.No;
            lblPrintCount.Size = new Size(186, 33);
            lblPrintCount.TabIndex = 8;
            lblPrintCount.Text = "عدد مرات الطباعة :- 00";
            lblPrintCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLastPrintedAt
            // 
            lblLastPrintedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblLastPrintedAt.Dock = DockStyle.Fill;
            lblLastPrintedAt.Font = new Font("Microsoft Sans Serif", 9F);
            lblLastPrintedAt.Location = new Point(195, 0);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
            lblLastPrintedAt.RightToLeft = RightToLeft.No;
            lblLastPrintedAt.Size = new Size(249, 33);
            lblLastPrintedAt.TabIndex = 5;
            lblLastPrintedAt.Text = "تاريخ اخر طباعة:  27/09/2026";
            lblLastPrintedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblEditCount
            // 
            lblEditCount.BackColor = Color.FromArgb(192, 255, 255);
            lblEditCount.Dock = DockStyle.Fill;
            lblEditCount.Font = new Font("Microsoft Sans Serif", 9F);
            lblEditCount.Location = new Point(450, 0);
            lblEditCount.Name = "lblEditCount";
            lblEditCount.RightToLeft = RightToLeft.No;
            lblEditCount.Size = new Size(185, 33);
            lblEditCount.TabIndex = 4;
            lblEditCount.Text = "عدد التعديلات :- 00";
            lblEditCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedAt
            // 
            lblModifiedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedAt.Dock = DockStyle.Fill;
            lblModifiedAt.Font = new Font("Microsoft Sans Serif", 9F);
            lblModifiedAt.Location = new Point(641, 0);
            lblModifiedAt.Name = "lblModifiedAt";
            lblModifiedAt.RightToLeft = RightToLeft.No;
            lblModifiedAt.Size = new Size(249, 33);
            lblModifiedAt.TabIndex = 3;
            lblModifiedAt.Text = " تاريخ التعديل:  27/09/2026";
            lblModifiedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblModifiedBy
            // 
            lblModifiedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblModifiedBy.Dock = DockStyle.Fill;
            lblModifiedBy.Font = new Font("Microsoft Sans Serif", 9F);
            lblModifiedBy.Location = new Point(896, 0);
            lblModifiedBy.Name = "lblModifiedBy";
            lblModifiedBy.RightToLeft = RightToLeft.No;
            lblModifiedBy.Size = new Size(217, 33);
            lblModifiedBy.TabIndex = 2;
            lblModifiedBy.Text = "عدل بواسطة: - مدير النظام";
            lblModifiedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedAt.Dock = DockStyle.Fill;
            lblCreatedAt.Font = new Font("Microsoft Sans Serif", 9F);
            lblCreatedAt.Location = new Point(1119, 0);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.RightToLeft = RightToLeft.No;
            lblCreatedAt.Size = new Size(249, 33);
            lblCreatedAt.TabIndex = 1;
            lblCreatedAt.Text = " تاريخ الانشاء:  27/09/2026";
            lblCreatedAt.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCreatedBy
            // 
            lblCreatedBy.BackColor = Color.FromArgb(192, 255, 255);
            lblCreatedBy.Dock = DockStyle.Fill;
            lblCreatedBy.Font = new Font("Microsoft Sans Serif", 9F);
            lblCreatedBy.Location = new Point(1374, 0);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(217, 33);
            lblCreatedBy.TabIndex = 0;
            lblCreatedBy.Text = " أنشأ بواسطة: - مدير النظام";
            lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // UcWarehouseReceiptOrder
            // 
            AccessibleName = "امر توريد مخزني";
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(pnlMain);
            Font = new Font("Segoe UI", 10F);
            Margin = new Padding(8);
            Name = "UcWarehouseReceiptOrder";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1320, 842);
            pnlMain.ResumeLayout(false);
            tcReceiver.ResumeLayout(false);
            tabReceiverData.ResumeLayout(false);
            tabReceiverAdditionalData.ResumeLayout(false);
            tlpReceiverAdditionalData.ResumeLayout(false);
            tlpReceiverAdditionalData.PerformLayout();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlActions.ResumeLayout(false);
            tlpAuditInfo.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMain;
        private TableLayoutPanel tlpAuditInfo;
        private Label lblPrintCount;
        private Label lblLastPrintedAt;
        private Label lblEditCount;
        private Label lblModifiedAt;
        private Label lblModifiedBy;
        private Label lblCreatedAt;
        private Label lblCreatedBy;
        private Label lblTitle;
        private Panel pnlActions;
        private Button btnNew;
        private Panel panel1;
        private TabControl tcReceiver;
        private TabPage tabReceiverAdditionalData;
        private TableLayoutPanel tlpReceiverAdditionalData;
        private ComboBox comboBox9;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private Label label15;
        private DateTimePicker dateTimePicker1;
        private Label label16;
        private ComboBox comboBox10;
        private Label label17;
        private Label label18;
        private Label label19;
        private ComboBox comboBox11;
        private Label label21;
        private Label label22;
        private Label label23;
        private TextBox textBox7;
        private TabPage tabReceiverData;
        private FlowLayoutPanel flpWaybills;
        private Panel pnlHeader;
    }
}
