using System;
using System.Globalization;

namespace log4net.JsonLayout
{
	/// <summary>
	/// Primary object which will get serialized into a json object to pass to ES. Deviating from CamelCase
	/// class members so that we can stick with the build in serializer and not take a dependency on another lib. ES
	/// exepects fields to start with lowercase letters.
	/// </summary>
	public partial class LogEventMessage : JsonDictionary
	{
		private const string FacilityKey = "facility";
		private const string TargetIndexKey = "target_index";
		private const string FileNameKey = "file_name";
		private const string MessageKey = "message";
		private const string HostNameKey = "host_name";
		private const string LevelKey = "level";
		private const string LineNumberKey = "line_number";
		private const string ShortMessageKey = "short_message";
		private const string VersionKey = "version";
		private const string LoggerNameKey = "logger_name";
		private const string IdentityKey = "identity";
		private const string ClassKey = "class";
		private const string DomainKey = "domain";
		private const string FullInfoKey = "full_info";
		private const string MethodNameKey = "method_name";
		private const string TimeStampKey = "timestamp";
		private const string FixKey = "fix";
		private const string UserNameKey = "user_name";
		private const string ThreadNameKey = "thread_name";
		private const string MessageObjectKey = "message_object";
		private const string ExceptionKey = "exception";

		private readonly bool _sentTimeStampAsString;

		public LogEventMessage() : this(false)
		{
		}

		public LogEventMessage(bool sendTimeStampAsString) : base()
		{
			_sentTimeStampAsString = sendTimeStampAsString;
		}

		public string TargetIndex
		{
			get { return PullStringValue(TargetIndexKey); }
			set { StoreValue(TargetIndexKey, value); }
		}

		public string Facility
		{
			get { return PullStringValue(FacilityKey); }
			set { StoreValue(FacilityKey, value); }
		}

		public DateTime TimeStamp
		{
			get
			{
				if (!this.ContainsKey(TimeStampKey))
					return DateTime.MinValue;

				var val = this[TimeStampKey];
				if (_sentTimeStampAsString)
				{
					var parsed = double.TryParse(val as string, NumberStyles.Any, CultureInfo.InvariantCulture, out double value);
					return parsed ? value.FromUnixTimestamp() : DateTime.MinValue;
				}

				return Convert.ToDouble(val).FromUnixTimestamp();
			}
			set
			{
				var timestamp = value.ToUnixTimestamp();
				StoreValue(TimeStampKey, timestamp);
				if (_sentTimeStampAsString)
				{
					StoreValue(TimeStampKey, timestamp.ToString(CultureInfo.InvariantCulture));
				}
			}
		}

		public string Message
		{
			get { return PullStringValue(MessageKey); }
			set { StoreValue(MessageKey, value); }
		}

		public object MessageObject
		{
			get { return PullStringValue(MessageObjectKey); }
			set { StoreValue(MessageObjectKey, value); }
		}

		public JsonSerializableException Exception
		{
			get { return PullExceptionValue(ExceptionKey); }
			set { StoreValue(ExceptionKey, value); }
		}

		public string LoggerName
		{
			get { return PullStringValue(LoggerNameKey); }
			set { StoreValue(LoggerNameKey, value); }
		}

		public string Domain
		{
			get { return PullStringValue(DomainKey); }
			set { StoreValue(DomainKey, value); }
		}

		public string Identity
		{
			get { return PullStringValue(IdentityKey); }
			set { StoreValue(IdentityKey, value); }
		}

		public string Level
		{
			get { return PullStringValue(LevelKey); }
			set { StoreValue(LevelKey, value); }
		}

		public string ClassName
		{
			get { return PullStringValue(ClassKey); }
			set { StoreValue(ClassKey, value); }
		}

		public string FileName
		{
			get { return PullStringValue(FileNameKey); }
			set { StoreValue(FileNameKey, value); }
		}

		public string LineNumber
		{
			get { return PullStringValue(LineNumberKey); }
			set { StoreValue(LineNumberKey, value); }
		}

		public string FullInfo
		{
			get { return PullStringValue(FullInfoKey); }
			set { StoreValue(FullInfoKey, value); }
		}

		public string MethodName
		{
			get { return PullStringValue(MethodNameKey); }
			set { StoreValue(MethodNameKey, value); }
		}

		public string Fix
		{
			get { return PullStringValue(FixKey); }
			set { StoreValue(FixKey, value); }
		}

		public string UserName
		{
			get { return PullStringValue(UserNameKey); }
			set { StoreValue(UserNameKey, value); }
		}

		public string ThreadName
		{
			get { return PullStringValue(ThreadNameKey); }
			set { StoreValue(ThreadNameKey, value); }
		}

		public string HostName
		{
			get { return PullStringValue(HostNameKey); }
			set { StoreValue(HostNameKey, value); }
		}

		public string Version
		{
			get { return PullStringValue(VersionKey); }
			set { StoreValue(VersionKey, value); }
		}

		public string ShortMessage
		{
			get { return PullStringValue(ShortMessageKey); }
			set { StoreValue(ShortMessageKey, value); }
		}
	}
}