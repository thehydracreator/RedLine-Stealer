using System.Drawing;
using System.Windows.Forms;

public static class ControlEx
{
    public static void Center(this Control control)
    {
        control.Location = new Point((control.Parent.Width - control.Width) / 2, (control.Parent.Height - control.Height) / 2);
    }
}
