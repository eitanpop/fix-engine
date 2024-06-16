using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invinitive.FIX.Application
{
    public interface IQueueWriter
    {
        Task<bool> WriteAsync(string message);
    }
}
