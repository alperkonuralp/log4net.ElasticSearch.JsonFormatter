using MessagePack;
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
			for (int i = 0; i < IterationCount; i++)
			{
				var message = new LogEventMessage(false)
				{
					TargetIndex = "TargetIndex",
					LoggerName = "LoggerName",
					Facility = "Facility",
					TimeStamp = new DateTime(2020, 12, 8, 18, 32, 0).ToUniversalTime(),
					Message = "Message",
					MessageObject = new { Id = 1, Name = "Alper" },
					Exception = new { Message = "Hata", StackTrace = "StackTrace" },
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
				result.ShouldBe("{\"target_index\":\"TargetIndex\",\"logger_name\":\"LoggerName\",\"facility\":\"Facility\",\"timestamp\":1607441520,\"message\":\"Message\",\"message_object\":{\"Id\":1,\"Name\":\"Alper\"},\"exception\":{\"Message\":\"Hata\",\"StackTrace\":\"StackTrace\"},\"domain\":\"Domain\",\"identity\":\"Identity\",\"level\":\"Level\",\"class\":\"ClassName\",\"file_name\":\"FileName\",\"line_number\":\"LineNumber\",\"full_info\":\"FullInfo\",\"method_name\":\"MethodName\",\"fix\":\"Fix\",\"user_name\":\"UserName\",\"thread_name\":\"ThreadName\",\"host_name\":\"HostName\",\"version\":\"Version\",\"short_message\":\"ShortMessage\"}");
			}
		}

		[Fact]
		public void Encode_WhenParameterIsTestClass_ReturnString()
		{
			for (int i = 0; i < IterationCount; i++)
			{
				var message = new TestClass()
				{
					TargetIndex = "TargetIndex",
					LoggerName = "LoggerName",
					Facility = "Facility",
					TimeStamp = new DateTime(2020, 12, 8, 18, 32, 0),
					Message = "Message",
					MessageObject = new { Id = 1, Name = "Alper" },
					Exception = new { Message = "Hata", StackTrace = "StackTrace" },
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
				}.ToDictionary();

				var result = JsonCoder.Encode(message);

				result.ShouldNotBeNull();
				result.ShouldNotBeEmpty();
				//result.ShouldBe("{\"TargetIndex\":\"TargetIndex\",\"Facility\":\"Facility\",\"TimeStamp\":{\"$type\":\"System.DateTime, System.Private.CoreLib, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = 7cec85d7bea7798e\",637430491200000000},\"Message\":\"Message\",\"MessageObject\":{\"Id\":1,\"Name\":\"Alper\"},\"Exception\":{\"Message\":\"Hata\",\"StackTrace\":\"StackTrace\"},\"LoggerName\":\"LoggerName\",\"Domain\":\"Domain\",\"Identity\":\"Identity\",\"Level\":\"Level\",\"ClassName\":\"ClassName\",\"FileName\":\"FileName\",\"LineNumber\":\"LineNumber\",\"FullInfo\":\"FullInfo\",\"MethodName\":\"MethodName\",\"Fix\":\"Fix\",\"UserName\":\"UserName\",\"ThreadName\":\"ThreadName\",\"HostName\":\"HostName\",\"Version\":\"Version\",\"ShortMessage\":\"ShortMessage\"}");
			}
		}

		[MessagePackObject]
		public class TestClass
		{
			[Key("target_index")]
			public string TargetIndex { get; set; }

			[Key("facility")]
			public string Facility { get; set; }

			[Key("timestamp")]
			public DateTime TimeStamp { get; set; }

			[Key("message")]
			public string Message { get; set; }

			[Key("message_object")]
			public object MessageObject { get; set; }

			[Key("exception")]
			public object Exception { get; set; }

			[Key("logger_name")]
			public string LoggerName { get; set; }

			[Key("domain")]
			public string Domain { get; set; }

			[Key("identity")]
			public string Identity { get; set; }

			[Key("level")]
			public string Level { get; set; }

			[Key("class_name")]
			public string ClassName { get; set; }

			[Key("file_name")]
			public string FileName { get; set; }

			[Key("line_number")]
			public string LineNumber { get; set; }

			[Key("full_info")]
			public string FullInfo { get; set; }

			[Key("method_name")]
			public string MethodName { get; set; }

			[Key("fix")]
			public string Fix { get; set; }

			[Key("user_name")]
			public string UserName { get; set; }

			[Key("thread_name")]
			public string ThreadName { get; set; }

			[Key("host_name")]
			public string HostName { get; set; }

			[Key("version")]
			public string Version { get; set; }

			[Key("short_message")]
			public string ShortMessage { get; set; }
		}
	}
}