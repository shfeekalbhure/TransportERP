namespace TransportERP.Desktop.البوالص_والشحن.العمليات.توريد_مخزني
{
    partial class UcWarehouseReceiptOrder
    {
    private TransportERP.Desktop.CoreUI.AuditMetadataControl standardAuditMetadata = null!;

    private TransportERP.Desktop.CoreUI.DesignerCommandBar designerCommandBar = null!;
    private FlowLayoutPanel designerCommandFlow = null!;
    private Panel designerCloseHost = null!;
    private Button standardCommandEdit = null!;
    private Button standardCommandDelete = null!;
    private Button standardCommandCancel = null!;
    private Button standardCommandView = null!;
    private Button standardCommandLast = null!;
    private Button standardCommandNext = null!;
    private Button standardCommandPrevious = null!;
    private Button standardCommandFirst = null!;
    private Button standardCommandSave = null!;
    private Button standardCommandPrint = null!;
    private Button standardCommandClose = null!;
    private Button standardCommandRefresh = null!;
    private Button standardCommandImport = null!;
    private Button standardCommandExport = null!;
    private Button standardCommandHelp = null!;

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
        designerCommandBar = new TransportERP.Desktop.CoreUI.DesignerCommandBar();
        designerCommandFlow = new FlowLayoutPanel();
        designerCloseHost = new Panel();
        standardCommandEdit = new Button();
        standardCommandDelete = new Button();
        standardCommandCancel = new Button();
        standardCommandView = new Button();
        standardCommandLast = new Button();
        standardCommandNext = new Button();
        standardCommandPrevious = new Button();
        standardCommandFirst = new Button();
        standardCommandSave = new Button();
        standardCommandPrint = new Button();
        standardCommandClose = new Button();
        standardCommandRefresh = new Button();
        standardCommandImport = new Button();
        standardCommandExport = new Button();
        standardCommandHelp = new Button();
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
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Margin = new Padding(0);
            pnlMain.BackColor = Color.LightCyan;
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(1320, 842);
            pnlMain.TabIndex = 16;
            // 
            // tcReceiver
            // 
            tcReceiver.Controls.Add(tabReceiverData);
            tcReceiver.Controls.Add(tabReceiverAdditionalData);
            tcReceiver.Dock = DockStyle.Fill;
            tcReceiver.Font = new Font("Segoe UI", 9F);
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
            tabReceiverData.BackColor = Color.LightCyan;
            tabReceiverData.Controls.Add(flpWaybills);
            tabReceiverData.Location = new Point(4, 36);
            tabReceiverData.UseVisualStyleBackColor = false;
            tabReceiverData.Padding = new Padding(3);
            tabReceiverData.Margin = new Padding(0);
            tabReceiverData.Name = "tabReceiverData";
            tabReceiverData.Size = new Size(1586, 103);
            tabReceiverData.TabIndex = 2;
            tabReceiverData.Text = "بيانات المستلم";
            // 
            // flpWaybills
            // 
            flpWaybills.AllowDrop = true;
            flpWaybills.BackColor = Color.LightCyan;
            flpWaybills.Dock = DockStyle.Fill;
            flpWaybills.FlowDirection = FlowDirection.TopDown;
            flpWaybills.Location = new Point(0, 0);
            flpWaybills.Name = "flpWaybills";
            flpWaybills.AutoScroll = true;
            flpWaybills.Size = new Size(1586, 103);
            flpWaybills.TabIndex = 0;
            flpWaybills.WrapContents = false;
            // 
            // tabReceiverAdditionalData
            // 
            tabReceiverAdditionalData.AutoScroll = true;
            tabReceiverAdditionalData.BackColor = Color.LightCyan;
            tabReceiverAdditionalData.Controls.Add(tlpReceiverAdditionalData);
            tabReceiverAdditionalData.Location = new Point(4, 36);
            tabReceiverAdditionalData.UseVisualStyleBackColor = false;
            tabReceiverAdditionalData.Margin = new Padding(0);
            tabReceiverAdditionalData.Name = "tabReceiverAdditionalData";
            tabReceiverAdditionalData.Padding = new Padding(3);
            tabReceiverAdditionalData.Size = new Size(1586, 103);
            tabReceiverAdditionalData.TabIndex = 3;
            tabReceiverAdditionalData.Text = "بيانات اظافية";
            // 
            // tlpReceiverAdditionalData
            // 
            tlpReceiverAdditionalData.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpReceiverAdditionalData.BackColor = Color.LightCyan;
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
            tlpReceiverAdditionalData.Dock = DockStyle.Top;
            tlpReceiverAdditionalData.Location = new Point(3, 3);
            tlpReceiverAdditionalData.Margin = new Padding(0);
            tlpReceiverAdditionalData.Name = "tlpReceiverAdditionalData";
            tlpReceiverAdditionalData.MinimumSize = new Size(330, 352);
            tlpReceiverAdditionalData.Padding = new Padding(0);
            tlpReceiverAdditionalData.RightToLeft = RightToLeft.Yes;
            tlpReceiverAdditionalData.RowCount = 8;
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpReceiverAdditionalData.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpReceiverAdditionalData.Size = new Size(401, 352);
            tlpReceiverAdditionalData.TabIndex = 19;
            // 
            // comboBox9
            // 
            comboBox9.Dock = DockStyle.Fill;
            comboBox9.FormattingEnabled = true;
            comboBox9.Location = new Point(13, 173);
            comboBox9.BackColor = Color.White;
            comboBox9.ForeColor = Color.FromArgb(16, 24, 40);
            comboBox9.Name = "comboBox9";
            comboBox9.Margin = new Padding(3);
            comboBox9.Font = new Font("Segoe UI", 11F);
            comboBox9.Size = new Size(245, 29);
            comboBox9.TabIndex = 171;
            // 
            // textBox4
            // 
            textBox4.Dock = DockStyle.Fill;
            textBox4.Location = new Point(15, 135);
            textBox4.Margin = new Padding(3);
            textBox4.BackColor = Color.White;
            textBox4.ForeColor = Color.FromArgb(16, 24, 40);
            textBox4.Name = "textBox4";
            textBox4.Font = new Font("Segoe UI", 11F);
            textBox4.Size = new Size(241, 28);
            textBox4.TabIndex = 170;
            // 
            // textBox5
            // 
            textBox5.Dock = DockStyle.Fill;
            textBox5.Location = new Point(15, 95);
            textBox5.Margin = new Padding(3);
            textBox5.BackColor = Color.White;
            textBox5.ForeColor = Color.FromArgb(16, 24, 40);
            textBox5.Name = "textBox5";
            textBox5.Font = new Font("Segoe UI", 11F);
            textBox5.Size = new Size(241, 28);
            textBox5.TabIndex = 169;
            // 
            // textBox6
            // 
            textBox6.Dock = DockStyle.Fill;
            textBox6.Location = new Point(15, 295);
            textBox6.Margin = new Padding(3);
            textBox6.Multiline = true;
            textBox6.BackColor = Color.White;
            textBox6.ForeColor = Color.FromArgb(16, 24, 40);
            textBox6.Name = "textBox6";
            textBox6.Font = new Font("Segoe UI", 11F);
            textBox6.Size = new Size(241, 30);
            textBox6.TabIndex = 168;
            // 
            // label15
            // 
            label15.Dock = DockStyle.Fill;
            label15.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label15.Location = new Point(264, 290);
            label15.ForeColor = Color.FromArgb(16, 24, 40);
            label15.BackColor = Color.Transparent;
            label15.Name = "label15";
            label15.Margin = new Padding(3);
            label15.AutoEllipsis = true;
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
            dateTimePicker1.Margin = new Padding(3);
            dateTimePicker1.Font = new Font("Segoe UI", 11F);
            dateTimePicker1.Size = new Size(245, 28);
            dateTimePicker1.TabIndex = 166;
            // 
            // label16
            // 
            label16.Dock = DockStyle.Fill;
            label16.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label16.Location = new Point(264, 250);
            label16.ForeColor = Color.FromArgb(16, 24, 40);
            label16.BackColor = Color.Transparent;
            label16.Name = "label16";
            label16.Margin = new Padding(3);
            label16.AutoEllipsis = true;
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
            comboBox10.BackColor = Color.White;
            comboBox10.ForeColor = Color.FromArgb(16, 24, 40);
            comboBox10.Name = "comboBox10";
            comboBox10.Margin = new Padding(3);
            comboBox10.Font = new Font("Segoe UI", 11F);
            comboBox10.Size = new Size(245, 29);
            comboBox10.TabIndex = 164;
            // 
            // label17
            // 
            label17.Dock = DockStyle.Fill;
            label17.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label17.Location = new Point(264, 210);
            label17.ForeColor = Color.FromArgb(16, 24, 40);
            label17.BackColor = Color.Transparent;
            label17.Name = "label17";
            label17.Margin = new Padding(3);
            label17.AutoEllipsis = true;
            label17.Size = new Size(124, 40);
            label17.TabIndex = 163;
            label17.Text = "جهة الاصدار";
            label17.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            label18.Dock = DockStyle.Fill;
            label18.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label18.Location = new Point(264, 130);
            label18.ForeColor = Color.FromArgb(16, 24, 40);
            label18.BackColor = Color.Transparent;
            label18.Name = "label18";
            label18.Margin = new Padding(3);
            label18.AutoEllipsis = true;
            label18.Size = new Size(124, 40);
            label18.TabIndex = 162;
            label18.Text = "رقم الهوية";
            label18.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            label19.Dock = DockStyle.Fill;
            label19.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label19.Location = new Point(264, 170);
            label19.ForeColor = Color.FromArgb(16, 24, 40);
            label19.BackColor = Color.Transparent;
            label19.Name = "label19";
            label19.Margin = new Padding(3);
            label19.AutoEllipsis = true;
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
            comboBox11.BackColor = Color.White;
            comboBox11.ForeColor = Color.FromArgb(16, 24, 40);
            comboBox11.Name = "comboBox11";
            comboBox11.Margin = new Padding(3);
            comboBox11.Font = new Font("Segoe UI", 11F);
            comboBox11.Size = new Size(245, 29);
            comboBox11.TabIndex = 62;
            // 
            // label21
            // 
            label21.Dock = DockStyle.Fill;
            label21.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label21.Location = new Point(264, 50);
            label21.ForeColor = Color.FromArgb(16, 24, 40);
            label21.BackColor = Color.Transparent;
            label21.Name = "label21";
            label21.Margin = new Padding(3);
            label21.AutoEllipsis = true;
            label21.Size = new Size(124, 40);
            label21.TabIndex = 59;
            label21.Text = "ف";
            label21.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            label22.Dock = DockStyle.Fill;
            label22.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label22.Location = new Point(264, 90);
            label22.ForeColor = Color.FromArgb(16, 24, 40);
            label22.BackColor = Color.Transparent;
            label22.Name = "label22";
            label22.Margin = new Padding(3);
            label22.AutoEllipsis = true;
            label22.Size = new Size(124, 40);
            label22.TabIndex = 53;
            label22.Text = "ف";
            label22.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            label23.Dock = DockStyle.Fill;
            label23.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label23.Location = new Point(264, 10);
            label23.ForeColor = Color.FromArgb(16, 24, 40);
            label23.BackColor = Color.Transparent;
            label23.Name = "label23";
            label23.Margin = new Padding(3);
            label23.AutoEllipsis = true;
            label23.Size = new Size(124, 40);
            label23.TabIndex = 0;
            label23.Text = "ع";
            label23.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBox7
            // 
            textBox7.Dock = DockStyle.Fill;
            textBox7.Location = new Point(15, 15);
            textBox7.Margin = new Padding(3);
            textBox7.BackColor = Color.White;
            textBox7.ForeColor = Color.FromArgb(16, 24, 40);
            textBox7.Name = "textBox7";
            textBox7.Font = new Font("Segoe UI", 11F);
            textBox7.Size = new Size(241, 28);
            textBox7.TabIndex = 151;
            // 
            // panel1
            // 
            panel1.AccessibleName = "بنال بين الحاويات العلويه والسفلية";
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 49);
            panel1.Margin = new Padding(0);
            panel1.BackColor = Color.LightCyan;
            panel1.Name = "panel1";
            panel1.Size = new Size(1594, 237);
            panel1.TabIndex = 19;
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(designerCommandBar);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.BackColor = Color.LightCyan;
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1320, 60);
            pnlHeader.TabIndex = 18;
            // 
            // pnlActions
            // 

            pnlActions.Location = new Point(1210, 2);
            pnlActions.BackColor = Color.LightCyan;
            pnlActions.Name = "pnlActions";
            pnlActions.Dock = DockStyle.Right;
            pnlActions.Size = new Size(250, 60);
            pnlActions.TabIndex = 2;
            // 
            // btnNew
            // 
            btnNew.Location = new Point(78, 12);
            btnNew.BackColor = Color.FromArgb(224, 224, 224);
            btnNew.ForeColor = Color.FromArgb(16, 24, 40);
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Name = "btnNew";
            btnNew.Font = new Font("Microsoft Sans Serif", 10F);
            btnNew.Margin = new Padding(4);
            btnNew.Dock = DockStyle.None;
            btnNew.Size = new Size(70, 30);
            btnNew.TabIndex = 2;
            btnNew.Text = "جديد";
            btnNew.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = false;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(1480, 18);
            lblTitle.ForeColor = Color.FromArgb(16, 24, 40);
            lblTitle.BackColor = Color.FromArgb(192, 192, 255);
            lblTitle.Name = "lblTitle";
            lblTitle.AutoEllipsis = true;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Dock = DockStyle.Right;
            lblTitle.Size = new Size(180, 60);
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
            tlpAuditInfo.BackColor = Color.FromArgb(248, 250, 252);
            tlpAuditInfo.Name = "tlpAuditInfo";
            tlpAuditInfo.Padding = new Padding(8);
            tlpAuditInfo.RightToLeft = RightToLeft.Yes;
            tlpAuditInfo.RowCount = 1;
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAuditInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpAuditInfo.Size = new Size(1594, 48);
            tlpAuditInfo.TabIndex = 17;
            // 
            // lblPrintCount
            // 
            lblPrintCount.BackColor = Color.FromArgb(192, 255, 255);
            lblPrintCount.Dock = DockStyle.Fill;
            lblPrintCount.Font = new Font("Segoe UI", 10F);
            lblPrintCount.Location = new Point(3, 0);
            lblPrintCount.ForeColor = Color.FromArgb(16, 24, 40);
            lblPrintCount.Name = "lblPrintCount";
            lblPrintCount.Margin = new Padding(3);
            lblPrintCount.AutoEllipsis = true;
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
            lblLastPrintedAt.Font = new Font("Segoe UI", 10F);
            lblLastPrintedAt.Location = new Point(195, 0);
            lblLastPrintedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblLastPrintedAt.Name = "lblLastPrintedAt";
            lblLastPrintedAt.Margin = new Padding(3);
            lblLastPrintedAt.AutoEllipsis = true;
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
            lblEditCount.Font = new Font("Segoe UI", 10F);
            lblEditCount.Location = new Point(450, 0);
            lblEditCount.ForeColor = Color.FromArgb(16, 24, 40);
            lblEditCount.Name = "lblEditCount";
            lblEditCount.Margin = new Padding(3);
            lblEditCount.AutoEllipsis = true;
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
            lblModifiedAt.Font = new Font("Segoe UI", 10F);
            lblModifiedAt.Location = new Point(641, 0);
            lblModifiedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedAt.Name = "lblModifiedAt";
            lblModifiedAt.Margin = new Padding(3);
            lblModifiedAt.AutoEllipsis = true;
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
            lblModifiedBy.Font = new Font("Segoe UI", 10F);
            lblModifiedBy.Location = new Point(896, 0);
            lblModifiedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblModifiedBy.Name = "lblModifiedBy";
            lblModifiedBy.Margin = new Padding(3);
            lblModifiedBy.AutoEllipsis = true;
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
            lblCreatedAt.Font = new Font("Segoe UI", 10F);
            lblCreatedAt.Location = new Point(1119, 0);
            lblCreatedAt.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.Margin = new Padding(3);
            lblCreatedAt.AutoEllipsis = true;
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
            lblCreatedBy.Font = new Font("Segoe UI", 10F);
            lblCreatedBy.Location = new Point(1374, 0);
            lblCreatedBy.ForeColor = Color.FromArgb(16, 24, 40);
            lblCreatedBy.Name = "lblCreatedBy";
            lblCreatedBy.Margin = new Padding(3);
            lblCreatedBy.AutoEllipsis = true;
            lblCreatedBy.RightToLeft = RightToLeft.No;
            lblCreatedBy.Size = new Size(217, 33);
            lblCreatedBy.TabIndex = 0;
            lblCreatedBy.Text = " أنشأ بواسطة: - مدير النظام";
            lblCreatedBy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // UcWarehouseReceiptOrder
            // 
            AccessibleName = "امر توريد مخزني";
            AutoScaleDimensions = new SizeF(120F, 120F);
            ForeColor = Color.FromArgb(16, 24, 40);
            Padding = new Padding(0);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(248, 250, 252);
            Controls.Add(pnlMain);
            Font = new Font("Segoe UI", 10F);
            Margin = new Padding(0);
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
        
        // Explicit Designer-owned command catalog.
        designerCommandBar.Dock = DockStyle.Right;
        designerCommandBar.Location = new Point(1210, 2);
        designerCommandBar.Size = new Size(250, 60);
        designerCommandBar.TabIndex = 2;
        designerCommandBar.Controls.Add(pnlActions);
        pnlActions.Dock = DockStyle.Fill;
        pnlActions.Margin = Padding.Empty;
        designerCommandBar.Name = "designerCommandBar";
        designerCommandBar.BackColor = Color.FromArgb(239, 239, 239);
        designerCommandBar.BorderStyle = BorderStyle.FixedSingle;
        designerCommandBar.Controls.Add(designerCommandFlow);
        designerCommandBar.Controls.Add(designerCloseHost);
        designerCloseHost.Dock = DockStyle.Left;
        designerCloseHost.Width = 28;
        designerCloseHost.Name = "designerCloseHost";
        designerCommandFlow.Name = "designerCommandFlow";
        designerCommandFlow.Dock = DockStyle.Fill;
        designerCommandFlow.AutoSize = false;
        designerCommandFlow.WrapContents = false;
        designerCommandFlow.FlowDirection = FlowDirection.RightToLeft;
        designerCommandFlow.RightToLeft = RightToLeft.No;
        designerCommandFlow.Padding = new Padding(2);
        designerCommandBar.MinimumSize = new Size(0, 30);
        designerCommandBar.Height = 44;
        standardCommandEdit.Name = "standardCommandEdit";
        standardCommandEdit.Enabled = false;
        standardCommandEdit.Visible = true;
        standardCommandEdit.AccessibleName = "تعديل";
        standardCommandDelete.Name = "standardCommandDelete";
        standardCommandDelete.Enabled = false;
        standardCommandDelete.Visible = true;
        standardCommandDelete.AccessibleName = "حذف";
        standardCommandCancel.Name = "standardCommandCancel";
        standardCommandCancel.Enabled = false;
        standardCommandCancel.Visible = true;
        standardCommandCancel.AccessibleName = "تراجع";
        standardCommandView.Name = "standardCommandView";
        standardCommandView.Enabled = false;
        standardCommandView.Visible = true;
        standardCommandView.AccessibleName = "عرض";
        standardCommandLast.Name = "standardCommandLast";
        standardCommandLast.Enabled = false;
        standardCommandLast.Visible = true;
        standardCommandLast.AccessibleName = "الأخير";
        standardCommandNext.Name = "standardCommandNext";
        standardCommandNext.Enabled = false;
        standardCommandNext.Visible = true;
        standardCommandNext.AccessibleName = "التالي";
        standardCommandPrevious.Name = "standardCommandPrevious";
        standardCommandPrevious.Enabled = false;
        standardCommandPrevious.Visible = true;
        standardCommandPrevious.AccessibleName = "السابق";
        standardCommandFirst.Name = "standardCommandFirst";
        standardCommandFirst.Enabled = false;
        standardCommandFirst.Visible = true;
        standardCommandFirst.AccessibleName = "الأول";
        standardCommandSave.Name = "standardCommandSave";
        standardCommandSave.Enabled = false;
        standardCommandSave.Visible = true;
        standardCommandSave.AccessibleName = "حفظ";
        standardCommandPrint.Name = "standardCommandPrint";
        standardCommandPrint.Enabled = false;
        standardCommandPrint.Visible = true;
        standardCommandPrint.AccessibleName = "طباعة";
        standardCommandClose.Name = "standardCommandClose";
        standardCommandClose.Enabled = false;
        standardCommandClose.Visible = true;
        standardCommandClose.AccessibleName = "إغلاق";
        standardCommandRefresh.Name = "standardCommandRefresh";
        standardCommandRefresh.Enabled = false;
        standardCommandRefresh.Visible = false;
        standardCommandRefresh.AccessibleName = "تحديث";
        standardCommandImport.Name = "standardCommandImport";
        standardCommandImport.Enabled = false;
        standardCommandImport.Visible = false;
        standardCommandImport.AccessibleName = "استيراد";
        standardCommandExport.Name = "standardCommandExport";
        standardCommandExport.Enabled = false;
        standardCommandExport.Visible = false;
        standardCommandExport.AccessibleName = "تصدير";
        standardCommandHelp.Name = "standardCommandHelp";
        standardCommandHelp.Enabled = false;
        standardCommandHelp.Visible = false;
        standardCommandHelp.AccessibleName = "مساعدة";
        btnNew.AutoSize = false;
        btnNew.Dock = DockStyle.None;
        btnNew.MinimumSize = Size.Empty;
        btnNew.Size = new Size(26, 24);
        btnNew.Margin = new Padding(1);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        btnNew.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Add;
        btnNew.Text = "";
        designerCommandFlow.Controls.Add(btnNew);
        designerCommandBar.SetCommandRole(btnNew, TransportERP.Desktop.CoreUI.DesignerCommandRole.Add);
        standardCommandEdit.AutoSize = false;
        standardCommandEdit.Dock = DockStyle.None;
        standardCommandEdit.MinimumSize = Size.Empty;
        standardCommandEdit.Size = new Size(26, 24);
        standardCommandEdit.Margin = new Padding(1);
        standardCommandEdit.FlatStyle = FlatStyle.Flat;
        standardCommandEdit.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandEdit.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Edit;
        standardCommandEdit.Text = "";
        designerCommandFlow.Controls.Add(standardCommandEdit);
        designerCommandBar.SetCommandRole(standardCommandEdit, TransportERP.Desktop.CoreUI.DesignerCommandRole.Edit);
        standardCommandDelete.AutoSize = false;
        standardCommandDelete.Dock = DockStyle.None;
        standardCommandDelete.MinimumSize = Size.Empty;
        standardCommandDelete.Size = new Size(26, 24);
        standardCommandDelete.Margin = new Padding(1);
        standardCommandDelete.FlatStyle = FlatStyle.Flat;
        standardCommandDelete.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandDelete.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Delete;
        standardCommandDelete.Text = "";
        designerCommandFlow.Controls.Add(standardCommandDelete);
        designerCommandBar.SetCommandRole(standardCommandDelete, TransportERP.Desktop.CoreUI.DesignerCommandRole.Delete);
        standardCommandCancel.AutoSize = false;
        standardCommandCancel.Dock = DockStyle.None;
        standardCommandCancel.MinimumSize = Size.Empty;
        standardCommandCancel.Size = new Size(26, 24);
        standardCommandCancel.Margin = new Padding(1);
        standardCommandCancel.FlatStyle = FlatStyle.Flat;
        standardCommandCancel.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandCancel.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Cancel;
        standardCommandCancel.Text = "";
        designerCommandFlow.Controls.Add(standardCommandCancel);
        designerCommandBar.SetCommandRole(standardCommandCancel, TransportERP.Desktop.CoreUI.DesignerCommandRole.Cancel);
        standardCommandView.AutoSize = false;
        standardCommandView.Dock = DockStyle.None;
        standardCommandView.MinimumSize = Size.Empty;
        standardCommandView.Size = new Size(26, 24);
        standardCommandView.Margin = new Padding(1);
        standardCommandView.FlatStyle = FlatStyle.Flat;
        standardCommandView.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandView.Image = TransportERP.Desktop.CoreUI.CommandBarImages.View;
        standardCommandView.Text = "";
        designerCommandFlow.Controls.Add(standardCommandView);
        designerCommandBar.SetCommandRole(standardCommandView, TransportERP.Desktop.CoreUI.DesignerCommandRole.View);
        standardCommandLast.AutoSize = false;
        standardCommandLast.Dock = DockStyle.None;
        standardCommandLast.MinimumSize = Size.Empty;
        standardCommandLast.Size = new Size(26, 24);
        standardCommandLast.Margin = new Padding(1);
        standardCommandLast.FlatStyle = FlatStyle.Flat;
        standardCommandLast.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandLast.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Last;
        standardCommandLast.Text = "";
        designerCommandFlow.Controls.Add(standardCommandLast);
        designerCommandBar.SetCommandRole(standardCommandLast, TransportERP.Desktop.CoreUI.DesignerCommandRole.Last);
        standardCommandNext.AutoSize = false;
        standardCommandNext.Dock = DockStyle.None;
        standardCommandNext.MinimumSize = Size.Empty;
        standardCommandNext.Size = new Size(26, 24);
        standardCommandNext.Margin = new Padding(1);
        standardCommandNext.FlatStyle = FlatStyle.Flat;
        standardCommandNext.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandNext.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Next;
        standardCommandNext.Text = "";
        designerCommandFlow.Controls.Add(standardCommandNext);
        designerCommandBar.SetCommandRole(standardCommandNext, TransportERP.Desktop.CoreUI.DesignerCommandRole.Next);
        standardCommandPrevious.AutoSize = false;
        standardCommandPrevious.Dock = DockStyle.None;
        standardCommandPrevious.MinimumSize = Size.Empty;
        standardCommandPrevious.Size = new Size(26, 24);
        standardCommandPrevious.Margin = new Padding(1);
        standardCommandPrevious.FlatStyle = FlatStyle.Flat;
        standardCommandPrevious.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrevious.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Previous;
        standardCommandPrevious.Text = "";
        designerCommandFlow.Controls.Add(standardCommandPrevious);
        designerCommandBar.SetCommandRole(standardCommandPrevious, TransportERP.Desktop.CoreUI.DesignerCommandRole.Previous);
        standardCommandFirst.AutoSize = false;
        standardCommandFirst.Dock = DockStyle.None;
        standardCommandFirst.MinimumSize = Size.Empty;
        standardCommandFirst.Size = new Size(26, 24);
        standardCommandFirst.Margin = new Padding(1);
        standardCommandFirst.FlatStyle = FlatStyle.Flat;
        standardCommandFirst.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandFirst.Image = TransportERP.Desktop.CoreUI.CommandBarImages.First;
        standardCommandFirst.Text = "";
        designerCommandFlow.Controls.Add(standardCommandFirst);
        designerCommandBar.SetCommandRole(standardCommandFirst, TransportERP.Desktop.CoreUI.DesignerCommandRole.First);
        standardCommandSave.AutoSize = false;
        standardCommandSave.Dock = DockStyle.None;
        standardCommandSave.MinimumSize = Size.Empty;
        standardCommandSave.Size = new Size(26, 24);
        standardCommandSave.Margin = new Padding(1);
        standardCommandSave.FlatStyle = FlatStyle.Flat;
        standardCommandSave.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandSave.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Save;
        standardCommandSave.Text = "";
        designerCommandFlow.Controls.Add(standardCommandSave);
        designerCommandBar.SetCommandRole(standardCommandSave, TransportERP.Desktop.CoreUI.DesignerCommandRole.Save);
        standardCommandPrint.AutoSize = false;
        standardCommandPrint.Dock = DockStyle.None;
        standardCommandPrint.MinimumSize = Size.Empty;
        standardCommandPrint.Size = new Size(26, 24);
        standardCommandPrint.Margin = new Padding(1);
        standardCommandPrint.FlatStyle = FlatStyle.Flat;
        standardCommandPrint.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandPrint.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Print;
        standardCommandPrint.Text = "";
        designerCommandFlow.Controls.Add(standardCommandPrint);
        designerCommandBar.SetCommandRole(standardCommandPrint, TransportERP.Desktop.CoreUI.DesignerCommandRole.Print);
        standardCommandClose.AutoSize = false;
        standardCommandClose.Dock = DockStyle.None;
        standardCommandClose.MinimumSize = Size.Empty;
        standardCommandClose.Size = new Size(26, 24);
        standardCommandClose.Margin = new Padding(1);
        standardCommandClose.FlatStyle = FlatStyle.Flat;
        standardCommandClose.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandClose.Image = TransportERP.Desktop.CoreUI.CommandBarImages.Close;
        standardCommandClose.Text = "";
        designerCloseHost.Controls.Add(standardCommandClose);
        standardCommandClose.Location = new Point(1, 1);
        designerCommandBar.SetCommandRole(standardCommandClose, TransportERP.Desktop.CoreUI.DesignerCommandRole.Close);
        standardCommandRefresh.AutoSize = false;
        standardCommandRefresh.Dock = DockStyle.None;
        standardCommandRefresh.MinimumSize = Size.Empty;
        standardCommandRefresh.Size = new Size(26, 24);
        standardCommandRefresh.Margin = new Padding(1);
        standardCommandRefresh.FlatStyle = FlatStyle.Flat;
        standardCommandRefresh.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandRefresh.Text = "تحديث";
        designerCommandFlow.Controls.Add(standardCommandRefresh);
        designerCommandBar.SetCommandRole(standardCommandRefresh, TransportERP.Desktop.CoreUI.DesignerCommandRole.Refresh);
        standardCommandImport.AutoSize = false;
        standardCommandImport.Dock = DockStyle.None;
        standardCommandImport.MinimumSize = Size.Empty;
        standardCommandImport.Size = new Size(26, 24);
        standardCommandImport.Margin = new Padding(1);
        standardCommandImport.FlatStyle = FlatStyle.Flat;
        standardCommandImport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandImport.Text = "استيراد";
        designerCommandFlow.Controls.Add(standardCommandImport);
        designerCommandBar.SetCommandRole(standardCommandImport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Import);
        standardCommandExport.AutoSize = false;
        standardCommandExport.Dock = DockStyle.None;
        standardCommandExport.MinimumSize = Size.Empty;
        standardCommandExport.Size = new Size(26, 24);
        standardCommandExport.Margin = new Padding(1);
        standardCommandExport.FlatStyle = FlatStyle.Flat;
        standardCommandExport.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandExport.Text = "تصدير";
        designerCommandFlow.Controls.Add(standardCommandExport);
        designerCommandBar.SetCommandRole(standardCommandExport, TransportERP.Desktop.CoreUI.DesignerCommandRole.Export);
        standardCommandHelp.AutoSize = false;
        standardCommandHelp.Dock = DockStyle.None;
        standardCommandHelp.MinimumSize = Size.Empty;
        standardCommandHelp.Size = new Size(26, 24);
        standardCommandHelp.Margin = new Padding(1);
        standardCommandHelp.FlatStyle = FlatStyle.Flat;
        standardCommandHelp.FlatAppearance.BorderColor = Color.FromArgb(163, 163, 163);
        standardCommandHelp.Text = "مساعدة";
        designerCommandFlow.Controls.Add(standardCommandHelp);
        designerCommandBar.SetCommandRole(standardCommandHelp, TransportERP.Desktop.CoreUI.DesignerCommandRole.Help);
    
        // Shared audit presentation; original sources remain owned by this screen.
        standardAuditMetadata = new TransportERP.Desktop.CoreUI.AuditMetadataControl();
        standardAuditMetadata.Name = "standardAuditMetadata";
        standardAuditMetadata.Dock = DockStyle.Bottom;
        standardAuditMetadata.Margin = Padding.Empty;
        standardAuditMetadata.TabStop = false;
        standardAuditMetadata.Size = new Size(800, 64);
        tlpAuditInfo.Visible = false;
        Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
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
