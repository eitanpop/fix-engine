using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invinitive.FIX.Application.DTO.Events
{
    public class ForexConversion
    {
        public string? OrderId { get; set; }
        public bool IsBuy { get; set; }
        public string? FromCurrency { get; set; }
        public string? ToCurrency { get; set; }

    }
}
