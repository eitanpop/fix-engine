using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invinitive.FIX.Application
{
    public class WebhookIntegrationService: IIntegrationService
    {
        private readonly HttpClient _client;
        private readonly string _url;
        public WebhookIntegrationService()
        {
            _client = new HttpClient();
            _url = "https://webhook.site/4f1b1b1b-1b1b-1b1b-1b1b-1b1b1b1b1b1b";
        }

        private void PostMessage(string message)
        {
            var content = new StringContent(message, Encoding.UTF8, "application/json");
            _client.PostAsync(_url, content);
        }
        public void PostAdminMessage(string message)
        {
            PostMessage(message);
        }

        public void PostApplicationMessage(string message)
        {
            PostMessage(message);
        }
    }
}
