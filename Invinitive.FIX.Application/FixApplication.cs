using QuickFix;
using QuickFix.Fields;
using Invinitive.FIX.Engine;
using Microsoft.Extensions.Logging;

namespace Invinitive.FIX.Application
{
    public class FixApplication : MessageCracker, IFixApplication
    {
        private readonly ILogger<FixApplication> _logger;
        private readonly IIntegrationService _integrationService;
        private Session? _session;

        public FixApplication(ILogger<FixApplication> logger, IIntegrationService integrationService)
        {
            _logger = logger;
            _integrationService = integrationService;
        }
        
        public void FromAdmin(Message message, SessionID sessionId)
        {
            _logger.LogInformation("Received admin message: " + message);
            _integrationService.PostAdminMessage(message.ToString());
        }
        
        public void FromApp(Message message, SessionID sessionId)
        {
            _logger.LogInformation("Received application message: " + message);
            Crack(message, sessionId);
        }

        public void OnCreate(SessionID sessionId)
        {
            _session = Session.LookupSession(sessionId);
            _logger.LogInformation("Session created: " + sessionId);
        }

        public void OnLogout(SessionID sessionId)
        {
            _logger.LogInformation("Logged out: " + sessionId);
        }

        public void OnLogon(SessionID sessionId)
        {
            _logger.LogInformation("Logged in: " + sessionId);
        }

        public void OnMessage(QuickFix.FIX42.ExecutionReport message,
            SessionID session)
        {
            _logger.LogInformation("Execution Report");
        }

        public void ToAdmin(Message message, SessionID sessionId)
        {
            _logger.LogInformation("Sending admin message: " + message);
        }

        public void ToApp(Message message, SessionID sessionId)
        {
            _logger.LogInformation("Sending application message: " + message);
        }
        public bool SendMessage(Message message)
        {
            if (_session == null)
                throw new InvalidOperationException("Session is not initialized");
            return _session.Send(message);
        }
    }
}
