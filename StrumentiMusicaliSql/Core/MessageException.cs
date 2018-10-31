using System;
using System.Collections.Generic;
using System.Text;

namespace StrumentiMusicali.Library.Core
{
	public class MessageException : Exception
	{
		public MessageException(List<string> message)
			: base()
		{
			StringBuilder sb = new StringBuilder();

			foreach (var item in message)
			{
				sb.AppendLine(item);
			}
			Messages = sb.ToString();
		}

		public string Messages { get; private set; }
	}
}