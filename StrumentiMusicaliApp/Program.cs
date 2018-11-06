using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Articoli;
using StrumentiMusicali.Library.Core;
using System;

namespace StrumentiMusicali.App
{
	internal static class Program
	{
		/// <summary>
		/// Punto di ingresso principale dell'applicazione.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			if (args == null || args.Length==0 || string.IsNullOrEmpty( args[0]))
			{
				using (var controller = new ControllerMaster())
				{
					controller.ShowMainView();
				}
			}
			else
			{
				if (args[0]=="INVIO")
				{
					using (var controller=new ControllerMaster())
					{
						EventAggregator.Instance().Publish<InvioArticoliCSV>(new InvioArticoliCSV());
					}
				}
			}
		}
	}
}