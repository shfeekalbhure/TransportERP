using System.ComponentModel;
using System.Resources;
namespace TransportERP.Desktop.CoreUI;
public static class CommandBarImages
{
    private static readonly ResourceManager Resources = new(typeof(StandardCommandBar));
    public static Image? Add => Resources.GetObject("btnAdd.Image") as Image;
    public static Image? Edit => Resources.GetObject("btnEdit.Image") as Image;
    public static Image? Delete => Resources.GetObject("btnDelete.Image") as Image;
    public static Image? Cancel => Resources.GetObject("btnCancel.Image") as Image;
    public static Image? View => Resources.GetObject("btnView.Image") as Image;
    public static Image? Last => Resources.GetObject("btnLast.Image") as Image;
    public static Image? Next => Resources.GetObject("btnNext.Image") as Image;
    public static Image? Previous => Resources.GetObject("btnPrevious.Image") as Image;
    public static Image? First => Resources.GetObject("btnFirst.Image") as Image;
    public static Image? Save => Resources.GetObject("btnSave.Image") as Image;
    public static Image? Print => Resources.GetObject("btnPrint.Image") as Image;
    public static Image? Close => Resources.GetObject("btnClose.Image") as Image;
}
