using StrumentiMusicali.Library.Repo;
using System.Linq;
namespace TestMOdel
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var uof=new UnitOfWork())
			{
				uof.ArticoliRepository.Find(a => 1 == 1).Select(a => a.Titolo);

			}
		}
	}
}
