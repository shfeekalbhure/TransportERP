#nullable enable
namespace TransportERP.EmptyForms;
partial class ApprovalQueuePicker
{
    private TableLayoutPanel layout = null!;
    private FlowLayoutPanel actions = null!;
    private TextBox txtSearch = null!;
    private ListBox lstRequests = null!;
    private Button btnSearch = null!,btnPrevious = null!,btnNext = null!,btnSelect = null!;
    private Label lblQueueStatus = null!;
    private void InitializeComponent()
    {
        layout=new TableLayoutPanel();actions=new FlowLayoutPanel();txtSearch=new TextBox();lstRequests=new ListBox();
        btnSearch=new Button();btnPrevious=new Button();btnNext=new Button();btnSelect=new Button();lblQueueStatus=new Label();
        SuspendLayout();
        layout.Name="queueLayout";layout.Dock=DockStyle.Fill;layout.ColumnCount=1;layout.RowCount=3;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));layout.RowStyles.Add(new RowStyle(SizeType.Percent,100F));layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        actions.Name="queueActions";actions.Dock=DockStyle.Fill;actions.AutoSize=true;actions.WrapContents=true;
        txtSearch.Name="txtQueueSearch";txtSearch.AccessibleName="بحث الطلبات";txtSearch.Width=220;txtSearch.TabIndex=0;
        btnSearch.Name="btnQueueSearch";btnSearch.Text="بحث";btnSearch.AutoSize=true;btnSearch.TabIndex=1;
        btnPrevious.Name="btnQueuePrevious";btnPrevious.Text="السابق";btnPrevious.AutoSize=true;btnPrevious.Enabled=false;btnPrevious.TabIndex=2;
        btnNext.Name="btnQueueNext";btnNext.Text="التالي";btnNext.AutoSize=true;btnNext.Enabled=false;btnNext.TabIndex=3;
        btnSelect.Name="btnQueueSelect";btnSelect.Text="عرض الطلب";btnSelect.AutoSize=true;btnSelect.TabIndex=4;
        actions.Controls.Add(txtSearch);actions.Controls.Add(btnSearch);actions.Controls.Add(btnPrevious);actions.Controls.Add(btnNext);actions.Controls.Add(btnSelect);
        lstRequests.Name="lstRequests";lstRequests.Dock=DockStyle.Fill;lstRequests.IntegralHeight=false;lstRequests.AccessibleName="قائمة طلبات الاعتماد";lstRequests.TabIndex=1;
        lblQueueStatus.Name="lblQueueStatus";lblQueueStatus.Text="لم تُحمّل الطلبات";lblQueueStatus.AutoSize=true;lblQueueStatus.Dock=DockStyle.Fill;
        layout.Controls.Add(actions,0,0);layout.Controls.Add(lstRequests,0,1);layout.Controls.Add(lblQueueStatus,0,2);
        Controls.Add(layout);Name="ApprovalQueuePicker";RightToLeft=RightToLeft.Yes;AutoScaleMode=AutoScaleMode.Dpi;AutoScaleDimensions=new SizeF(96F,96F);Size=new Size(680,190);
        ResumeLayout(false);PerformLayout();
    }
}
