using System;

namespace log4net.JsonLayout
{
	public class JsonSerializableException : JsonDictionary
	{
		private const string TypeKey = "type";
		private const string MessageKey = "message";
		private const string HelpLinkKey = "help_link";
		private const string SourceKey = "source";
		private const string HResultKey = "hresult";
		private const string StackTraceKey = "stack_trace";
		private const string DataKey = "data";
		private const string InnerExceptionKey = "inner_exception";

		public string Type
		{
			get { return PullStringValue(TypeKey); }
			set { StoreValue(TypeKey, value); }
		}

		public string Message
		{
			get { return PullStringValue(MessageKey); }
			set { StoreValue(MessageKey, value); }
		}

		public string HelpLink
		{
			get { return PullStringValue(HelpLinkKey); }
			set { StoreValue(HelpLinkKey, value); }
		}

		public string Source
		{
			get { return PullStringValue(SourceKey); }
			set { StoreValue(SourceKey, value); }
		}

		public int? HResult
		{
			get { return PullInt32Value(HResultKey); }
			set { StoreValue(HResultKey, value); }
		}

		public string StackTrace
		{
			get { return PullStringValue(StackTraceKey); }
			set { StoreValue(StackTraceKey, value); }
		}

		public System.Collections.IDictionary Data
		{
			get
			{
				return ContainsKey(DataKey)
					? (this[DataKey] as System.Collections.IDictionary)
					: null;
			}
			set
			{
				this[DataKey] = value;
			}
		}

		public JsonSerializableException InnerException
		{
			get
			{
				return ContainsKey(InnerExceptionKey)
					? (this[InnerExceptionKey] as JsonSerializableException)
					: null;
			}
			set
			{
				this[InnerExceptionKey] = value;
			}
		}

		public static JsonSerializableException Create(Exception ex)
		{
			if (ex == null)
				return null;

			var serializable = new JsonSerializableException
			{
				Type = ex.GetType().FullName,
				Message = ex.Message,
				HelpLink = ex.HelpLink,
				Source = ex.Source,
				StackTrace = ex.StackTrace,
				Data = ex.Data
			};

			serializable.InnerException = Create(ex.InnerException);
			return serializable;
		}
	}
}