using log4net;
using TheSingleResponsibilityPrinciple1.Interfaces;

namespace TheSingleResponsibilityPrinciple1
{
    class Log4NetLoggerAdabpter : ILogger
    {
        private readonly ILog log;

        public Log4NetLoggerAdabpter(log4net.ILog log)
        {
            this.log = log;
        }
        public void LogError(string message, params object[] args)
        {
            log.ErrorFormat(message, args);
        }

        public void LogInfo(string message, params object[] args)
        {
            log.InfoFormat(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            log.WarnFormat(message,args);
        }
    }
}
