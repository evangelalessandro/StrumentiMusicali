using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicaliSql.Core
{
	public class MessageException :Exception
	{
		public MessageException(List<string> message)
			:base()
		{
			StringBuilder sb = new StringBuilder();

			foreach (var item in message)
			{
				sb.AppendLine( item);
			}
			Messages = sb.ToString();
		}
		public string Messages { get; private set; }

	}
}
