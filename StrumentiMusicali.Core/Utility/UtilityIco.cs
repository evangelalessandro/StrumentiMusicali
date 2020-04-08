using System.Drawing;

namespace StrumentiMusicali.Core.Utility
{
    public static class UtilityIco
    {

        public static Icon GetIco(Bitmap bitmap)
        {
            Bitmap bm = new Bitmap(bitmap);

            // Convert to an icon and use for the form's icon.
            return Icon.FromHandle(bm.GetHicon());
        }
    }
}