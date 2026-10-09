using System.Globalization;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic.FileIO;

namespace TransportERP.EmptyForms;

// UI design keys are not DTO property names. The binding adapter owns identity,
// permission, eligible-state, version and business-rule checks on the server.
public sealed record BatchFiveLookupRequest(string Key, Keys Shortcut);
public sealed record BatchFiveChoice(string Id, string Label);
public sealed record BatchFiveField(Control Editor, bool Required, bool Numeric, bool ReadOnly)
{
    // Opt-in policy from an authoritative setting; null means the value is unavailable.
    public Func<bool?>? RequiredWhen { get; init; }
}
public sealed record BatchFiveRequest(string Action, string? ExpectedVersion,
    IReadOnlyDictionary<string, object?> Fields, IReadOnlyList<IReadOnlyDictionary<string, object?>> Rows,
    IReadOnlyList<int>? SelectedRows = null, string? RequestedItemId = null);
public sealed record BatchFiveDocument(IReadOnlyDictionary<string,object?> Fields,
    IReadOnlyList<IReadOnlyDictionary<string,object?>> Rows, string? Version, bool CanEdit,
    IReadOnlyDictionary<string,string>? ContextText = null);
public sealed record BatchFiveResult(bool Success, bool Persisted, string Message, string? Version = null,
    BatchFiveDocument? Document = null);

public interface IBatchFiveScreen { BatchFiveUiSession Binding { get; } }

public sealed partial class BatchFiveUiSession
{
    private readonly UserControl owner;
    private readonly Label status;
    private readonly ErrorProvider errors;
    private readonly Dictionary<string, BatchFiveField> fields;
    private readonly DataGridView? grid;
    private readonly Dictionary<string, Button> actions;
    private readonly Control workspace;
    private readonly Button clear;
    private readonly Button? add, remove;
    private readonly HashSet<string> editableColumns = new();
    private readonly HashSet<string> displayOnlyColumns;
    private readonly Dictionary<DataGridView, TextBox> importPreviews = new();
    private HashSet<string> allowed = new(StringComparer.Ordinal);
    private HashSet<string> newDraftAllowed = new(StringComparer.Ordinal);
    private Func<BatchFiveRequest, Task<BatchFiveResult>>? execute;
    private bool loading, busy, editable = true;
    public bool HasChanges { get; private set; }
    private BatchFiveDocument? undoDocument;
    public event Action? StateChanged;
    public void NotifyLocalChange() => Changed();
    public void AcceptLocalBaseline()
    {
        var snapshot = Snapshot("Baseline", null);
        undoDocument = new(snapshot.Fields, snapshot.Rows, ExpectedVersion, editable);
        HasChanges = false; StateChanged?.Invoke();
    }
    public bool UndoLocalChanges(Func<bool> confirmDiscard)
    {
        if (busy || !HasPendingChanges || !confirmDiscard()) return false;
        grid?.CancelEdit();
        var document = undoDocument ?? new(new Dictionary<string, object?>(), [], null, true);
        ApplyDocument(document.Fields, document.Rows, document.Version, document.CanEdit, document.ContextText);
        if (document.Version == null) DraftReset?.Invoke();
        AcceptLocalBaseline();
        return true;
    }
    public bool IsBusy => busy;
    // Opt-in from an adapter with create permission; other screens retain their existing clear behavior.
    public bool CanStartNewDraft { get; set; }
    public Func<bool>? AdditionalPendingChanges { get; set; }
    public bool HasPendingChanges => HasChanges || grid?.IsCurrentCellDirty == true || AdditionalPendingChanges?.Invoke() == true;
    public event Action<BatchFiveLookupRequest>? LookupRequested;
    public Func<string,string?>? ValidateCommand { get; set; }
    public event Action? DocumentLoaded;
    public event Action? DraftReset;
    public Task ViewItemAsync(string requestId) => RunAsync("View",requestId);
    public string? ExpectedVersion { get; private set; }

