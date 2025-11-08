using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Workflow.API.JWT
{
    public class JwtHandlerEvents : JwtBearerEvents
    {
        private readonly RevokableJwtSecurityTokenHandler _handler;

        public JwtHandlerEvents(RevokableJwtSecurityTokenHandler handler)
        {
            _handler = handler;
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {
            context.Options.TokenHandlers.Clear();
            context.Options.TokenHandlers.Add(_handler);
            return Task.CompletedTask;
        }
    }
}