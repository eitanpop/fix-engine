using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invinitive.FIX.Application.DTO.Events
{
    public class MarketOrder
    {
        public string? OrderId { get; set; }
        public string? Symbol { get; set; }
        public bool IsBuy { get; set; }
        public int Quantity { get; set; }
        public string ISIN { get; set; }
        public string SecurityExchange { get; set; }
    }
}
