using StrumentiMusicali.Library.Repo;
using System.Linq;

namespace TestMOdel
{
	  class Program
	{
		  static void Main(string[] args)
		{
            StrumentiMusicali.ftpBackup.Class1 class1 = new StrumentiMusicali.ftpBackup.Class1();
            class1.Upload();
        }
	}
}