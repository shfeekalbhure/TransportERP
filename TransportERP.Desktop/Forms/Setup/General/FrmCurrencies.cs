using System.ComponentModel;
using System.Drawing;
using TransportERP.Contracts.Setup.Currencies;
using TransportERP.Desktop.Services;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-009 — العملات. جميع عمليات البيانات تمر عبر API ولا توجد بيانات محلية بديلة.</summary>
public sealed class FrmCurrencies : Form
{
    private readonly ICurrenciesApiClient _client;
    private readonly TextBox _code = new(), _arabic = new(), _english = new(), _iso = new(), _symbol = new(), _notes = new() { Multiline = true, Height = 56 };
    private readonly NumericUpDown _decimals = new() { Minimum = 0, Maximum = 6, Height = 30 };
    private readonly CheckBox _local = new() { Text = "عملة محلية", AutoSize = true };
    private readonly ComboBox _status = Combo("نشط", "موقوف"), _filter = Combo("الكل", "نشط", "موقوف");
    private readonly TextBox _search = new();
    private readonly DataGridView _grid = new();
    private readonly Label _state = new() { AutoSize = true }, _audit = new() { AutoSize = true };
    private CurrencyDto? _selected;
    private int _page = 1;

    public FrmCurrencies(ICurrenciesApiClient? client = null)
    {
        _client = client ?? CurrenciesApiClient.CreateDefault();
        Text = "GEN-009 — العملات"; RightToLeft = RightToLeft.Yes; RightToLeftLayout = true; Font = new Font("Segoe UI", 10F); BackColor = Color.FromArgb(247,249,252); Dock = DockStyle.Fill; TopLevel = false; FormBorderStyle = FormBorderStyle.None;
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6, Padding = new Padding(16), BackColor = BackColor };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.Percent,35)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.Percent,65)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(Toolbar(),0,0); root.Controls.Add(Tabs(),0,1); root.Controls.Add(Search(),0,2); root.Controls.Add(Grid(),0,3); root.Controls.Add(Pager(),0,4); root.Controls.Add(_audit,0,5); Controls.Add(root);
        Shown += async (_,_) => await LoadAsync();
    }

    private Control Toolbar()
    {
        var p=Bar(); Button(p,"جديد",(_,_)=>NewRecord(),Color.FromArgb(47,128,237)); Button(p,"حفظ",async(_,_)=>await SaveAsync(),Color.FromArgb(39,174,96)); Button(p,"تعديل",async(_,_)=>await SaveAsync(),Color.FromArgb(242,153,74)); Button(p,"إيقاف",async(_,_)=>await SuspendAsync(),Color.Gray); Button(p,"حذف",async(_,_)=>await DeleteAsync(),Color.FromArgb(235,87,87)); Button(p,"طباعة",(_,_)=>MessageBox.Show("الطباعة تمر عبر خدمة التقارير المعتمدة.",Text),Color.FromArgb(155,81,224)); return p;
    }
    private Control Tabs()
    {
        var t=new TabControl { Dock=DockStyle.Fill,RightToLeft=RightToLeft.Yes,RightToLeftLayout=true}; var page=new TabPage("البيانات الرئيسية"){BackColor=BackColor};
        var f=new TableLayoutPanel{Dock=DockStyle.Fill,ColumnCount=4,RowCount=5,RightToLeft=RightToLeft.Yes}; f.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,14));f.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,36));f.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,14));f.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,36));
        Field(f,0,"رمز العملة *",_code,true);Field(f,1,"الاسم العربي *",_arabic,true);Field(f,2,"الاسم الإنجليزي",_english,false);Field(f,3,"رمز ISO *",_iso,true);Field(f,4,"الرمز المختصر",_symbol,false);Field(f,5,"عدد المنازل العشرية *",_decimals,true);Field(f,6,"عملة محلية",_local,false);Field(f,7,"الحالة *",_status,true);Field(f,8,"ملاحظات",_notes,false);
        page.Controls.Add(f);t.TabPages.Add(page);return t;
    }
    private Control Search(){var p=new FlowLayoutPanel{Dock=DockStyle.Fill,AutoSize=true,FlowDirection=FlowDirection.RightToLeft,Padding=new Padding(0,8,0,8)};p.Controls.Add(new Label{Text="البحث والتصفية:",AutoSize=true,Padding=new Padding(3,8,3,0)});_search.Width=250;_search.TextChanged+=async(_,_)=>{_page=1;await LoadAsync();};p.Controls.Add(_search);_filter.SelectedIndexChanged+=async(_,_)=>{_page=1;await LoadAsync();};p.Controls.Add(_filter);var clear=new Button{Text="مسح التصفية"};clear.Click+=(_,_)=>{_search.Clear();_filter.SelectedIndex=0;};p.Controls.Add(clear);p.Controls.Add(_state);return p;}
    private Control Grid(){_grid.Dock=DockStyle.Fill;_grid.ReadOnly=true;_grid.AllowUserToAddRows=false;_grid.AutoGenerateColumns=false;_grid.BackgroundColor=Color.White;_grid.SelectionMode=DataGridViewSelectionMode.FullRowSelect;AddCol(nameof(CurrencyDto.Code),"الرمز",90);AddCol(nameof(CurrencyDto.ArabicName),"الاسم العربي",180);AddCol(nameof(CurrencyDto.IsoCode),"ISO",80);AddCol(nameof(CurrencyDto.IsLocal),"المحلية",70);AddCol(nameof(CurrencyDto.DecimalPlaces),"المنازل",70);AddCol(nameof(CurrencyDto.Status),"الحالة",90);_grid.CellClick+=(_,_)=>SelectRow();return _grid;}
    private Control Pager(){var p=new FlowLayoutPanel{Dock=DockStyle.Fill,AutoSize=true,FlowDirection=FlowDirection.RightToLeft};Button(p,"الأول",async(_,_)=>{_page=1;await LoadAsync();},Color.Gray);Button(p,"السابق",async(_,_)=>{_page=Math.Max(1,_page-1);await LoadAsync();},Color.Gray);Button(p,"التالي",async(_,_)=>{_page++;await LoadAsync();},Color.Gray);return p;}
    private async Task LoadAsync(){try{var status=_filter.Text=="نشط"?CurrencyStatus.Active:_filter.Text=="موقوف"?CurrencyStatus.Suspended:(CurrencyStatus?)null;var r=await _client.SearchAsync(new CurrencySearchRequest(_search.Text,status,_page),CancellationToken.None);_grid.DataSource=new BindingList<CurrencyDto>(r.Items.ToList());_state.Text=r.StorageAvailable?$"عدد السجلات: {r.TotalCount}":r.Message??"مانع التخزين المعتمد";if(!r.StorageAvailable)_audit.Text=$"الحالة: {r.BlockerCode} — لا توجد بيانات بديلة.";}catch(HttpRequestException){_grid.DataSource=new BindingList<CurrencyDto>();_state.Text="تعذر الاتصال بخدمة العملات.";} }
    private async Task SaveAsync(){if(string.IsNullOrWhiteSpace(_code.Text)||string.IsNullOrWhiteSpace(_arabic.Text)||string.IsNullOrWhiteSpace(_iso.Text)){MessageBox.Show("رمز العملة والاسم العربي ورمز ISO حقول إلزامية.",Text);return;}var r=_selected is null?await _client.CreateAsync(new(_code.Text.Trim(),_arabic.Text.Trim(),Null(_english),_iso.Text.Trim(),Null(_symbol),(int)_decimals.Value,_local.Checked,Status(),Null(_notes)),CancellationToken.None):await _client.UpdateAsync(_selected.Id,new(_arabic.Text.Trim(),Null(_english),_iso.Text.Trim(),Null(_symbol),(int)_decimals.Value,_local.Checked,Status(),Null(_notes)),CancellationToken.None);Show(r);if(r.Succeeded)await LoadAsync();}
    private async Task SuspendAsync(){if(_selected is null){MessageBox.Show("اختر سجلًا أولًا.",Text);return;}Show(await _client.SuspendAsync(_selected.Id,CancellationToken.None));await LoadAsync();}
    private async Task DeleteAsync(){if(_selected is null){MessageBox.Show("اختر سجلًا أولًا.",Text);return;}if(MessageBox.Show("هل تريد حذف السجل؟",Text,MessageBoxButtons.YesNo)!=DialogResult.Yes)return;Show(await _client.DeleteAsync(_selected.Id,CancellationToken.None));await LoadAsync();}
    private void NewRecord(){_selected=null;_code.Clear();_arabic.Clear();_english.Clear();_iso.Clear();_symbol.Clear();_decimals.Value=0;_local.Checked=false;_status.SelectedIndex=0;_notes.Clear();_audit.Text="سجل جديد";}
    private void SelectRow(){if(_grid.CurrentRow?.DataBoundItem is not CurrencyDto x)return;_selected=x;_code.Text=x.Code;_arabic.Text=x.ArabicName;_english.Text=x.EnglishName??"";_iso.Text=x.IsoCode;_symbol.Text=x.Symbol??"";_decimals.Value=x.DecimalPlaces;_local.Checked=x.IsLocal;_status.Text=x.Status==CurrencyStatus.Active?"نشط":"موقوف";_notes.Text=x.Notes??"";_audit.Text=$"أنشئ بواسطة: {x.CreatedBy} في {x.CreatedAt:yyyy/MM/dd HH:mm} | آخر تعديل: {x.ModifiedBy??"—"} | تعديل: {x.EditCount} | طباعة: {x.PrintCount}";}
    private CurrencyStatus Status()=>_status.Text=="موقوف"?CurrencyStatus.Suspended:CurrencyStatus.Active; private static string? Null(TextBox x)=>string.IsNullOrWhiteSpace(x.Text)?null:x.Text.Trim(); private void Show(CurrencyCommandResponse r)=>MessageBox.Show(r.Message??(r.Succeeded?"تم الحفظ.":"تعذر التنفيذ."),Text);
    private void AddCol(string n,string h,int w)=>_grid.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName=n,HeaderText=h,Width=w});
    private static FlowLayoutPanel Bar()=>new(){Dock=DockStyle.Fill,AutoSize=true,WrapContents=false,FlowDirection=FlowDirection.RightToLeft,Padding=new Padding(8),BackColor=Color.FromArgb(28,80,130)};
    private static void Button(FlowLayoutPanel p,string t,EventHandler h,Color c){var b=new Button{Text=t,AutoSize=true,Height=36,Margin=new Padding(4),BackColor=c,ForeColor=Color.White,FlatStyle=FlatStyle.Flat};b.FlatAppearance.BorderSize=0;b.Click+=h;p.Controls.Add(b);}
    private static ComboBox Combo(params string[] a){var c=new ComboBox{DropDownStyle=ComboBoxStyle.DropDownList,Height=30};c.Items.AddRange(a);return c;}
    private static void Field(TableLayoutPanel p,int i,string l,Control x,bool req){var r=i/2;var c=(i%2)*2;p.Controls.Add(new Label{Text=l,Dock=DockStyle.Fill,TextAlign=ContentAlignment.MiddleRight},c,r);x.Dock=DockStyle.Fill;x.Margin=new Padding(6,4,16,4);if(req)x.BackColor=Color.FromArgb(255,252,220);p.Controls.Add(x,c+1,r);}
}
