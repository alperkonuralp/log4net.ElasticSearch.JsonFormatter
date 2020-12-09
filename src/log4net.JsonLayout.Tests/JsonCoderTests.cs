using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace log4net.JsonLayout.Tests
{
	public class JsonCoderTests
	{
		[Fact]
		public void Encode_WhenParameterIsString_ReturnString()
		{
			var result = JsonCoder.Encode("str");

			result.ShouldBe("\"str\"");
		}

		[Fact]
		public void Encode_WhenParameterIsDictionary_ReturnString()
		{
			var dic = new Dictionary<string, object>() {
				{"a","b" },
				{"c", 123 }
			};

			var result = JsonCoder.Encode(dic);

			result.ShouldBe("{\"a\":\"b\",\"c\":123}");
		}

		[Fact]
		public void EncodeDecode_WhenInObjectOutDictionary_ThenSuccessfully()
		{
			var obj = new
			{
				Id = 1,
				Name = "Alper"
			};

			var result = JsonCoder.Encode(obj);

			result.ShouldNotBeNull();
			result.ShouldNotBeEmpty();
			result.ShouldBe("{\"Id\":1,\"Name\":\"Alper\"}");

			var result2 = JsonCoder.Decode<Dictionary<string, object>>(result);

			result2.ShouldNotBeNull();
			result2.ContainsKey(nameof(obj.Id)).ShouldBeTrue();
			result2.ContainsKey(nameof(obj.Name)).ShouldBeTrue();
			result2.Count.ShouldBe(2);
			result2[nameof(obj.Id)].ShouldBe(obj.Id);
			result2[nameof(obj.Name)].ShouldBe(obj.Name);
		}
	}
}