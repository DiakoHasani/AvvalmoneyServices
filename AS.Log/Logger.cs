using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AS.Log
{
    public class Logger : ILogger
    {
        private readonly Serilog.Core.Logger _logger;
        private readonly IClient _client;
        public Logger(IClient client)
        {
            _client = client;
            _logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341")
                .WriteTo.File(GetFilePath(), outputTemplate: GetOutputTemplate())
                .CreateLogger();
        }

        private string GetFilePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + @"App_Data\log.txt";
        }

        private string GetOutputTemplate()
        {
            return "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Message}{NewLine}{Exception}";
        }
        private Serilog.ILogger GetForContext(object model, string callerFilePath, long callerLineNumber, string callerMember)
        {
            var ip = _client.GetIp();
            return _logger.ForContext("FilePath", callerFilePath).ForContext("LineNumber", callerLineNumber)
                .ForContext("Method", callerMember).ForContext("HostName", ip.HostName).ForContext("HostAddresses", ip.HostAddresses)
                .ForContext("Ipv6", ip.Ipv6).ForContext("Model", model != null ? JsonConvert.SerializeObject(model) : "");
        }

        public void Information(string templateMessage, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "")
        {
            GetForContext(null, callerFilePath, callerLineNumber, callerMember).Information(templateMessage);
        }

        public void Information(string templateMessage, Exception ex, object model, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "")
        {
            GetForContext(model, callerFilePath, callerLineNumber, callerMember).Information(ex, templateMessage);
        }

        public void Information(string templateMessage, Exception ex, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "")
        {
            GetForContext(null, callerFilePath, callerLineNumber, callerMember).Information(ex, templateMessage);
        }

        public void Information(string templateMessage, object model, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "")
        {
            GetForContext(model, callerFilePath, callerLineNumber, callerMember).Information(templateMessage);
        }

        public void Warning(string templateMessage, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "")
        {
            GetForContext(null, callerFilePath, callerLineNumber, callerMember).Warning(templateMessage);
        }

        public void Warning(string templateMessage, Exception ex, object model, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "")
        {
            GetForContext(model, callerFilePath, callerLineNumber, callerMember).Warning(ex, templateMessage);
        }

        public void Warning(string templateMessage, Exception ex, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "")
        {
            GetForContext(null, callerFilePath, callerLineNumber, callerMember).Warning(ex, templateMessage);
        }

        public void Warning(string templateMessage, object model, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "")
        {
            GetForContext(model, callerFilePath, callerLineNumber, callerMember).Warning(templateMessage);
        }

        public void Error(string templateMessage, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "")
        {
            GetForContext(null, callerFilePath, callerLineNumber, callerMember).Error(templateMessage);
        }

        public void Error(string templateMessage, Exception ex, object model, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0,
            [CallerMemberName] string callerMember = "")
        {
            GetForContext(model, callerFilePath, callerLineNumber, callerMember).Error(ex, templateMessage);
        }

        public void Error(string templateMessage, Exception ex, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "")
        {
            GetForContext(null, callerFilePath, callerLineNumber, callerMember).Error(ex, templateMessage);
        }

        public void Error(string templateMessage, object model, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "")
        {
            GetForContext(model, callerFilePath, callerLineNumber, callerMember).Error(templateMessage);
        }

    }
    public interface ILogger
    {
        void Information(string templateMessage, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "");

        void Information(string templateMessage, Exception ex, object model, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "");

        void Information(string templateMessage, Exception ex, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "");

        void Information(string templateMessage, object model, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "");

        void Warning(string templateMessage, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "");

        void Warning(string templateMessage, Exception ex, object model, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "");

        void Warning(string templateMessage, Exception ex, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "");

        void Warning(string templateMessage , object model, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "");

        void Error(string templateMessage, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "");

        void Error(string templateMessage, Exception ex, object model, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "");

        void Error(string templateMessage, Exception ex, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "");

        void Error(string templateMessage, object model, [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] long callerLineNumber = 0, [CallerMemberName] string callerMember = "");
    }
}
