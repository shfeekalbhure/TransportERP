namespace TransportERP.EmptyForms;

// Native radio editor for the pictured per-account allocation choices. Values are UI
// choices only; this cell neither allocates money nor calculates inventory cost.
public sealed class AllocationRadioCell : DataGridViewTextBoxCell
{
    public override Type EditType => typeof(AllocationRadioEditor);
    public override object DefaultNewRowValue => string.Empty;
    internal static string[] Choices(string? column) => column=="colAccountBasis"
        ? new[]{"نسبة","نسبة من الصنف","المبلغ"} : new[]{"توزيع آلي","قيمة ثابتة"};
    public override void InitializeEditingControl(int rowIndex,object? initialFormattedValue,DataGridViewCellStyle dataGridViewCellStyle)
    {
        base.InitializeEditingControl(rowIndex,initialFormattedValue,dataGridViewCellStyle);
        var editor=(AllocationRadioEditor)DataGridView!.EditingControl!;
        editor.AccessibleName=OwningColumn?.HeaderText;
        editor.SetChoices(Choices(OwningColumn?.Name),Convert.ToString(Value)??string.Empty);
    }
    protected override void Paint(Graphics graphics,Rectangle clipBounds,Rectangle cellBounds,int rowIndex,DataGridViewElementStates cellState,object? value,object? formattedValue,string? errorText,DataGridViewCellStyle cellStyle,DataGridViewAdvancedBorderStyle advancedBorderStyle,DataGridViewPaintParts paintParts)
    {
        base.Paint(graphics,clipBounds,cellBounds,rowIndex,cellState,value,string.Empty,errorText,cellStyle,advancedBorderStyle,paintParts&~DataGridViewPaintParts.ContentForeground);
        if(rowIndex<0)return;
        int x=cellBounds.Right-6;
        foreach(var choice in Choices(OwningColumn?.Name))
        {
            var radio=new Rectangle(x-14,cellBounds.Top+(cellBounds.Height-14)/2,14,14);
            ControlPaint.DrawRadioButton(graphics,radio,string.Equals(Convert.ToString(value),choice,StringComparison.Ordinal)?ButtonState.Checked:ButtonState.Normal);
            int width=TextRenderer.MeasureText(graphics,choice,cellStyle.Font,Size.Empty,TextFormatFlags.NoPadding).Width;
            var label=new Rectangle(radio.Left-width-4,cellBounds.Top,width,cellBounds.Height);
            TextRenderer.DrawText(graphics,choice,cellStyle.Font,label,(cellState&DataGridViewElementStates.Selected)!=0?cellStyle.SelectionForeColor:cellStyle.ForeColor,TextFormatFlags.RightToLeft|TextFormatFlags.Right|TextFormatFlags.VerticalCenter|TextFormatFlags.NoPadding);
            x=label.Left-12;
        }
    }
}

public sealed class AllocationRadioEditor : FlowLayoutPanel,IDataGridViewEditingControl
{
    bool loading;
    string selected=string.Empty;
    public AllocationRadioEditor()
    {
        FlowDirection=FlowDirection.LeftToRight;RightToLeft=RightToLeft.Yes;
        WrapContents=false;Margin=Padding.Empty;Padding=Padding.Empty;TabStop=false;
    }
    public void SetChoices(IEnumerable<string> choices,string value)
    {
        loading=true;
        foreach(Control child in Controls.Cast<Control>().ToArray())child.Dispose();
        foreach(var choice in choices)
        {
            var button=new RadioButton{Text=choice,AccessibleName=choice,AutoSize=true,Margin=new Padding(3,2,3,0),Checked=choice==value,TabStop=choice==value};
            button.CheckedChanged+=(_,_)=>
            {
                if(loading||!button.Checked)return;
                selected=button.Text;EditingControlValueChanged=true;
                foreach(var peer in Controls.OfType<RadioButton>())peer.TabStop=peer==button;
                EditingControlDataGridView?.NotifyCurrentCellDirty(true);
            };
            button.PreviewKeyDown+=(_,e)=> { if(e.KeyCode is Keys.Left or Keys.Right or Keys.Up or Keys.Down)e.IsInputKey=true; };
            button.KeyDown+=(_,e)=>
            {
                if(e.KeyCode is not (Keys.Left or Keys.Right or Keys.Up or Keys.Down or Keys.Space))return;
                var peers=Controls.OfType<RadioButton>().ToArray();int index=Array.IndexOf(peers,button);
                if(e.KeyCode!=Keys.Space)index=(index+(e.KeyCode is Keys.Left or Keys.Down?1:-1)+peers.Length)%peers.Length;
                peers[index].Checked=true;peers[index].Focus();e.Handled=true;e.SuppressKeyPress=true;
            };
            Controls.Add(button);
        }
        selected=value;loading=false;EditingControlValueChanged=false;
        if(!Controls.OfType<RadioButton>().Any(b=>b.Checked)&&Controls.Count>0)Controls[0].TabStop=true;
    }
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    [System.Diagnostics.CodeAnalysis.AllowNull]
    public object EditingControlFormattedValue
    {
        get=>selected;
        set
        {
            selected=Convert.ToString(value)??string.Empty;loading=true;
            foreach(var radio in Controls.OfType<RadioButton>())radio.Checked=radio.Text==selected;
            loading=false;
        }
    }
    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)=>selected;
    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle style){Font=style.Font;ForeColor=style.ForeColor;BackColor=style.BackColor;}
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int EditingControlRowIndex {get;set;}
    public bool EditingControlWantsInputKey(Keys keyData,bool dataGridViewWantsInputKey)=>
        (keyData&Keys.KeyCode) is Keys.Left or Keys.Right or Keys.Up or Keys.Down or Keys.Space;
    public void PrepareEditingControlForEdit(bool selectAll)
    {
        (Controls.OfType<RadioButton>().FirstOrDefault(b=>b.Checked)??Controls.OfType<RadioButton>().FirstOrDefault())?.Focus();
    }
    public bool RepositionEditingControlOnValueChange=>false;
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public DataGridView? EditingControlDataGridView {get;set;}
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool EditingControlValueChanged {get;set;}
    public Cursor EditingPanelCursor=>Cursors.Default;
}
