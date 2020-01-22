using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using ImageCaster.Checks;
using ImageCaster.Api;
using ImageCaster.Extensions;
using NLog;
using ICommand = ImageCaster.Api.ICommand;

namespace ImageCaster.Commands
{
    /// <summary>
    /// Performs any validation to ensure the project structure adheres
    /// to all standards, without actually exporting any images.
    /// </summary>
    public class CheckCommand : ICommand
    {
        /// <summary>The NLog logger.</summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>The collector implementation to use to collect files.</summary>
        public ICollector Collector { get; }
        
        /// <summary>The configured checks to perform.</summary>
        public Configuration.Checks Checks { get; }

        public CheckCommand(ICollector collector, Configuration.Checks checks)
        {
            this.Collector = collector.RequireNonNull();
            this.Checks = checks;
        }
        
        public Command Configure()
        {
            return new Command("check", "Validate that the project structure and standards are maintained")
            {
                Handler = CommandHandler.Create(Execute)
            };
        }

        public int Execute()
        {
            Logger.Debug("Executed Check command, running checks.");

            if (Checks == null)
            {
                Logger.Info("Executed check command, however no checks are defined in the configuration; doing nothing.");
                return 0;
            }

            List<Failure> failures = new List<Failure>();

            if (Checks.FileExists != null)
            {
                FileExistsChecker checker = new FileExistsChecker(Collector, Checks.FileExists);
                failures.AddRange(checker.Check());
            }

            if (Checks.NamingConvention != null)
            {
                NamingConventionChecker checker = new NamingConventionChecker(Collector, Checks.NamingConvention);
                failures.AddRange(checker.Check());
            }

            if (Checks.PowerOfTwo != null)
            {
                PowerOfTwoChecker checker = new PowerOfTwoChecker(Collector, Checks.PowerOfTwo);
                failures.AddRange(checker.Check());
            }

            if (Checks.ResolutionMatches != null)
            {
                MaskResolutionChecker checker = new MaskResolutionChecker(Collector, Checks.ResolutionMatches);
                failures.AddRange(checker.Check());
            }
            
            foreach (Failure failure in failures)
            {
                Logger.Warn(failure);
            }

            return failures.Count == 0 ? 0 : 1;        }
    }
}
