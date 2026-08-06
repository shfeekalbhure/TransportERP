using System.ComponentModel;
using System.Drawing;
using TransportERP.Contracts.AccessControl;
using TransportERP.Desktop.Services;

namespace TransportERP.Desktop.Forms.Security;

/// <summary>SEC-002 — الأدوار؛ تقرأ فقط من خدمة API المعتمدة ولا تنشئ بيانات محلية.</summary>
public sealed class FrmRoles : Form
{
    private readonly AccessControlApiClient? _client;
    private readonly BindingSource _source = new();
    private readonly TextBox _code = RequiredTextBox();
    private readonly TextBox _nameAr = RequiredTextBox();
    private readonly TextBox _nameEn = PlainTextBox();
    private readonly TextBox _description = PlainTextBox();
    private readonly ComboBox _status = Combo("نشط", "موقوف");
    private readonly Label _message = new() { Dock = DockStyle.Bottom, Height = 32, TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.DimGray };

    public FrmRoles() : this(null) { }
    public FrmRoles(AccessControlApiClient? client)
    {
        _client = client; Text = "الأدوار — SEC-002"; Name = nameof(FrmRoles); MinimumSize = new Size(1080, 700); RightToLeft = RightToLeft.Yes; RightToLeftLayout = true; StartPosition = FormStartPosition.CenterParent; BackColor = Color.FromArgb(247,249,252);
        Build(); Shown += async (_, _) => await LoadAsync();
    }

    private void Build()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 5, Padding = new Padding(14) };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.Percent,42)); root.RowStyles.Add(new RowStyle(SizeType.Percent,58)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(new Label { Text = "الأدوار — SEC-002", AutoSize = true, Font = new Font("Tahoma",13,FontStyle.Bold), ForeColor=Color.FromArgb(30,64,116) },0,0);
        root.Controls.Add(Toolbar(),0,1); root.Controls.Add(Tabs(),0,2); root.Controls.Add(Grid(),0,3); root.Controls.Add(_message,0,4); Controls.Add(root);
    }
    private Control Toolbar()
    {
        var bar=new FlowLayoutPanel{Dock=DockStyle.Fill,AutoSize=true,FlowDirection=FlowDirection.RightToLeft};
        foreach(var text in new[]{"جديد","حفظ","تعديل","إيقاف","حذف","طباعة"}) { var b=new Button{Text=text,AutoSize=true,Margin=new Padding(4)}; b.Click+=(_,_)=>_message.Text="مانع التخزين المعتمد: إجراء الأدوار غير متاح حتى اعتماد الخدمة."; bar.Controls.Add(b); } return bar;
    }
    private Control Tabs()
    {
        var tabs=new TabControl{Dock=DockStyle.Fill,RightToLeft=RightToLeft.Yes,RightToLeftLayout=true};
        var main=new TabPage("البيانات الرئيسية"){BackColor=Color.White}; var f=new TableLayoutPanel{Dock=DockStyle.Top,AutoSize=true,ColumnCount=4,Padding=new Padding(12)}; foreach(var x in new[]{new[]{ "كود الدور *","الاسم العربي *"},new[]{"الاسم الإنجليزي","الوصف"},new[]{"الحالة *",""}}){var r=f.RowCount++; Add(f,r,x[0],x[0]=="كود الدور *"?_code:x[0]=="الاسم العربي *"?_nameAr:x[0]=="الاسم الإنجليزي"?_nameEn:x[0]=="الوصف"?_description:_status,0); if(!string.IsNullOrEmpty(x[1]))Add(f,r,x[1],x[1]=="الاسم العربي *"?_nameAr:_status,2);} main.Controls.Add(f); tabs.TabPages.Add(main);
        foreach(var title in new[]{"صلاحيات الشاشات","نطاق البيانات","المستخدمون","التدقيق"}) { var p=new TabPage(title){BackColor=Color.White};p.Controls.Add(new Label{Dock=DockStyle.Top,Height=42,Text="يتطلب هذا التبويب مصدر API معتمد؛ لا توجد بيانات محلية بديلة.",TextAlign=ContentAlignment.MiddleRight,ForeColor=Color.FromArgb(150,70,0)});tabs.TabPages.Add(p);}return tabs;
    }
    private Control Grid(){var g=new DataGridView{Dock=DockStyle.Fill,DataSource=_source,ReadOnly=true,AutoGenerateColumns=false,AllowUserToAddRows=false,RightToLeft=RightToLeft.Yes};foreach(var c in new[]{("Code","الكود"),("NameAr","الاسم العربي"),("Status","الحالة"),("UserCount","عدد المستخدمين"),("UpdatedAt","آخر تعديل")})g.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName=c.Item1,HeaderText=c.Item2,AutoSizeMode=DataGridViewAutoSizeColumnMode.Fill});return g;}
    private async Task LoadAsync(){if(_client is null){_message.Text="مانع التكامل: لم يُحقن عميل API المعتمد.";return;}try{var r=await _client.SearchRolesAsync(new PagedQuery());_source.DataSource=r.Items;_message.Text=r.Blocker??"تم التحميل من API المعتمد.";}catch(Exception e){_message.Text="تعذر الاتصال بخدمة الأدوار: "+e.Message;}}
    private static TextBox RequiredTextBox()=>new(){BackColor=Color.FromArgb(255,253,231),Dock=DockStyle.Fill};private static TextBox PlainTextBox()=>new(){Dock=DockStyle.Fill};private static ComboBox Combo(params string[] x){var c=new ComboBox{DropDownStyle=ComboBoxStyle.DropDownList,Dock=DockStyle.Fill,BackColor=Color.FromArgb(255,253,231)};c.Items.AddRange(x);c.SelectedIndex=0;return c;}private static void Add(TableLayoutPanel p,int r,string label,Control c,int col){p.Controls.Add(new Label{Text=label,AutoSize=true},col,r);p.Controls.Add(c,col+1,r);}
}