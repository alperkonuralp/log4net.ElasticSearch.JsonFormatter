using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace log4net.JsonLayout.Tests
{
	public class JsonCoderDictionartTests
	{
		private const int IterationCount = 100000;

		[Fact]
		public void Encode_WhenParameterIsDictionary_ReturnString()
		{
			for (int i = 0; i < IterationCount; i++)
			{
				var dic = new Dictionary<string, object>() {
					{"a","b" },
					{"c", 123 }
				};

				var result = JsonCoder.Encode(dic);

				result.ShouldNotBeNull();
				result.ShouldNotBeEmpty();
				result.ShouldBe("{\"a\":\"b\",\"c\":123}");
			}
		}

		[Fact]
		public void Encode_WhenParameterIsLogEventMessage_ReturnString()
		{
			var date = new DateTime(2020, 12, 8, 18, 32, 0).ToUniversalTime();
			var dateString = date.ToUnixTimestamp().ToString();
			for (int i = 0; i < IterationCount; i++)
			{
				var message = new LogEventMessage(false)
				{
					TargetIndex = "TargetIndex",
					LoggerName = "LoggerName",
					Facility = "Facility",
					TimeStamp = date,
					Message = "Message",
					MessageObject = new { Id = 1, Name = "Alper" },
					//Exception = new { Message = "Hata", StackTrace = "StackTrace" },
					Domain = "Domain",
					Identity = "Identity",
					Level = "Level",
					ClassName = "ClassName",
					FileName = "FileName",
					LineNumber = "LineNumber",
					FullInfo = "FullInfo",
					MethodName = "MethodName",
					Fix = "Fix",
					UserName = "UserName",
					ThreadName = "ThreadName",
					HostName = "HostName",
					Version = "Version",
					ShortMessage = "ShortMessage",
				};

				var result = JsonCoder.Encode(message);

				result.ShouldNotBeNull();
				result.ShouldNotBeEmpty();
				//result.ShouldBe($"{{\"target_index\":\"TargetIndex\",\"logger_name\":\"LoggerName\",\"facility\":\"Facility\",\"timestamp\":{dateString},\"message\":\"Message\",\"message_object\":{{\"Id\":1,\"Name\":\"Alper\"}},\"exception\":{{\"Message\":\"Hata\",\"StackTrace\":\"StackTrace\"}},\"domain\":\"Domain\",\"identity\":\"Identity\",\"level\":\"Level\",\"class\":\"ClassName\",\"file_name\":\"FileName\",\"line_number\":\"LineNumber\",\"full_info\":\"FullInfo\",\"method_name\":\"MethodName\",\"fix\":\"Fix\",\"user_name\":\"UserName\",\"thread_name\":\"ThreadName\",\"host_name\":\"HostName\",\"version\":\"Version\",\"short_message\":\"ShortMessage\"}}");
				result.ShouldBe($"{{\"target_index\":\"TargetIndex\",\"logger_name\":\"LoggerName\",\"facility\":\"Facility\",\"timestamp\":{dateString},\"message\":\"Message\",\"message_object\":{{\"Id\":1,\"Name\":\"Alper\"}},\"domain\":\"Domain\",\"identity\":\"Identity\",\"level\":\"Level\",\"class\":\"ClassName\",\"file_name\":\"FileName\",\"line_number\":\"LineNumber\",\"full_info\":\"FullInfo\",\"method_name\":\"MethodName\",\"fix\":\"Fix\",\"user_name\":\"UserName\",\"thread_name\":\"ThreadName\",\"host_name\":\"HostName\",\"version\":\"Version\",\"short_message\":\"ShortMessage\"}}");
			}
		}
	}
}