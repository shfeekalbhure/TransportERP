using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Setup.Security;

/// <summary>Screen 02.03.06. Three fields and two actions approved by the owner; design only.</summary>
public sealed class FrmChangePassword : FrmBase
{
    public FrmChangePassword()
    {
        SuspendLayout();
        Name = nameof(FrmChangePassword);
        Text = "تغيير كلمة السر";
        Tag = "02.03.06";
        ClientSize = new Size(640, 420); // Existing FrmDialogTemplate client size.
        MinimumSize = SizeFromClientSize(ClientSize);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = MinimizeBox = false;
        var layout = new TableLayoutPanel { Name = "passwordLayout", Dock = DockStyle.Fill, Padding = UiDesignTokens.FormPadding, ColumnCount = 1, RowCount = 4 };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var title = new Label { Name = "lblTitle", Text = Text, AutoSize = true, Dock = DockStyle.Fill, Margin = new Padding(8, 8, 8, 16) };
        SettingsFormStyle.ApplyLabelStyle(title);
        title.Font = UiDesignTokens.SectionTitleFont();
        title.MinimumSize = TextRenderer.MeasureText(title.Text, title.Font);
        layout.Controls.Add(title, 0, 0);
        var fields = new TableLayoutPanel { Name = "passwordFields", AutoSize = true, Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3, RightToLeft = RightToLeft.Yes };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        var definitions = new[] { ("CurrentPassword", "كلمة السر الحالية"), ("NewPassword", "كلمة السر الجديدة"), ("ConfirmPassword", "تأكيد كلمة السر الجديدة") };
        for (var i = 0; i < definitions.Length; i++)
        {
            fields.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var label = new Label { Name = "lbl" + definitions[i].Item1, Text = definitions[i].Item2, AutoSize = true, Anchor = AnchorStyles.Right, Margin = new Padding(4, 8, 16, 8) };
            SettingsFormStyle.ApplyLabelStyle(label);
            label.MinimumSize = TextRenderer.MeasureText(label.Text, label.Font);
            var input = new TextBox { Name = definitions[i].Item1, AccessibleName = definitions[i].Item2, UseSystemPasswordChar = true, Anchor = AnchorStyles.Left | AnchorStyles.Right, Margin = new Padding(8), TabIndex = i };
            SettingsFormStyle.ApplyInputStyle(input);
            fields.Controls.Add(label, 0, i);
            fields.Controls.Add(input, 1, i);
        }
        layout.Controls.Add(fields, 0, 1);
        var actions = new FlowLayoutPanel { Name = "passwordActions", AutoSize = true, Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, WrapContents = true };
        var change = new Button { Name = "btnChangePassword", Text = "تغيير كلمة السر", AutoSize = true, Padding = new Padding(16, 4, 16, 4), Margin = new Padding(4) };
        var close = new Button { Name = "btnClose", Text = "إغلاق", AutoSize = true, Padding = new Padding(16, 4, 16, 4), Margin = new Padding(4) };
        SettingsFormStyle.ApplyPrimaryButtonStyle(change);
        SettingsFormStyle.ApplySecondaryButtonStyle(close);
        foreach (var button in new[] { change, close })
        {
            button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button.MinimumSize = new Size(TextRenderer.MeasureText(button.Text, button.Font).Width + button.Padding.Horizontal, Math.Max(40, button.GetPreferredSize(Size.Empty).Height));
            actions.Controls.Add(button);
        }
        close.Click += (_, _) => Close();
        CancelButton = close;
        // The change action has no authentication/persistence implementation in this design task.
        layout.Controls.Add(actions, 0, 3);
        Controls.Add(layout);
        ResumeLayout(true);
    }
}
