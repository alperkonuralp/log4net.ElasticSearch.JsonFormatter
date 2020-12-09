using AutoFixture;
using AutoFixture.AutoMoq;

namespace log4net.JsonLayout.Tests
{
	public static class FixtureHelper
	{
		public static IFixture CreateFixture()
		{
			var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });

			// client has a circular reference from AutoFixture point of view
			fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
			fixture.Behaviors.Add(new OmitOnRecursionBehavior());

			return fixture;
		}
	}
}