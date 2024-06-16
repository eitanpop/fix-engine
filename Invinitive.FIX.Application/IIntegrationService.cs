using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invinitive.FIX.Application
{
    public interface IIntegrationService
    {
        void PostAdminMessage(string message);
        void PostApplicationMessage(string message);
    }
}
