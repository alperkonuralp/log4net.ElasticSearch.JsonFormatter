using System;
using System.Collections.Generic;

namespace log4net.JsonLayout
{
	public class JsonDictionary : Dictionary<string, object>
	{
		protected string PullStringValue(String key)
		{
			return ContainsKey(key) ? this[key].ToString() : string.Empty;
		}
		protected int? PullInt32Value(String key)
		{
			return ContainsKey(key) ? ToInt32(this[key]) : default;
		}

		protected JsonSerializableException PullExceptionValue(String key)
		{
			return ContainsKey(key) ? this[key] as JsonSerializableException : null;
		}

		protected void StoreValue(string key, object value)
		{
			if (!ContainsKey(key))
				Add(key, value);
			else
				this[key] = value;
		}

		internal void AddRange(IEnumerable<KeyValuePair<string, string>> enumerable)
		{
			foreach (var p in enumerable)
				this.Add(p.Key, p.Value);
		}

		private int? ToInt32(object obj)
		{
			if (obj == null) return null;
			if(obj is Int32 v)
			{
				return v;
			}
			if (Int32.TryParse(obj.ToString(), out v))
				return v;

			return null;
		}
	}
}