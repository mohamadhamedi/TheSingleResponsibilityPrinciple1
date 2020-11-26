using System.Collections.Generic;

namespace TheSingleResponsibilityPrinciple1.Interfaces
{
    public interface ITradeParser
    {
        IEnumerable<TradeRecord> Parse(IEnumerable<string> lines);
    }
}
