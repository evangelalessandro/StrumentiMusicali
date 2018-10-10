using NLog;
using NLog.Targets;
using StrumentiMusicaliSql.Repo;
using StrumentiMusicaliApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicaliApp
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {

			
			using (var controller=new Controller())
			{
				
			}
		}
		
    }
}
