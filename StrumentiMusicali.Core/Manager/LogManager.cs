using NLog;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace StrumentiMusicali.Core.Manager
{
    public class ManagerLog
    {
        public static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void AddLogObject(object obj, string message)
        {
            var stringBuilder = new StringBuilder();
            var serializer = new Serializer();
            serializer.Serialize(new IndentedTextWriter(new StringWriter(stringBuilder)), obj);
            
            _logger.Info( message+ Environment.NewLine  + stringBuilder.ToString());
        }

        public static void AddLogMessage(string message)
        {
            _logger.Info(message);
        }
        public static void AddLogException(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }
    }
}
