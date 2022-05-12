using NLog;
using NLog.Targets;
using StrumentiMusicali.PrestaShopSyncro.Job;
using System;
 
 
namespace testConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TestLog();

            TestPresta();

            //Console.WriteLine("Fine");
            //Console.Read();
        }

        private static void TestPresta()
		{
			SyncStockFromWeb job=new SyncStockFromWeb();
			job.Exec();

			//AggiornaLocaleDaWeb();

			//var sync = new StrumentiMusicali.PrestaShopSyncro.Sync.ProductSyncroLocalToWeb();
			//sync.AggiornaWeb();


		}

		private static void AggiornaLocaleDaWeb()
		{
			var sync = new StrumentiMusicali.PrestaShopSyncro.Sync.ProductSyncroWebToLocal();
			sync.SaveLocalFromSite();
			 
		}

		private static void TestLog()
        {
            var target = new ColoredConsoleTarget();
            //target.ClassName = typeof(ControllerMaster).AssemblyQualifiedName;
            //target.MethodName = "LogMethod";
            //target.Parameters.Add(new MethodCallParameter("${level}"));
            //target.Parameters.Add(new MethodCallParameter("${message}"));
            //target.Parameters.Add(new MethodCallParameter("${exception:format=tostring,Data:maxInnerExceptionLevel=10}"));
            //target.Parameters.Add(new MethodCallParameter("${stacktrace}"));
            //target.Parameters.Add(new MethodCallParameter("${callsite}"));

            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);

            //StrumentiMusicali.Core.Manager.ManagerLog.Logger.AddLogMessage("Avvio");
            //var ftpTest = new FtpManager();
            ////class1.Upload(@"C:\Users\fastcode13042017\Downloads\DocFatture\b9f768fb-8f3e-4a42-b54f-11fbde23ec50\Answers-to-Difficult-Bible-Passages.pdf");
            ////class1.Delete("Answers-to-Difficult-Bible-Passages.pdf");
            //var list =ftpTest.FileList();

            //var back = new BackupManager();
            //back.Manage();
            //var obj = new StrumentiMusicali.Library.Model.ModelSm();
            ////obj.sql 
        }
    }
}
