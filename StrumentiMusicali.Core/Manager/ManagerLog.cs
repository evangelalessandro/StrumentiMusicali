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
        public static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void AddLogObject(object obj, string message)
        {
            
            Logger.Info( message+ Environment.NewLine  + SerializeXmlObject(obj));
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
