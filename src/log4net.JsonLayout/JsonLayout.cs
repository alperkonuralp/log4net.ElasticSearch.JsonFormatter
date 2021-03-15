using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace log4net.JsonLayout
{
	/// <summary>
	/// New Lined Log Event Layout. This layout adds to new line after Log Event message.
	/// It is using to write Log Event message to console.
	/// </summary>
	public class JsonLayout : LayoutSkeleton
	{
		private const string LOG_EVENT_VERSION = "1.0";
		private const int SHORT_MESSAGE_LENGTH = 250;
		private readonly PatternLayout _patternLayout = new PatternLayout();

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonLayout"/> class.
		/// </summary>
		public JsonLayout()
		{
			IncludeLocationInformation = false;
			IgnoresException = false;
			SendTimeStampAsString = false;
		}

		/// <summary>
		/// The content type output by this layout.
		/// </summary>
		public override string ContentType => "application/json";

		/// <summary>
		/// Gets or sets Facility.
		/// </summary>
		public string Facility { get; set; } = "LogEvent";

		/// <summary>
		/// Gets or sets the name of the host.
		/// </summary>
		/// <value>
		/// The name of the host.
		/// </value>
		public string HostName { get; set; }

		public string TargetIndexPrefix { get; set; } = "logstash";

		/// <summary>Gets or sets the field separator.</summary>
		/// <value>The field separator.</value>
		public string FieldSeparator { get; set; }

		/// <summary>Gets or sets the key value separator.</summary>
		/// <value>The key value separator.</value>
		public string KeyValueSeparator { get; set; }

		/// <summary>
		/// The additional fields configured in log4net config.
		/// </summary>
		public string AdditionalFields { get; set; }

		/// <summary>
		/// The conversion pattern to use for the message body
		/// </summary>
		public string ConversionPattern { get; set; }

		/// <summary>
		/// Specifies wehter location inormation should be included in the message
		/// </summary>
		public bool IncludeLocationInformation { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [send time stamp as string].
		/// </summary>
		/// <value>
		///   <c>true</c> if [send time stamp as string]; otherwise, <c>false</c>.
		/// </value>
		public bool SendTimeStampAsString { get; set; }

		public bool AppendNewLine { get; set; } = false;

		/// <summary>Activates the options.</summary>
		public override void ActivateOptions()
		{
		}

		private Dictionary<string, object> ParseField(string value)
		{
			if (value == null)
				return new Dictionary<string, object>();

			string[] fields = !string.IsNullOrEmpty(FieldSeparator)
					? value.Split(new[] { FieldSeparator }, StringSplitOptions.RemoveEmptyEntries)
					: value.Split(',');

			Dictionary<string, object> innerAdditionalFields = !string.IsNullOrEmpty(KeyValueSeparator)
									? fields
										.Select(it => it.Split(new[] { KeyValueSeparator }, StringSplitOptions.RemoveEmptyEntries))
										.ToDictionary(it => it[0], it => (object)it[1])
									: fields
										.Select(it => it.Split(':'))
										.ToDictionary(it => it[0], it => (object)it[1]);
			return innerAdditionalFields;
		}

		/// <summary>Formats the specified writer.</summary>
		/// <param name="writer">The writer.</param>
		/// <param name="loggingEvent">The logging event.</param>
		public override void Format(System.IO.TextWriter writer, LoggingEvent loggingEvent)
		{
			var logEventMessage = GetBaseLogEventMessage(loggingEvent);

			AddLoggingEventToMessage(loggingEvent, logEventMessage);

			AddAdditionalFields(loggingEvent, logEventMessage);

			writer.Write(JsonCoder.Encode(logEventMessage));
			if (AppendNewLine) writer.WriteLine();
		}

		private void AddLoggingEventToMessage(LoggingEvent loggingEvent, LogEventMessage logEventMessage)
		{
			if (loggingEvent.MessageObject != null && !(loggingEvent.MessageObject is string))
			{
				logEventMessage.MessageObject = loggingEvent.MessageObject;
			}

			logEventMessage.Exception = JsonSerializableException.Create(loggingEvent.ExceptionObject);

			//If conversion pattern is specified then defer to PatterLayout for building the message body
			if (!string.IsNullOrWhiteSpace(ConversionPattern))
			{
				var message = GetValueFromPattern(loggingEvent, ConversionPattern);
				logEventMessage.Message = message;
				logEventMessage.ShortMessage = message.TruncateMessage(SHORT_MESSAGE_LENGTH);
			}
			else //Otherwise do our custom message builder stuff
			{
				var messageObject = loggingEvent.MessageObject;
				if (messageObject == null)
				{
					if (!string.IsNullOrEmpty(loggingEvent.RenderedMessage))
					{
						try
						{
							var dic = JsonCoder.Decode<Dictionary<string, object>>(loggingEvent.RenderedMessage);
							logEventMessage.MessageObject = dic;
						}
						catch
						{
							// eğer rendered message decode edilebilir ise bunu yapıyoruz. yoksa birşey yapmayacağız.
						}

						FillMessagesIfEmpties(logEventMessage, loggingEvent.RenderedMessage);
						return;
					}
					logEventMessage.Message = SystemInfo.NullText;
					logEventMessage.ShortMessage = SystemInfo.NullText;
				}
				else if (messageObject is string || messageObject is SystemStringFormat)
				{
					var fullMessage = messageObject.ToString();
					logEventMessage.Message = fullMessage;
					logEventMessage.ShortMessage = fullMessage.TruncateMessage(SHORT_MESSAGE_LENGTH);
				}
				else if (messageObject is IDictionary)
				{
					logEventMessage.MessageObject = messageObject;
				}
				else
				{
					logEventMessage.MessageObject = messageObject;
				}

				FillMessagesIfEmpties(logEventMessage, messageObject?.ToString());
			}
		}

		private static void FillMessagesIfEmpties(LogEventMessage logEventMessage, string msg)
		{
			if (string.IsNullOrEmpty(logEventMessage.Message))
			{
				logEventMessage.Message = msg;
			}
			if (string.IsNullOrEmpty(logEventMessage.ShortMessage))
			{
				logEventMessage.ShortMessage = logEventMessage.Message.TruncateMessage(SHORT_MESSAGE_LENGTH);
			}
		}

		private LogEventMessage GetBaseLogEventMessage(LoggingEvent loggingEvent)
		{
			var today = loggingEvent.TimeStamp.ToUniversalTime().Date;
			var message = new LogEventMessage(SendTimeStampAsString)
			{
				Facility = Facility ?? "LogEvent",
				FileName = string.Empty,
				HostName = HostName ?? Environment.MachineName,
				Level = loggingEvent.Level?.DisplayName,
				LineNumber = string.Empty,
				TimeStamp = loggingEvent.TimeStamp.ToUniversalTime(),
				Version = LOG_EVENT_VERSION,
				LoggerName = loggingEvent.LoggerName,
				Domain = loggingEvent.Domain,
				TargetIndex = $"{this.TargetIndexPrefix}-{today:yyyyMMdd}",
				ThreadName = loggingEvent.ThreadName,
				Identity = loggingEvent.Identity,
				UserName = loggingEvent.UserName,
				Fix = loggingEvent.Fix.ToString(),
			};

			if (this.IncludeLocationInformation)
			{
				LocationInfo locationInformation = loggingEvent.LocationInformation;
				message.FileName = GetString(locationInformation.FileName);
				message.LineNumber = GetString(locationInformation.LineNumber);
				message.ClassName = GetString(locationInformation.ClassName);
				message.FullInfo = GetString(locationInformation.FullInfo);
				message.MethodName = GetString(locationInformation.MethodName);
			}

			message.AddRange(
				loggingEvent.GetProperties()
					.AsPairs()
					.Union(AppenderPropertiesFor(loggingEvent))
			);

			return message;
		}

		private string GetString(string str)
		{
			return str != "?" ? str : string.Empty;
		}

		private static IEnumerable<KeyValuePair<string, string>> AppenderPropertiesFor(LoggingEvent loggingEvent)
		{
			yield return Pair.For("@timestamp", loggingEvent.TimeStamp.ToUniversalTime().ToString("O"));
		}

		private void AddAdditionalFields(LoggingEvent loggingEvent, LogEventMessage message)
		{
			var additionalFields = ParseField(AdditionalFields) ?? new Dictionary<string, object>();
			foreach (DictionaryEntry item in loggingEvent.GetProperties())
			{
				if (item.Key is string key && !key.StartsWith("log4net:") /*exclude log4net built-in properties */ )
				{
					additionalFields.Add(key, FormatAdditionalField(item.Value));
				}
			}

			foreach (var kvp in additionalFields)
			{
				var key = kvp.Key.StartsWith("_") ? kvp.Key : "_" + kvp.Key;

				//If the value starts with a '%' then defer to the pattern layout
				var value = kvp.Value is string patternValue
					&& patternValue.StartsWith("%")
						? GetValueFromPattern(loggingEvent, patternValue)
						: kvp.Value;
				message[key] = value;
			}
		}

		private object FormatAdditionalField(object value)
		{
			return value == null || value.GetType().IsNumeric() ? value : value.ToString();
		}

		private string GetValueFromPattern(LoggingEvent loggingEvent, string pattern)
		{
			//Reset the pattern layout
			_patternLayout.ConversionPattern = pattern;
			_patternLayout.ActivateOptions();

			//Write the results
			var sb = new StringBuilder();
			using (var writer = new System.IO.StringWriter(sb))
			{
				_patternLayout.Format(writer, loggingEvent);
				writer.Flush();
				return sb.ToString();
			}
		}
	}
}