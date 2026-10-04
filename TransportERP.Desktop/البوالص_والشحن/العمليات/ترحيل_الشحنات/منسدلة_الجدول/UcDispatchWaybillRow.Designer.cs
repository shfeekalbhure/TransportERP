namespace TransportERP.Desktop.البوالص_والشحن.العمليات.ترحيل_الشحنات.منسدلة_الجدول
{
    partial class UcDispatchWaybillRow
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
            pnlWaybillHeader = new Panel();
            tlpWaybillRow = new TableLayoutPanel();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            textBox11 = new TextBox();
            txtDispatchStatus = new TextBox();
            txtCollectionOfficer1 = new TextBox();
            txtCollectionOfficer = new TextBox();
            txtCollectionParty = new TextBox();
            txtCollectionStatus = new TextBox();
            txtCurrency = new TextBox();
            txtRemainingAmount = new TextBox();
            txtPaidAmount = new TextBox();
            txtTotalAmount = new TextBox();
            txtPackageCount1 = new TextBox();
            txtWeight = new TextBox();
            txtPackageCount = new TextBox();
            txtRoute = new TextBox();
            txtReceiver = new TextBox();
            txtSender = new TextBox();
            txtWaybillNumber = new TextBox();
            chkRowSelect = new CheckBox();
            btnExpandWaybill = new Button();
            pnlPackages = new Panel();
            tpMainShipmentData = new TabControl();
            tabPage1 = new TabPage();
            dgvMainItems = new DataGridView();
            colLineNo = new DataGridViewTextBoxColumn();
            btnSelectItem = new DataGridViewCheckBoxColumn();
            colPackageType = new DataGridViewComboBoxColumn();
            colQuantity = new DataGridViewTextBoxColumn();
            colDescription = new DataGridViewTextBoxColumn();
            colQtyToDispatch = new DataGridViewTextBoxColumn();
            colNotes = new DataGridViewTextBoxColumn();
            colWeight = new DataGridViewTextBoxColumn();
            tpExtraData = new TabPage();
            pnlExtraData = new Panel();
            tblExtraData = new TableLayoutPanel();
            txtBarcodeScan = new TextBox();
            label6 = new Label();
            txtImprovementOfficer = new TextBox();
            label5 = new Label();
            txtTripSegment = new TextBox();
            label4 = new Label();
            label3 = new Label();
            cmbNextStation = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            cmbCurrentStation = new ComboBox();
            dtpDispatchDate = new DateTimePicker();
            pnlWaybillHeader.SuspendLayout();
            tlpWaybillRow.SuspendLayout();
            pnlPackages.SuspendLayout();
            tpMainShipmentData.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMainItems).BeginInit();
            tpExtraData.SuspendLayout();
            pnlExtraData.SuspendLayout();
            tblExtraData.SuspendLayout();
            SuspendLayout();
            // 
            // pnlWaybillHeader
            // 
            pnlWaybillHeader.Controls.Add(tlpWaybillRow);
            pnlWaybillHeader.Dock = DockStyle.Top;
            pnlWaybillHeader.Location = new Point(0, 0);
            pnlWaybillHeader.Name = "pnlWaybillHeader";
            pnlWaybillHeader.Size = new Size(1100, 54);
            pnlWaybillHeader.TabIndex = 0;
            // 
            // tlpWaybillRow
            // 
            tlpWaybillRow.BackColor = Color.White;
            tlpWaybillRow.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlpWaybillRow.ColumnCount = 21;
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 85F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 85F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpWaybillRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpWaybillRow.Controls.Add(textBox2, 20, 0);
            tlpWaybillRow.Controls.Add(textBox1, 19, 0);
            tlpWaybillRow.Controls.Add(textBox11, 18, 0);
            tlpWaybillRow.Controls.Add(txtDispatchStatus, 17, 0);
            tlpWaybillRow.Controls.Add(txtCollectionOfficer1, 16, 0);
            tlpWaybillRow.Controls.Add(txtCollectionOfficer, 15, 0);
            tlpWaybillRow.Controls.Add(txtCollectionParty, 14, 0);
            tlpWaybillRow.Controls.Add(txtCollectionStatus, 13, 0);
            tlpWaybillRow.Controls.Add(txtCurrency, 12, 0);
            tlpWaybillRow.Controls.Add(txtRemainingAmount, 11, 0);
            tlpWaybillRow.Controls.Add(txtPaidAmount, 10, 0);
            tlpWaybillRow.Controls.Add(txtTotalAmount, 9, 0);
            tlpWaybillRow.Controls.Add(txtPackageCount1, 8, 0);
            tlpWaybillRow.Controls.Add(txtWeight, 7, 0);
            tlpWaybillRow.Controls.Add(txtPackageCount, 6, 0);
            tlpWaybillRow.Controls.Add(txtRoute, 5, 0);
            tlpWaybillRow.Controls.Add(txtReceiver, 4, 0);
            tlpWaybillRow.Controls.Add(txtSender, 3, 0);
            tlpWaybillRow.Controls.Add(txtWaybillNumber, 2, 0);
            tlpWaybillRow.Controls.Add(chkRowSelect, 1, 0);
            tlpWaybillRow.Controls.Add(btnExpandWaybill, 0, 0);
            tlpWaybillRow.Dock = DockStyle.Top;
            tlpWaybillRow.Location = new Point(0, 0);
            tlpWaybillRow.Margin = new Padding(0);
            tlpWaybillRow.Name = "tlpWaybillRow";
            tlpWaybillRow.RowCount = 1;
            tlpWaybillRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpWaybillRow.Size = new Size(1100, 40);
            tlpWaybillRow.TabIndex = 10;
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Location = new Point(-1248, 4);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(104, 27);
            textBox2.TabIndex = 48;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Location = new Point(-1137, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(104, 27);
            textBox1.TabIndex = 47;
            // 
            // textBox11
            // 
            textBox11.Dock = DockStyle.Fill;
            textBox11.Location = new Point(-1026, 4);
            textBox11.Name = "textBox11";
            textBox11.Size = new Size(104, 27);
            textBox11.TabIndex = 46;
            // 
            // txtDispatchStatus
            // 
            txtDispatchStatus.Dock = DockStyle.Fill;
            txtDispatchStatus.Location = new Point(-915, 4);
            txtDispatchStatus.Name = "txtDispatchStatus";
            txtDispatchStatus.Size = new Size(104, 27);
            txtDispatchStatus.TabIndex = 45;
            // 
            // txtCollectionOfficer1
            // 
            txtCollectionOfficer1.Dock = DockStyle.Fill;
            txtCollectionOfficer1.Location = new Point(-804, 4);
            txtCollectionOfficer1.Name = "txtCollectionOfficer1";
            txtCollectionOfficer1.Size = new Size(104, 27);
            txtCollectionOfficer1.TabIndex = 44;
            // 
            // txtCollectionOfficer
            // 
            txtCollectionOfficer.Dock = DockStyle.Fill;
            txtCollectionOfficer.Location = new Point(-693, 4);
            txtCollectionOfficer.Name = "txtCollectionOfficer";
            txtCollectionOfficer.Size = new Size(104, 27);
            txtCollectionOfficer.TabIndex = 43;
            // 
            // txtCollectionParty
            // 
            txtCollectionParty.Dock = DockStyle.Fill;
            txtCollectionParty.Location = new Point(-582, 4);
            txtCollectionParty.Name = "txtCollectionParty";
            txtCollectionParty.Size = new Size(104, 27);
            txtCollectionParty.TabIndex = 42;
            // 
            // txtCollectionStatus
            // 
            txtCollectionStatus.Dock = DockStyle.Fill;
            txtCollectionStatus.Location = new Point(-471, 4);
            txtCollectionStatus.Name = "txtCollectionStatus";
            txtCollectionStatus.Size = new Size(144, 27);
            txtCollectionStatus.TabIndex = 41;
            // 
            // txtCurrency
            // 
            txtCurrency.Dock = DockStyle.Fill;
            txtCurrency.Location = new Point(-320, 4);
            txtCurrency.Name = "txtCurrency";
            txtCurrency.Size = new Size(74, 27);
            txtCurrency.TabIndex = 40;
            // 
            // txtRemainingAmount
            // 
            txtRemainingAmount.Dock = DockStyle.Fill;
            txtRemainingAmount.Location = new Point(-239, 4);
            txtRemainingAmount.Name = "txtRemainingAmount";
            txtRemainingAmount.Size = new Size(104, 27);
            txtRemainingAmount.TabIndex = 39;
            // 
            // txtPaidAmount
            // 
            txtPaidAmount.Dock = DockStyle.Fill;
            txtPaidAmount.Location = new Point(-128, 4);
            txtPaidAmount.Name = "txtPaidAmount";
            txtPaidAmount.Size = new Size(104, 27);
            txtPaidAmount.TabIndex = 38;
            // 
            // txtTotalAmount
            // 
            txtTotalAmount.Dock = DockStyle.Fill;
            txtTotalAmount.Location = new Point(-17, 4);
            txtTotalAmount.Name = "txtTotalAmount";
            txtTotalAmount.Size = new Size(104, 27);
            txtTotalAmount.TabIndex = 37;
            // 
            // txtPackageCount1
            // 
            txtPackageCount1.Dock = DockStyle.Fill;
            txtPackageCount1.Location = new Point(94, 4);
            txtPackageCount1.Name = "txtPackageCount1";
            txtPackageCount1.Size = new Size(104, 27);
            txtPackageCount1.TabIndex = 36;
            // 
            // txtWeight
            // 
            txtWeight.Dock = DockStyle.Fill;
            txtWeight.Location = new Point(205, 4);
            txtWeight.Name = "txtWeight";
            txtWeight.Size = new Size(79, 27);
            txtWeight.TabIndex = 35;
            // 
            // txtPackageCount
            // 
            txtPackageCount.Dock = DockStyle.Fill;
            txtPackageCount.Location = new Point(291, 4);
            txtPackageCount.Name = "txtPackageCount";
            txtPackageCount.Size = new Size(79, 27);
            txtPackageCount.TabIndex = 34;
            // 
            // txtRoute
            // 
            txtRoute.Dock = DockStyle.Fill;
            txtRoute.Location = new Point(377, 4);
            txtRoute.Name = "txtRoute";
            txtRoute.Size = new Size(134, 27);
            txtRoute.TabIndex = 33;
            // 
            // txtReceiver
            // 
            txtReceiver.Dock = DockStyle.Fill;
            txtReceiver.Location = new Point(518, 4);
            txtReceiver.Name = "txtReceiver";
            txtReceiver.Size = new Size(174, 27);
            txtReceiver.TabIndex = 32;
            // 
            // txtSender
            // 
            txtSender.Dock = DockStyle.Fill;
            txtSender.Location = new Point(699, 4);
            txtSender.Name = "txtSender";
            txtSender.Size = new Size(174, 27);
            txtSender.TabIndex = 31;
            // 
            // txtWaybillNumber
            // 
            txtWaybillNumber.Dock = DockStyle.Fill;
            txtWaybillNumber.Location = new Point(880, 4);
            txtWaybillNumber.Name = "txtWaybillNumber";
            txtWaybillNumber.Size = new Size(104, 27);
            txtWaybillNumber.TabIndex = 30;
            // 
            // chkRowSelect
            // 
            chkRowSelect.AutoSize = true;
            chkRowSelect.Dock = DockStyle.Left;
            chkRowSelect.Location = new Point(1047, 4);
            chkRowSelect.Name = "chkRowSelect";
            chkRowSelect.Size = new Size(18, 34);
            chkRowSelect.TabIndex = 29;
            chkRowSelect.UseVisualStyleBackColor = true;
            // 
            // btnExpandWaybill
            // 
            btnExpandWaybill.Dock = DockStyle.Fill;
            btnExpandWaybill.FlatStyle = FlatStyle.Flat;
            btnExpandWaybill.Location = new Point(1072, 4);
            btnExpandWaybill.Name = "btnExpandWaybill";
            btnExpandWaybill.Size = new Size(24, 34);
            btnExpandWaybill.TabIndex = 28;
            btnExpandWaybill.TabStop = false;
            btnExpandWaybill.Text = "+";
            btnExpandWaybill.UseVisualStyleBackColor = true;
            btnExpandWaybill.Click += btnExpandWaybill_Click;
            // 
            // pnlPackages
            // 
            pnlPackages.BorderStyle = BorderStyle.FixedSingle;
            pnlPackages.Controls.Add(tpMainShipmentData);
            pnlPackages.Dock = DockStyle.Top;
            pnlPackages.Location = new Point(0, 54);
            pnlPackages.Name = "pnlPackages";
            pnlPackages.Size = new Size(1100, 274);
            pnlPackages.TabIndex = 2;
            pnlPackages.Visible = false;
            // 
            // tpMainShipmentData
            // 
            tpMainShipmentData.Controls.Add(tabPage1);
            tpMainShipmentData.Controls.Add(tpExtraData);
            tpMainShipmentData.Dock = DockStyle.Fill;
            tpMainShipmentData.Location = new Point(0, 0);
            tpMainShipmentData.Name = "tpMainShipmentData";
            tpMainShipmentData.RightToLeftLayout = true;
            tpMainShipmentData.SelectedIndex = 0;
            tpMainShipmentData.Size = new Size(1098, 272);
            tpMainShipmentData.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dgvMainItems);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1090, 239);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "بينات المنقول الاساسية";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvMainItems
            // 
            dgvMainItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMainItems.Columns.AddRange(new DataGridViewColumn[] { colLineNo, btnSelectItem, colPackageType, colQuantity, colDescription, colQtyToDispatch, colNotes, colWeight });
            dgvMainItems.Dock = DockStyle.Fill;
            dgvMainItems.Location = new Point(3, 3);
            dgvMainItems.Name = "dgvMainItems";
            dgvMainItems.RowHeadersWidth = 51;
            dgvMainItems.Size = new Size(1084, 233);
            dgvMainItems.TabIndex = 0;
            // 
            // colLineNo
            // 
            colLineNo.HeaderText = "م";
            colLineNo.MinimumWidth = 6;
            colLineNo.Name = "colLineNo";
            colLineNo.Width = 60;
            // 
            // btnSelectItem
            // 
            btnSelectItem.HeaderText = "اختيار";
            btnSelectItem.MinimumWidth = 6;
            btnSelectItem.Name = "btnSelectItem";
            btnSelectItem.Width = 125;
            // 
            // colPackageType
            // 
            colPackageType.HeaderText = "نوع المنقول";
            colPackageType.MinimumWidth = 6;
            colPackageType.Name = "colPackageType";
            colPackageType.Resizable = DataGridViewTriState.True;
            colPackageType.SortMode = DataGridViewColumnSortMode.Automatic;
            colPackageType.Width = 125;
            // 
            // colQuantity
            // 
            colQuantity.HeaderText = "الكمية";
            colQuantity.MinimumWidth = 6;
            colQuantity.Name = "colQuantity";
            colQuantity.Width = 125;
            // 
            // colDescription
            // 
            colDescription.HeaderText = "الوصف";
            colDescription.MinimumWidth = 6;
            colDescription.Name = "colDescription";
            colDescription.Width = 300;
            // 
            // colQtyToDispatch
            // 
            colQtyToDispatch.HeaderText = "الكمية الصادرة";
            colQtyToDispatch.MinimumWidth = 6;
            colQtyToDispatch.Name = "colQtyToDispatch";
            colQtyToDispatch.Width = 125;
            // 
            // colNotes
            // 
            colNotes.HeaderText = "ملاحظة";
            colNotes.MinimumWidth = 6;
            colNotes.Name = "colNotes";
            colNotes.Width = 125;
            // 
            // colWeight
            // 
            colWeight.HeaderText = "الوزن";
            colWeight.MinimumWidth = 6;
            colWeight.Name = "colWeight";
            colWeight.Resizable = DataGridViewTriState.True;
            colWeight.Width = 125;
            // 
            // tpExtraData
            // 
            tpExtraData.Controls.Add(pnlExtraData);
            tpExtraData.Location = new Point(4, 29);
            tpExtraData.Name = "tpExtraData";
            tpExtraData.Padding = new Padding(3);
            tpExtraData.Size = new Size(1090, 239);
            tpExtraData.TabIndex = 1;
            tpExtraData.Text = "بينات اضافية";
            tpExtraData.UseVisualStyleBackColor = true;
            // 
            // pnlExtraData
            // 
            pnlExtraData.Controls.Add(tblExtraData);
            pnlExtraData.Dock = DockStyle.Fill;
            pnlExtraData.Location = new Point(3, 3);
            pnlExtraData.Name = "pnlExtraData";
            pnlExtraData.Size = new Size(1084, 233);
            pnlExtraData.TabIndex = 0;
            // 
            // tblExtraData
            // 
            tblExtraData.ColumnCount = 4;
            tblExtraData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tblExtraData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tblExtraData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tblExtraData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tblExtraData.Controls.Add(txtBarcodeScan, 3, 2);
            tblExtraData.Controls.Add(label6, 2, 2);
            tblExtraData.Controls.Add(txtImprovementOfficer, 1, 2);
            tblExtraData.Controls.Add(label5, 0, 2);
            tblExtraData.Controls.Add(txtTripSegment, 3, 1);
            tblExtraData.Controls.Add(label4, 2, 1);
            tblExtraData.Controls.Add(label3, 0, 1);
            tblExtraData.Controls.Add(cmbNextStation, 3, 0);
            tblExtraData.Controls.Add(label2, 2, 0);
            tblExtraData.Controls.Add(label1, 0, 0);
            tblExtraData.Controls.Add(cmbCurrentStation, 1, 0);
            tblExtraData.Controls.Add(dtpDispatchDate, 1, 1);
            tblExtraData.Dock = DockStyle.Fill;
            tblExtraData.Location = new Point(0, 0);
            tblExtraData.Name = "tblExtraData";
            tblExtraData.RowCount = 3;
            tblExtraData.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tblExtraData.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tblExtraData.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tblExtraData.Size = new Size(1084, 233);
            tblExtraData.TabIndex = 0;
            // 
            // txtBarcodeScan
            // 
            txtBarcodeScan.Dock = DockStyle.Fill;
            txtBarcodeScan.Location = new Point(3, 157);
            txtBarcodeScan.Name = "txtBarcodeScan";
            txtBarcodeScan.Size = new Size(357, 27);
            txtBarcodeScan.TabIndex = 37;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(448, 154);
            label6.Name = "label6";
            label6.Size = new Size(92, 20);
            label6.TabIndex = 36;
            label6.Text = "مسح البركورد";
            // 
            // txtImprovementOfficer
            // 
            txtImprovementOfficer.Dock = DockStyle.Fill;
            txtImprovementOfficer.Location = new Point(546, 157);
            txtImprovementOfficer.Name = "txtImprovementOfficer";
            txtImprovementOfficer.Size = new Size(355, 27);
            txtImprovementOfficer.TabIndex = 35;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(969, 154);
            label5.Name = "label5";
            label5.Size = new Size(112, 20);
            label5.TabIndex = 34;
            label5.Text = "مسؤول التحسين";
            // 
            // txtTripSegment
            // 
            txtTripSegment.Dock = DockStyle.Fill;
            txtTripSegment.Location = new Point(3, 80);
            txtTripSegment.Name = "txtTripSegment";
            txtTripSegment.Size = new Size(357, 27);
            txtTripSegment.TabIndex = 33;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(451, 77);
            label4.Name = "label4";
            label4.Size = new Size(89, 20);
            label4.TabIndex = 6;
            label4.Text = "مقطع الرحلة";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(994, 77);
            label3.Name = "label3";
            label3.Size = new Size(87, 20);
            label3.TabIndex = 4;
            label3.Text = "تاريخ الترحيل";
            // 
            // cmbNextStation
            // 
            cmbNextStation.FormattingEnabled = true;
            cmbNextStation.Location = new Point(209, 3);
            cmbNextStation.Name = "cmbNextStation";
            cmbNextStation.Size = new Size(151, 28);
            cmbNextStation.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(443, 0);
            label2.Name = "label2";
            label2.Size = new Size(97, 20);
            label2.TabIndex = 2;
            label2.Text = "المحطة التالية";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(980, 0);
            label1.Name = "label1";
            label1.Size = new Size(101, 20);
            label1.TabIndex = 0;
            label1.Text = "المحطة الحالية";
            // 
            // cmbCurrentStation
            // 
            cmbCurrentStation.FormattingEnabled = true;
            cmbCurrentStation.Location = new Point(750, 3);
            cmbCurrentStation.Name = "cmbCurrentStation";
            cmbCurrentStation.Size = new Size(151, 28);
            cmbCurrentStation.TabIndex = 1;
            // 
            // dtpDispatchDate
            // 
            dtpDispatchDate.Location = new Point(651, 80);
            dtpDispatchDate.Name = "dtpDispatchDate";
            dtpDispatchDate.Size = new Size(250, 27);
            dtpDispatchDate.TabIndex = 5;
            // 
            // UcDispatchWaybillRow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(pnlPackages);
            Controls.Add(pnlWaybillHeader);
            Name = "UcDispatchWaybillRow";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1100, 350);
            pnlWaybillHeader.ResumeLayout(false);
            tlpWaybillRow.ResumeLayout(false);
            tlpWaybillRow.PerformLayout();
            pnlPackages.ResumeLayout(false);
            tpMainShipmentData.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMainItems).EndInit();
            tpExtraData.ResumeLayout(false);
            pnlExtraData.ResumeLayout(false);
            tblExtraData.ResumeLayout(false);
            tblExtraData.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlWaybillHeader;
        private TableLayoutPanel tlpWaybillRow;
        private TextBox textBox2;
        private TextBox textBox1;
        private TextBox textBox11;
        private TextBox txtDispatchStatus;
        private TextBox txtCollectionOfficer1;
        private TextBox txtCollectionOfficer;
        private TextBox txtCollectionParty;
        private TextBox txtCollectionStatus;
        private TextBox txtCurrency;
        private TextBox txtRemainingAmount;
        private TextBox txtPaidAmount;
        private TextBox txtTotalAmount;
        private TextBox txtPackageCount1;
        private TextBox txtWeight;
        private TextBox txtPackageCount;
        private TextBox txtRoute;
        private TextBox txtReceiver;
        private TextBox txtSender;
        private TextBox txtWaybillNumber;
        private CheckBox chkRowSelect;
        private Button btnExpandWaybill;
        private Panel pnlPackages;
        private TabControl tpMainShipmentData;
        private TabPage tabPage1;
        private TabPage tpExtraData;
        private DataGridView dgvMainItems;
        private Panel pnlExtraData;
        private TableLayoutPanel tblExtraData;
        private Label label3;
        private ComboBox cmbNextStation;
        private Label label2;
        private Label label1;
        private ComboBox cmbCurrentStation;
        private DateTimePicker dtpDispatchDate;
        private Label label4;
        private TextBox txtTripSegment;
        private TextBox txtBarcodeScan;
        private Label label6;
        private TextBox txtImprovementOfficer;
        private Label label5;
        private DataGridViewTextBoxColumn colLineNo;
        private DataGridViewCheckBoxColumn btnSelectItem;
        private DataGridViewComboBoxColumn colPackageType;
        private DataGridViewTextBoxColumn colQuantity;
        private DataGridViewTextBoxColumn colDescription;
        private DataGridViewTextBoxColumn colQtyToDispatch;
        private DataGridViewTextBoxColumn colNotes;
        private DataGridViewTextBoxColumn colWeight;
    }
}
