namespace TransportERP.EmptyForms;

// Shared queue picker: presentation labels and opaque identities come from the adapter.
// It deliberately declares no accounting business columns.
public partial class ApprovalQueuePicker : UserControl
{
    public event Action<string>? RequestSelected;
    public event Action<string,int>? PageRequested; 
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(
    System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public Func<bool>? ConfirmNavigation { get; set; }

    private bool previousPage, nextPage;
    public ApprovalQueuePicker()
    {
        InitializeComponent();
        btnSelect.Click += (_,_) => SelectRequest();
        btnSearch.Click += (_,_) => RequestPage(0);
        btnPrevious.Click += (_,_) => {if(previousPage)RequestPage(-1);};
        btnNext.Click += (_,_) => {if(nextPage)RequestPage(1);};
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
    public void LoadPage(IEnumerable<BatchFiveChoice> requests,bool hasPrevious,bool hasNext)
    {
        var choices=requests.ToArray();
        if(choices.Any(c=>string.IsNullOrWhiteSpace(c.Id))||choices.Select(c=>c.Id).Distinct().Count()!=choices.Length)
            throw new ArgumentException("Request IDs must be unique and nonempty.");
        lstRequests.DisplayMember=nameof(BatchFiveChoice.Label);lstRequests.ValueMember=nameof(BatchFiveChoice.Id);
        lstRequests.DataSource=choices;lstRequests.SelectedIndex=-1;
        previousPage=hasPrevious;nextPage=hasNext;btnPrevious.Enabled=hasPrevious;btnNext.Enabled=hasNext;
        lblQueueStatus.Text=choices.Length==0?"لا توجد طلبات في الصفحة المحملة":"اختر طلبًا ثم اضغط عرض الطلب";
    }
    private bool CanNavigate()=>ConfirmNavigation?.Invoke()??true;
    private void SelectRequest()
    {
        if(lstRequests.SelectedItem is not BatchFiveChoice selected)return;
        if(RequestSelected==null){lblQueueStatus.Text="عرض الطلب غير موصول بعد";return;}
        if(CanNavigate())RequestSelected(selected.Id);
    }
    private void RequestPage(int direction)
    {
        if(PageRequested==null){lblQueueStatus.Text="بحث الطلبات غير موصول بعد";return;}
        if(CanNavigate())PageRequested(txtSearch.Text.Trim(),direction);
    }
}
