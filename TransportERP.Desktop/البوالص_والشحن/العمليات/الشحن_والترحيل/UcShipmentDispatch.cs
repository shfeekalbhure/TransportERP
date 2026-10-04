using System;
using System.Windows.Forms;
using TransportERP.Desktop.البوالص_والشحن.العمليات.ترحيل_الشحنات.منسدلة_الجدول;

namespace TransportERP.Desktop.البوالص_والشحن.العمليات.الشحن_والترحيل
{
    public partial class UcShipmentDispatch : UserControl
    {
        public UcShipmentDispatch()
        {
            InitializeComponent();

            // ضمان أن الحدث مربوط مرة واحدة فقط حتى لو كان مربوطًا من الـ Designer.
            flpWaybills.SizeChanged -= FlpWaybills_SizeChanged;
            flpWaybills.SizeChanged += FlpWaybills_SizeChanged;

            // ضبط عرض أي صف بوليصة تتم إضافته لاحقًا.
            flpWaybills.ControlAdded -= FlpWaybills_ControlAdded;
            flpWaybills.ControlAdded += FlpWaybills_ControlAdded;

            AdjustWaybillRowsWidth();
        }

        private void FlpWaybills_SizeChanged(object? sender, EventArgs e)
        {
            AdjustWaybillRowsWidth();
        }

        private void FlpWaybills_ControlAdded(object? sender, ControlEventArgs e)
        {
            if (e.Control is UcDispatchWaybillRow row)
            {
                AdjustWaybillRowWidth(row);
            }
        }

        private void AdjustWaybillRowsWidth()
        {
            foreach (Control control in flpWaybills.Controls)
            {
                if (control is UcDispatchWaybillRow row)
                {
                    AdjustWaybillRowWidth(row);
                }
            }
        }

        private void AdjustWaybillRowWidth(UcDispatchWaybillRow row)
        {
            int availableWidth =
                flpWaybills.ClientSize.Width
                - flpWaybills.Padding.Horizontal
                - row.Margin.Horizontal;

            // أثناء بداية إنشاء الواجهة قد يكون العرض غير جاهز بعد.
            // في هذه الحالة نترك العرض كما هو، وسيعاد ضبطه عند SizeChanged.
            if (availableWidth <= 0)
            {
                return;
            }

            row.Width = availableWidth;
        }

        // أبقينا هذا الحدث لأن ملف Designer.cs غير متوفر،
        // وقد يكون الحدث مربوطًا به حاليًا.
        private void ucDispatchWaybillRow1_Load(object sender, EventArgs e)
        {
            AdjustWaybillRowsWidth();
        }

        // أبقينا أحداث Paint الحالية كما هي حتى لا نكسر أي ربط
        // موجود في Designer.cs. يمكن حذفها لاحقًا بعد فحص الـ Designer.
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {
        }

        private void tableLayoutPanel4_Paint_1(object sender, PaintEventArgs e)
        {
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}