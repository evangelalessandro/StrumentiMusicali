using NLog;
using NLog.Targets;
using StrumentiMusicali.Library.Repo;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.Serialization;

namespace StrumentiMusicali.Core.Manager
{
    public class ManagerLog
    {
        public static ILogger Logger {
            get {
                ConfigureNLog();
                return LogManager.GetCurrentClassLogger();
            }
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
                MessageBox.Show(@"Errore nel salvataggio log\errore! " + Environment.NewLine + ex.Message, "info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private static bool _configuret;
        private static void ConfigureNLog()
        {
            if (_configuret)
                return;
            _configuret = true;
            MethodCallTarget target = new MethodCallTarget();
            target.ClassName = typeof(ManagerLog).AssemblyQualifiedName;
            target.MethodName = "LogMethod";
            target.Parameters.Add(new MethodCallParameter("${level}"));
            target.Parameters.Add(new MethodCallParameter("${message}"));
            target.Parameters.Add(new MethodCallParameter("${exception:format=tostring,Data:maxInnerExceptionLevel=10}"));
            target.Parameters.Add(new MethodCallParameter("${stacktrace}"));
            target.Parameters.Add(new MethodCallParameter("${callsite}"));

            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
        }
        public static void AddLogObject(object obj, string message)
        {

            Logger.Info(message + Environment.NewLine + SerializeXmlObject(obj));
        }

        public static string SerializeXmlObject(object obj)
        {
            var stringBuilder = new StringBuilder();
            var serializer = new Serializer();
            serializer.Serialize(new IndentedTextWriter(new StringWriter(stringBuilder)), obj);
            return stringBuilder.ToString();
        }
    }

}
