using System.Collections.Generic;

namespace TheSingleResponsibilityPrinciple1.Interfaces
{
    public interface ITradeDataProvider
    {
        IEnumerable<string> GetTradeData();
    }
}