    public BatchFiveUiSession(UserControl owner, Label status, ErrorProvider errors,
        Dictionary<string, BatchFiveField> fields, DataGridView? grid,
        Dictionary<string, Button> actions, Control workspace, Button clear, Button? add, Button? remove,
        IEnumerable<string>? displayOnlyColumnNames = null)
    {
        displayOnlyColumns = new HashSet<string>(displayOnlyColumnNames ?? Array.Empty<string>(), StringComparer.Ordinal);
        this.owner=owner; this.status=status; this.errors=errors; this.fields=fields;
        this.grid=grid; this.actions=actions; this.workspace=workspace;
        this.clear=clear; this.add=add; this.remove=remove;
        foreach (var field in fields.Values)
        {
            var c=field.Editor;
            if(c is ComboBox comboEditor)
                comboEditor.KeyDown += (sender,e) =>
                {
                    if(e.KeyCode is Keys.F7 or Keys.F8 or Keys.F9)
                    {
                        var key=fields.First(p=>ReferenceEquals(p.Value.Editor,comboEditor)).Key;
                        RequestLookup(key,e.KeyCode,comboEditor);e.Handled=true;e.SuppressKeyPress=true;
                    }
                };
            if (!field.ReadOnly)
            {
                if (c is ComboBox cb) cb.SelectedIndexChanged += (_,_) => Changed();
                else if (c is DateTimePicker dt) dt.ValueChanged += (_,_) => Changed();
                else if (c is CheckBox ck) ck.CheckStateChanged += (_,_) => Changed();
                else c.TextChanged += (_,_) => Changed();
            }
        }
        if (grid != null)
        {
            foreach (DataGridViewColumn column in grid.Columns)
                if (!column.ReadOnly) editableColumns.Add(column.Name);
            grid.CellValueChanged += (_,e) => { if(e.RowIndex>=0) Changed(); };
            grid.CurrentCellDirtyStateChanged += (_,_) => { if(grid.IsCurrentCellDirty) grid.CommitEdit(DataGridViewDataErrorContexts.Commit); };
            grid.CellValidating += (sender,e) =>
            {
                if (loading || e.RowIndex<0 || e.ColumnIndex<0) return;
                var col=grid.Columns[e.ColumnIndex];
                string text=Convert.ToString(e.FormattedValue) ?? "";
                if (Equals(col.Tag,"decimal") && !col.ReadOnly && text.Length>0 && !DecimalValue(text, out _))
                { e.Cancel=true; grid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText="أدخل رقمًا عشريًا صحيحًا دون فاصل آلاف"; }
                else if(Equals(col.Tag,"date") && !col.ReadOnly && text.Length>0 && !DateValue(e.FormattedValue))
                { e.Cancel=true; grid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText="اكتب التاريخ بصيغة yyyy-MM-dd"; }
                else grid.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText="";
            };
            grid.KeyDown += (sender,e) =>
            {
                if(e.KeyCode is Keys.F7 or Keys.F8 or Keys.F9 && grid.CurrentCell?.OwningColumn is DataGridViewComboBoxColumn column)
                {
                    if(e.KeyCode==Keys.F9 && column.DataSource!=null)
                    {grid.BeginEdit(true);if(grid.EditingControl is ComboBox combo)combo.DroppedDown=true;}
                    else RequestLookup(column.Name,e.KeyCode,null);
                    e.Handled=true;e.SuppressKeyPress=true;
                }
            };
            grid.DataError += (_,e) => { e.ThrowException=false; e.Cancel=true; status.Text="قيمة غير صالحة في الجدول. راجع الاختيار أو الرقم."; };
        }
        foreach (var pair in actions)
        {
            string action=pair.Key;
            pair.Value.Click += async (_,_) => await RunAsync(action);
        }
        clear.Click += (_,_) => Clear();
        if(add!=null) add.Click += (_,_) => AddRow();
        if(remove!=null) remove.Click += (_,_) => RemoveRow();
        ConfigureKnownRequiredAppearance();
        RefreshButtons();
    }
    private void RequestLookup(string key, Keys shortcut, ComboBox? combo)
    {
        if(busy||!editable)return;
        if(shortcut==Keys.F9&&combo!=null&&combo.Items.Count>0)combo.DroppedDown=true;
        else if(LookupRequested!=null)LookupRequested(new(key,shortcut));
        else status.Text="قائمة الاختيار غير موصولة بعد";
    }
    private static bool DateValue(object? value) => value is DateTime || DateTime.TryParseExact(Convert.ToString(value),
        "yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None,out _);
    private static bool DecimalValue(string value, out decimal result) => decimal.TryParse(value,
        NumberStyles.AllowLeadingSign|NumberStyles.AllowDecimalPoint|NumberStyles.AllowLeadingWhite|NumberStyles.AllowTrailingWhite,
        CultureInfo.CurrentCulture, out result);
    private void Changed()
    {
        if(loading) return;
        HasChanges=true;
        status.Text="تعديلات محلية غير محفوظة";
        StateChanged?.Invoke();
    }
    // The adapter must explicitly authorize Save for a NEW draft; permission to edit an
    // existing document is not permission to create one. State transitions never survive Clear.
    public void BindCommands(Func<BatchFiveRequest,Task<BatchFiveResult>> handler, IEnumerable<string> allowedActions,
        IEnumerable<string>? newDraftActions = null)
    {
        if(busy) throw new InvalidOperationException("An operation is active.");
        ArgumentNullException.ThrowIfNull(handler); ArgumentNullException.ThrowIfNull(allowedActions);
        var permitted = new HashSet<string>(allowedActions,StringComparer.Ordinal);
        var draft = new HashSet<string>(newDraftActions ?? permitted.Where(action=>action is "View" or "Create" or "LoadSource"),StringComparer.Ordinal);
        if(draft.Any(action=>!permitted.Contains(action) || action is not ("View" or "Create" or "Save" or "LoadSource")))
            throw new ArgumentException("New-draft actions must be explicitly authorized view/create/save/source commands.",nameof(newDraftActions));
        execute=handler; allowed=permitted; newDraftAllowed=draft; RefreshButtons();
    }
    public void DisconnectCommands()
    {
        if(busy) throw new InvalidOperationException("An operation is active.");
        execute=null; allowed.Clear(); newDraftAllowed.Clear(); RefreshButtons();
    }
    public void SetEditable(bool value)
    {
        editable=value;
        foreach(var f in fields.Values)
        {
            if(f.Editor is TextBoxBase text) text.ReadOnly=f.ReadOnly||!editable;
            else f.Editor.Enabled=!f.ReadOnly&&editable;
        }
        if(grid!=null) foreach(DataGridViewColumn col in grid.Columns) col.ReadOnly=!editable||!editableColumns.Contains(col.Name);
        RefreshButtons();
    }
    private void RefreshButtons()
    {
        foreach(var pair in actions) pair.Value.Enabled=!busy && execute!=null && allowed.Contains(pair.Key);
        clear.Enabled=!busy&&(editable||CanStartNewDraft); if(add!=null)add.Enabled=!busy&&editable; if(remove!=null)remove.Enabled=!busy&&editable;
        StateChanged?.Invoke();
    }
    public bool ConfirmLeave()
    {
        if(busy) { status.Text="انتظر انتهاء العملية الحالية"; return false; }
        if(grid!=null&&!grid.EndEdit()) return false;
        return !HasPendingChanges||MessageBox.Show(owner,"توجد تعديلات غير محفوظة. هل تريد المتابعة وفقدانها؟","تأكيد",
            MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)==DialogResult.Yes;
    }
    private object? Read(Control c) => c switch
    {
        ComboBox combo => (combo.SelectedItem as BatchFiveChoice)?.Id,
        DateTimePicker date => date.Checked ? date.Value : null,
        CheckBox check => check.CheckState==CheckState.Indeterminate ? null : check.Checked,
        _ => c.Text
    };
    private BatchFiveRequest Snapshot(string action,string? requestedItemId)
    {
        var values=fields.ToDictionary(x=>x.Key,x=>Read(x.Value.Editor));
        var rows=new List<IReadOnlyDictionary<string,object?>>();
        if(grid!=null) foreach(DataGridViewRow row in grid.Rows)
        {
            var valuesRow=new Dictionary<string,object?>();
            foreach(DataGridViewCell cell in row.Cells)
                if(!displayOnlyColumns.Contains(cell.OwningColumn!.Name))
                    valuesRow[cell.OwningColumn.Name]=cell.Value;
            rows.Add(new ReadOnlyDictionary<string,object?>(valuesRow));
        }
        return new(action,ExpectedVersion,new ReadOnlyDictionary<string,object?>(values),rows.AsReadOnly(),grid?.SelectedRows.Cast<DataGridViewRow>().Select(row=>row.Index).OrderBy(index=>index).ToArray(),requestedItemId);
    }
    public bool ValidateEditors()
    {
        if(grid!=null&&!grid.EndEdit()) return false;
        errors.Clear(); bool valid=true;
        foreach(var f in fields.Values.Where(f=>!f.ReadOnly))
        {
            var value=Read(f.Editor);string text=Convert.ToString(value)??"";
            bool? required = f.RequiredWhen == null ? f.Required : f.RequiredWhen();
            string message=required == null && string.IsNullOrWhiteSpace(text)?"تعذر تحديد إلزام الحقل؛ لم يُحمّل إعداد الإلزام":
                required == true &&(value==null||string.IsNullOrWhiteSpace(text))?"حقل مطلوب":
                f.Numeric&&text.Length>0&&!DecimalValue(text,out _)?"أدخل رقمًا عشريًا صحيحًا دون فاصل آلاف":"";
            errors.SetError(f.Editor,message);
            if(message.Length>0) { valid=false; FocusEditor(f.Editor); }
        }
        if(grid!=null) foreach(DataGridViewRow row in grid.Rows) foreach(DataGridViewCell cell in row.Cells)
        {
            var col=cell.OwningColumn!;string text=Convert.ToString(cell.Value)??"";
            if(!col.ReadOnly && Equals(col.HeaderCell.Tag,"required") && string.IsNullOrWhiteSpace(text))
            {cell.ErrorText="حقل مطلوب";valid=false;}
            else if(!col.ReadOnly && Equals(col.Tag,"decimal") && text.Length>0 && !DecimalValue(text,out _))
            {cell.ErrorText="رقم غير صالح";valid=false;}
            else if(!col.ReadOnly && Equals(col.Tag,"date") && text.Length>0 && !DateValue(cell.Value))
            {cell.ErrorText="اكتب التاريخ بصيغة yyyy-MM-dd";valid=false;}
            else cell.ErrorText="";
        }
        if(!valid)status.Text="راجع الحقول المشار إليها قبل إرسال الطلب";
        return valid;
    }
    private void FocusEditor(Control editor)
    {
        for(Control? c=editor;c!=null;c=c.Parent)
            if(c is TabPage page && page.Parent is TabControl tab) tab.SelectedTab=page;
        editor.Focus();
    }
    private async Task RunAsync(string action,string? requestedItemId=null)
    {
        if(busy)return;
        if(execute==null||!allowed.Contains(action)){status.Text="الأمر غير موصول أو غير متاح في الحالة الحالية";return;}
        if(action is "View" or "LoadSource") {if(!ConfirmLeave())return;}
        else if(!ValidateEditors())return;
        string? commandError=ValidateCommand?.Invoke(action);
        if(!string.IsNullOrEmpty(commandError)){status.Text=commandError;return;}
        if(new[]{"Cancel","Post","Reverse","Execute","Approve","Reject","Return","Reopen","Match","Finalize"}.Contains(action)
            && MessageBox.Show(owner,"هل تريد إرسال هذا الإجراء؟ سيعيد النظام التحقق من الصلاحيات والحالة.",
            "تأكيد الإجراء",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)!=DialogResult.Yes)return;
        var request=Snapshot(action,requestedItemId);busy=true;workspace.Enabled=false;RefreshButtons();status.Text="جارٍ إرسال الطلب…";
        try
        {
            var result=await execute(request);
            if(owner.IsDisposed)return;
            status.Text=result.Message;
            if(result.Success && result.Document != null)
            {
                busy=false;
                ApplyDocument(result.Document.Fields,result.Document.Rows,result.Document.Version,result.Document.CanEdit,result.Document.ContextText);
                // A computed/validated draft returned by a command is not proof of persistence.
                if (!result.Persisted && action is not ("View" or "LoadSource")) HasChanges = true;
                status.Text=result.Message;
            }
            else if(result.Success&&result.Persisted) {HasChanges=false;ExpectedVersion=result.Version;SetEditable(false);}
        }
        catch(Exception) { if(!owner.IsDisposed)status.Text="تعذر إكمال الطلب. بقيت التعديلات المحلية دون تغيير؛ تحقق من الحالة قبل إعادة الإرسال."; }
        finally {busy=false;if(!owner.IsDisposed){workspace.Enabled=true;RefreshButtons();}}
    }
    public void SetChoices(string key,IEnumerable<BatchFiveChoice> choices)
    {
        var data=choices.ToArray();if(data.Any(x=>string.IsNullOrWhiteSpace(x.Id))||data.Select(x=>x.Id).Distinct().Count()!=data.Length)
            throw new ArgumentException("Lookup IDs must be nonempty and unique.");
        ComboBox? combo=fields.TryGetValue(key,out var f)?f.Editor as ComboBox:null;
        var col=grid?.Columns[key] as DataGridViewComboBoxColumn;
        if(combo==null && col==null) throw new ArgumentException("Unknown lookup design key.",nameof(key));
        var old=(combo?.SelectedItem as BatchFiveChoice)?.Id;
        if(old!=null&&!data.Any(c=>c.Id==old))throw new InvalidOperationException("Current choice is absent; reload explicitly.");
        if(col!=null && grid!=null) foreach(DataGridViewRow row in grid.Rows)
            if(row.Cells[key].Value is string id&&!data.Any(c=>c.Id==id))throw new InvalidOperationException("A row choice is absent.");
        loading=true;
        try
        {
            if(combo!=null)
            {
                combo.DisplayMember=nameof(BatchFiveChoice.Label);combo.ValueMember=nameof(BatchFiveChoice.Id);
                combo.DataSource=data;combo.SelectedIndex=-1;if(old!=null)combo.SelectedValue=old;
            }
            if(col!=null)
            {col.DisplayMember=nameof(BatchFiveChoice.Label);col.ValueMember=nameof(BatchFiveChoice.Id);col.DataSource=data;}
        }
        finally {loading=false;}
    }
    // Invoke on the UI thread after resolving provider IDs. Returns false when the user keeps a dirty draft.
    public bool LoadDocument(IReadOnlyDictionary<string,object?> values, IEnumerable<IReadOnlyDictionary<string,object?>> rows,
        string? expectedVersion, bool canEdit, IReadOnlyDictionary<string,string>? contextText=null)
    {
        if(!ConfirmLeave())return false;
        return ApplyDocument(values,rows,expectedVersion,canEdit,contextText);
    }
    private bool ApplyDocument(IReadOnlyDictionary<string,object?> values, IEnumerable<IReadOnlyDictionary<string,object?>> rows,
        string? expectedVersion,bool canEdit,IReadOnlyDictionary<string,string>? contextText)
    {
        var preparedRows=rows.ToArray();
        // Reject incomplete lookup/date payloads before touching the existing draft.
        foreach(var p in fields)
        {
            values.TryGetValue(p.Key,out var v);
            if(v==null)continue;
            if(p.Value.Editor is ComboBox cb && !(v is string id && cb.Items.OfType<BatchFiveChoice>().Any(c=>c.Id==id)))
                throw new ArgumentException("Unknown lookup identity: "+p.Key);
            if(p.Value.Editor is DateTimePicker dt && !(v is DateTime date && date>=dt.MinDate && date<=dt.MaxDate))
                throw new ArgumentException("Invalid date: "+p.Key);
            if(p.Value.Editor is CheckBox && v is not bool)throw new ArgumentException("Invalid boolean: "+p.Key);
        }
        if(grid!=null)foreach(var row in preparedRows)foreach(var pair in row)
        {
            if(!grid.Columns.Contains(pair.Key))throw new ArgumentException("Unknown column: "+pair.Key);
            var col=grid.Columns[pair.Key];
            if(pair.Value==null)continue;
            if(col is DataGridViewComboBoxColumn cb && !(pair.Value is string id && cb.DataSource is BatchFiveChoice[] list && list.Any(x=>x.Id==id)))
                throw new ArgumentException("Unknown row lookup: "+pair.Key);
            if(col is DataGridViewCheckBoxColumn && pair.Value is not bool)throw new ArgumentException("Invalid row boolean: "+pair.Key);
        }
        loading=true;
        try
        {
            foreach(var p in fields)
            {
                values.TryGetValue(p.Key,out var value);
                switch(p.Value.Editor)
                {
                    case ComboBox combo: combo.SelectedIndex=-1;if(value!=null)combo.SelectedValue=value;break;
                    case DateTimePicker date:date.Checked=value is DateTime;if(value is DateTime d)date.Value=d;break;
                    case CheckBox check:check.CheckState=value is bool b?(b?CheckState.Checked:CheckState.Unchecked):CheckState.Indeterminate;break;
                    default:p.Value.Editor.Text=Convert.ToString(value,CultureInfo.CurrentCulture)??"";break;
                }
            }
            if(grid!=null)
            {
                grid.Rows.Clear();
                foreach(var row in preparedRows)
                {
                    int index=grid.Rows.Add();
                    foreach(DataGridViewColumn col in grid.Columns) if(row.TryGetValue(col.Name,out var value))grid.Rows[index].Cells[col.Name].Value=value;
                }
                Renumber();
            }
            foreach(Control panel in Descendants(owner))
                if(panel is RichTextBox view && view.ReadOnly) view.Text=contextText!=null&&contextText.TryGetValue(view.Name,out var content)?content:"";
            ResetImportPreviews();
            ExpectedVersion=expectedVersion;HasChanges=false;errors.Clear();SetEditable(canEdit);status.Text="تم تحميل بيانات العرض";DocumentLoaded?.Invoke();
            var snapshot=Snapshot("Baseline",null); undoDocument=new(snapshot.Fields,snapshot.Rows,ExpectedVersion,canEdit,contextText); StateChanged?.Invoke();return true;
        }
        finally {loading=false;}
    }
    private static IEnumerable<Control> Descendants(Control root)
    {foreach(Control c in root.Controls){yield return c;foreach(var child in Descendants(c))yield return child;}}
    private void Clear()
    {
        if((!editable&&!CanStartNewDraft)||!ConfirmLeave())return;
        ApplyDocument(new Dictionary<string,object?>(),Array.Empty<IReadOnlyDictionary<string,object?>>(),null,true,null);
        allowed = new HashSet<string>(newDraftAllowed, StringComparer.Ordinal);
        RefreshButtons();
        status.Text="مسودة واجهة فارغة — لم يتم حفظها";
        DraftReset?.Invoke();
    }
    private void AddRow()
    {
        if(busy||!editable||grid==null||!grid.EndEdit())return;
        int index=grid.Rows.Add();Renumber();grid.CurrentCell=grid.Rows[index].Cells.Cast<DataGridViewCell>().FirstOrDefault(c=>!c.ReadOnly);Changed();
    }
    private void RemoveRow()
    {
        if(busy||!editable||grid?.CurrentRow==null||!grid.EndEdit())return;
        if(MessageBox.Show(owner,"حذف السطر المحدد من المسودة؟","حذف سطر",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)!=DialogResult.Yes)return;
        grid.Rows.Remove(grid.CurrentRow);Renumber();Changed();
    }
    private void Renumber()
    {if(grid?.Columns.Contains("rowNo")==true)for(int i=0;i<grid.Rows.Count;i++)grid.Rows[i].Cells["rowNo"].Value=i+1;}
    private void ResetImportPreviews()
    {
        foreach (var pair in importPreviews)
        {
            if (!pair.Key.IsDisposed) { pair.Key.Rows.Clear(); pair.Key.Tag = null; }
            if (!pair.Value.IsDisposed) pair.Value.Clear();
        }
    }
    public void ChooseCsv(TextBox path,DataGridView preview)
    {
        if(!editable||busy||grid==null)return;
        importPreviews[preview] = path;
        using var dialog=new OpenFileDialog {Filter="CSV UTF-8 (*.csv)|*.csv",CheckFileExists=true,Multiselect=false};
        if(dialog.ShowDialog(owner)!=DialogResult.OK)return;
        ReadCsv(dialog.FileName, path, preview);
    }
    public void ReadCsv(string fileName, TextBox path, DataGridView preview)
    {
        if(!editable||busy||grid==null)return;
        importPreviews[preview] = path;
        try
        {
            if(new System.IO.FileInfo(fileName).Length>5*1024*1024)throw new InvalidOperationException("الحد الأقصى للملف 5 ميجابايت");
            using var parser=new TextFieldParser(fileName,System.Text.Encoding.UTF8){TextFieldType=FieldType.Delimited,HasFieldsEnclosedInQuotes=true};
            parser.SetDelimiters(",");
            var headers=parser.ReadFields()??Array.Empty<string>();
            var expected=grid!.Columns.Cast<DataGridViewColumn>().Where(c=>editableColumns.Contains(c.Name)).Select(c=>c.Name).ToArray();
            if(!headers.SequenceEqual(expected))throw new InvalidOperationException("عناوين CSV لا تطابق الترتيب المبين في الشاشة");
            var values=new List<string[]>();
            while(!parser.EndOfData)
            {var row=parser.ReadFields()!;if(row.Length!=headers.Length||values.Count>=1000)throw new InvalidOperationException("عدد الأعمدة غير صحيح أو تجاوز الملف 1000 سطر");values.Add(row);}
            preview.Rows.Clear();preview.Columns.Clear();
            foreach(string header in headers)preview.Columns.Add(header,grid.Columns[header]!.HeaderText);
            foreach(var row in values)preview.Rows.Add(row.Cast<object>().ToArray());
            path.Text=fileName;preview.Tag=headers;Changed();status.Text="تمت المعاينة المحلية فقط؛ لم تُضف أو تُحفظ أي بيانات";
        }
        catch(Exception e) when(e is System.IO.IOException or UnauthorizedAccessException or MalformedLineException or InvalidOperationException)
        {preview.Rows.Clear();preview.Tag=null;path.Clear();status.Text="تعذر قراءة CSV: "+e.Message;}
    }
    public void ImportPreview(DataGridView preview) => ImportPreview(preview, () => MessageBox.Show(owner,"إضافة أسطر المعاينة إلى المسودة المحلية؟","استيراد محلي",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)==DialogResult.Yes);
    public void ImportPreview(DataGridView preview, Func<bool> confirm)
    {
        if(!editable||busy||grid==null||preview.Tag is not string[] headers||preview.Rows.Count==0||!grid.EndEdit())return;
        var converted=new List<object?[]>();
        foreach(DataGridViewRow row in preview.Rows)
        {
            var values=new object?[grid.Columns.Count];
            foreach(string key in headers)
            {
                var col=grid.Columns[key]!;string value=Convert.ToString(row.Cells[key].Value)??"";
                if(Equals(col.Tag,"decimal")&&value.Length>0&&!DecimalValue(value,out _)) {status.Text=$"السطر {row.Index + 1}، العمود {col.HeaderText}: رقم غير صحيح في ملف الاستيراد";return;}
                if(Equals(col.Tag,"date")&&value.Length>0&&!DateValue(value)) {status.Text="تاريخ غير صحيح في ملف الاستيراد؛ استخدم yyyy-MM-dd";return;}
                if(col is DataGridViewCheckBoxColumn)
                {
                    if(value.Length==0) values[col.Index]=null;
                    else if(bool.TryParse(value,out var flag)) values[col.Index]=flag;
                    else {status.Text="قيمة منطقية غير صحيحة في ملف الاستيراد؛ استخدم True أو False";return;}
                    continue;
                }
                if(col is DataGridViewComboBoxColumn combo && value.Length>0 && !(combo.DataSource is BatchFiveChoice[] choices&&choices.Any(c=>c.Id==value)))
                {status.Text="رمز غير موجود في القائمة المرتبطة: "+col.HeaderText;return;}
                values[col.Index]=value.Length==0?null:value;
            }
            converted.Add(values);
        }
        if(!confirm())return;
        foreach(var row in converted)grid.Rows.Add(row.Select(value=>value!).ToArray());Renumber();Changed();preview.Rows.Clear();preview.Tag=null;
    }
}
