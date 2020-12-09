using AutoFixture;
using AutoFixture.AutoMoq;
using Shouldly;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace log4net.JsonLayout.Tests
{
	public class JsonLayoutTests
	{
		private readonly IFixture _fixture;

		public JsonLayoutTests()
		{
			_fixture = FixtureHelper.CreateFixture();
			_fixture.Customize(new SupportMutableValueTypesCustomization());
		}

		[Fact]
		public void Format_HappyPath()
		{
			var dateTime = new System.DateTime(2020, 12, 9, 17, 24, 25, 123);

			var loggingEventData = _fixture.Freeze<Core.LoggingEventData>();
			loggingEventData.TimeStampUtc = dateTime;
			
			var loggingEvent = _fixture.Build<Core.LoggingEvent>()
					.Create();

			var layout = _fixture.Build<JsonLayout>()
				.With(x => x.AdditionalFields, "Id:1,Name:Alper")
				.With(x => x.FieldSeparator, ",")
				.With(x => x.KeyValueSeparator, ":")
				.With(x=>x.SendTimeStampAsString, true)
				.With(x=>x.TargetIndexPrefix, "UnitTest")
				.Create();

			var result = layout.Format(loggingEvent);

			result.ShouldNotBeNull();
			result.ShouldNotBeEmpty();
			result.StartsWith("{\"").ShouldBeTrue();

			var resultDictionary = JsonCoder.Decode<Dictionary<string, object>>(result);

			resultDictionary["domain"].ShouldBe(loggingEventData.Domain);
			resultDictionary["identity"].ShouldBe(loggingEventData.Identity);
			resultDictionary["logger_name"].ShouldBe(loggingEventData.LoggerName);
			resultDictionary["thread_name"].ShouldBe(loggingEventData.ThreadName);
			resultDictionary["user_name"].ShouldBe(loggingEventData.UserName);
			resultDictionary["fix"].ShouldBe(loggingEvent.Fix.ToString());
			resultDictionary["timestamp"].ShouldBe(loggingEvent.TimeStamp.ToUnixTimestamp().ToString(CultureInfo.InvariantCulture));
			//resultDictionary["message"].ShouldBe(loggingEvent.Message);
		}
	}
}