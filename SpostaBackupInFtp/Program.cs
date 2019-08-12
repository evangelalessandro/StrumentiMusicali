using NLog;
using NLog.Targets;
using StrumentiMusicali.ftpBackup.Backup;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostaBackupInFtp
{
    class Program
    {
        internal static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        static int Main(string[] args)
        {
           
            ConfigureNLog();

            
            _logger.Warn("Avvio del processo di spostamento backup");

            if (Environment.GetCommandLineArgs().Where(a => a == "NoBackup").Count() == 0)
            {
                try
                {
                    EseguiBackup();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex,"Generazione backup", null);
                    return (int)ExitCode.Error;
                }
                
            }
            using (var back = new BackupManager())
            {
                if (back.Manage())
                {
                    return (int)ExitCode.Success;
                }
                return (int)ExitCode.Error;
            }
        }
        private static void EseguiBackup()
        {
            using (var uof = new UnitOfWork())
            {
                uof.EseguiBackup();
               
            }
        }
        enum ExitCode : int
        {
            Success = 0,
            Error = 1,

        }

        public static void LogMethod(string level, string message, string exception, string stacktrace, string classLine)
        {
            try
            {
                using (var uof = new UnitOfWork())
                {
                    uof.EventLogRepository.Add(new StrumentiMusicali.Library.Entity.EventLog()
                    { TipoEvento = level, Errore = message, DataCreazione = DateTime.Now, InnerException = exception, StackTrace = stacktrace, Class = classLine });

                    uof.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Errore nel salvataggio log\errore! " + Environment.NewLine + ex.Message);
            }
        }

        private static void ConfigureNLog()
        {
            MethodCallTarget target = new MethodCallTarget();
            target.ClassName = typeof(Program).AssemblyQualifiedName;
            target.MethodName = "LogMethod";
            target.Parameters.Add(new MethodCallParameter("${level}"));
            target.Parameters.Add(new MethodCallParameter("${message}"));
            target.Parameters.Add(new MethodCallParameter("${exception:format=tostring,Data:maxInnerExceptionLevel=10}"));
            target.Parameters.Add(new MethodCallParameter("${stacktrace}"));
            target.Parameters.Add(new MethodCallParameter("${callsite}"));

            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
        }

    }
}
