using System.Drawing;
using System.Windows.Forms;

namespace StrumentiMusicaliApp.Settings
{
    public class UserSettings
    {
        public string LastStringaRicerca { get; set; }
		public Size SizeFormMain { get; set; } = new Size(400, 400);
		public string LastArticoloSelected { get; set; } 
		public FormWindowState FormMainWindowState { get; set; } = FormWindowState.Maximized;
	}
}