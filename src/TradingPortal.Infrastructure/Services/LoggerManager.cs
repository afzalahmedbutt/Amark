//using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Infrastructure.Services
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILogger _logger;

        public LoggerManager(ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            string envName = env.EnvironmentName;
            _logger = loggerFactory.CreateLogger("DbLogger");
        }

        public void LogDebug(Exception ex)
        {
            Log(ex, LogLevel.Debug);
            
        }

        public void LogError(Exception ex)
        {
            Log(ex, LogLevel.Error);
        }

        public void LogInfo(Exception ex)
        {
            Log(ex, LogLevel.Information);
            
        }

        public void LogWarn(Exception ex)
        {
            Log(ex, LogLevel.Warning);
            
        }

        private void Log(Exception ex, LogLevel level)
        {

            var msg = $"Something went wrong: {ex}";
            //var rowGuid = Guid.NewGuid();
            _logger.Log(level,
                default(EventId),
                new MyLogEvent(ex.ToString())
                .AddProp("ShortMessage", ex.GetBaseException()?.Message ?? "")
                .AddProp("FullMessage", msg),
                //.AddProp("LogUniqueId", rowGuid),
                //.AddProp("IpAddress", currentUser.GetCurrentIpAddress())
                //.AddProp("PageUrl", currentUser.GetAbsoluteUri())
                //.AddProp("CustomerId", currentUser.User == null ? default(int?) : currentUser.User.Id),
                (Exception)null,
                MyLogEvent.Formatter);
            //return rowGuid;

        }

    }

    class MyLogEvent : IEnumerable<KeyValuePair<string, object>>
    {
        List<KeyValuePair<string, object>> _properties = new List<KeyValuePair<string, object>>();

        public string Message { get; }

        public MyLogEvent(string message)
        {
            Message = message;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public MyLogEvent AddProp(string name, object value)
        {
            _properties.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        public static Func<MyLogEvent, Exception, string> Formatter { get; } = (l, e) => l.Message;
    }
}
