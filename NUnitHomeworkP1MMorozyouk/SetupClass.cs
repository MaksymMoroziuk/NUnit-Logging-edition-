using NUnit.Framework;
using Serilog;
using System;
using System.IO;
using Serilog.Enrichers;

namespace NUnitHomeworkP1MMorozyouk
{
    [SetUpFixture]
    public class SetupClass
    {
        string testDirPath;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            testDirPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestResults", DateTime.Now.ToString("yyyy-MM-dd-HH-mm"));
            string logsPath = Path.Combine(testDirPath, "logs.txt");
            string detailedLogsPath = Path.Combine(testDirPath, "detailedLogs.txt");

            Log.Logger = new LoggerConfiguration()
            .Enrich.WithProcessId()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(logsPath, Serilog.Events.LogEventLevel.Information)
            .WriteTo.File(detailedLogsPath, Serilog.Events.LogEventLevel.Debug)
            .CreateLogger();

            Log.Logger.Debug("Finished global one time setup");
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            var sourcePath = testDirPath;
            var destinationPath = Path.Combine("C:\\TestResults", Path.GetFileName(testDirPath));

            Directory.CreateDirectory(destinationPath);

            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);

            Log.Logger.Debug("Finished global one time tear down");
            Log.CloseAndFlush();
        }
    }
}
