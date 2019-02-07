using StrumentiMusicali.Library.Repo;
using System.Linq;

namespace TestMOdel
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			using (var uof=new UnitOfWork())
			{
				var art =uof.ArticoliRepository.Find(a => a.BoxProposte == true).First();

			}
		}
	}
}