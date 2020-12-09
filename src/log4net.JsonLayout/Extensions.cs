using log4net.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace log4net.JsonLayout
{
	public static class Extensions
	{
		private static HashSet<Type> _numericTypes = new HashSet<Type>
				{
						typeof(decimal),
						typeof(double),
						typeof(float),
						typeof(int),
						typeof(uint),
						typeof(long),
						typeof(ulong),
						typeof(short),
						typeof(ushort)
				};

		public static bool IsNumeric(this Type type)
		{
			return _numericTypes.Contains(type);
		}

		//public static IDictionary<string, object> ToDictiponary(this LogEventMessage message)
		//{
		//	return new Dictionary<string, object>(message.Properties)
		//	{
		//	};
		//}

		public delegate string NameConverterDelegate(string name);

		public static IDictionary ToDictionary<T>(this T values, NameConverterDelegate nameConverter = null)
			where T : class
		{
			if (values == null)
			{
				return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			}

			if (nameConverter != null)
			{
				return TypeDescriptor.GetProperties(values)
					.Cast<PropertyDescriptor>()
					.ToDictionary(x => nameConverter(x.Name), x => x.GetValue(values), StringComparer.OrdinalIgnoreCase);
			}

			return TypeDescriptor.GetProperties(values)
				.Cast<PropertyDescriptor>()
				.ToDictionary(x => x.Name, x => x.GetValue(values), StringComparer.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Truncate the message
		/// </summary>
		public static string TruncateMessage(this string message, int length)
		{
			return (message.Length > length)
								 ? message.Substring(0, length - 1)
								 : message;
		}

		public static double ToUnixTimestamp(this DateTime d)
		{
			var duration = d.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);

			return duration.TotalSeconds;
		}

		public static DateTime FromUnixTimestamp(this double d)
		{
			var datetime = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(d * 1000).ToLocalTime();

			return datetime;
		}

		internal static IEnumerable<KeyValuePair<string, string>> AsPairs(this ReadOnlyPropertiesDictionary self)
		{
			return self.GetKeys().Select(key => Pair.For(key, self[key].ToStringOrNull()));
		}

		internal static string ToStringOrNull(this object self)
		{
			return self != null ? self.ToString() : null;
		}

		internal static bool IsNullOrEmpty(this string self)
		{
			return string.IsNullOrEmpty(self);
		}

		internal static void AddRance<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> pairs)
		{
			foreach (var p in pairs)
				dictionary.Add(p.Key, p.Value);
		}
	}
}