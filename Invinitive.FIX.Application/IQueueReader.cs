using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Invinitive.FIX.Application
{
    public interface IQueueReader
    {
        Task<IList<string>?> ReadAsync(bool deleteAfterRead);
    }
}
