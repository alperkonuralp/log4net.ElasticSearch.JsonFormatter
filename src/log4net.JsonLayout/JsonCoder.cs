using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Util;
using MessagePack;
using MessagePack.Formatters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace log4net.JsonLayout
{
	public static class JsonCoder
	{
		public static string Encode<T>(T o, CancellationToken cancellationToken = default)
		{
			return MessagePackSerializer.SerializeToJson<T>(o, MessagePackSerializer.Typeless.DefaultOptions, cancellationToken);
		}
		public static T Decode<T>(string jsonText, CancellationToken cancellationToken = default)
		{
			var binary = MessagePackSerializer.ConvertFromJson(jsonText, MessagePackSerializer.Typeless.DefaultOptions, cancellationToken);
			return MessagePackSerializer.Deserialize<T>(binary, MessagePackSerializer.Typeless.DefaultOptions, cancellationToken);
		}
	}

}