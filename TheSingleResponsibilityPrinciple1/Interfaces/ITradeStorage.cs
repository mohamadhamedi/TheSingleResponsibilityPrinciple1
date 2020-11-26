using System.Collections.Generic;

namespace TheSingleResponsibilityPrinciple1.Interfaces
{
    public interface ITradeStorage
    {
        public void Persist(IEnumerable<TradeRecord> tradeRecords);
    }
}
