using StrumentiMusicali.App.Core;
using System;

namespace StrumentiMusicali.App
{
	internal static class Program
	{
		/// <summary>
		/// Punto di ingresso principale dell'applicazione.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			using (var controller = new ControllerMaster())
			{
			}
		}
	}
}