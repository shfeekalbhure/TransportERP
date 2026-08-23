namespace TransportERP.Desktop.CoreUI;

/// <summary>DEC-014 shared MainData host only; individual fields belong to future governed screen contracts.</summary>
public sealed class TransportDataEntryPanel : TableLayoutPanel
{
    private int _columnCount = 2;

    public TransportDataEntryPanel()
    {
        Name = "MainData";
        Dock = DockStyle.Fill;
        RightToLeft = RightToLeft.Yes;
        AutoScroll = true;
        ColumnCount = 2;
    }

    public new int ColumnCount
    {
        get => _columnCount;
        set
        {
            if (value is < 1 or > 2) throw new ArgumentOutOfRangeException(nameof(value), "DEC-014 permits one or two columns only.");
            _columnCount = value;
            base.ColumnCount = value;
        }
    }

    public void AddField(Control field, int fieldSpan = 1)
    {
        if (fieldSpan is < 1 or > 2 || fieldSpan > ColumnCount)
            throw new ArgumentOutOfRangeException(nameof(fieldSpan), "Field span exceeds the governing column limit.");
        Controls.Add(field, 0, RowCount++);
        SetColumnSpan(field, fieldSpan);
    }
}
