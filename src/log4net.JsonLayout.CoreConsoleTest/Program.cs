using log4net.Config;
using System;
using System.IO;
using System.Reflection;

namespace log4net.JsonLayout.CoreConsoleTest
{

	class Program
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		static void Main(string[] args)
		{
			// Load configuration
			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

			log.Info("Hello logging world!");

			log.Warn(new { Id=1, Name="Alper" });

			log.Error(new { Id=1, Name= "Alper" }, new ArgumentNullException(nameof(args)));

			Console.WriteLine("Hit enter");
			Console.ReadLine();
		}
	}
}
