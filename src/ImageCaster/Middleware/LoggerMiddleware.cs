using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using ImageMagick;
using NLog;

namespace ImageCaster.Middleware
{
    /// <summary>
    /// A middleware for System.CommandLine to have a --license or -L option
    /// that shows the softwares license.
    /// </summary>
    public static class LoggerMiddleware
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public static CommandLineBuilder UseLogger(this CommandLineBuilder commandLineBuilder)
        {
            commandLineBuilder.AddOption(new Option(new[] {"--level", "-l"}, "The minimum logging level use during runtime")
            {
                Argument = new Argument<string>("level", "Info")
            });

            commandLineBuilder.AddOption(new Option(new[] {"--timestamp", "-t"}, "Include the timestamp when logging")
            {
                Argument = new Argument<bool>("timestamp", false)
            });
            
            commandLineBuilder.UseMiddleware(async (context, next) =>
            {
                CommandResult rootCommandResult = context.ParseResult.RootCommandResult;
                string level = rootCommandResult.ValueForOption<string>("-l");
                
                if (LogLevel.AllLevels.All(lvl => lvl.Name != level))
                {
                    Logger.Fatal("An invalid log level was passed to -l ({0}).", level ?? "null");
                    Environment.Exit((int)ExitCode.InvalidLogLevel);
                }
            
                LogManager.Configuration.Variables["level"] = level;
                
                bool timestamp = rootCommandResult.ValueForOption<bool>("-t");

                if (timestamp)
                {
                    LogManager.Configuration.Variables["layout"] = "[${level:uppercase=true:padding=-5}] [${date:universalTime:true:format=MM/dd HH\\:mm\\:ss}]";
                }
                
                await next(context);
            });

            return commandLineBuilder;
        }
    }
}