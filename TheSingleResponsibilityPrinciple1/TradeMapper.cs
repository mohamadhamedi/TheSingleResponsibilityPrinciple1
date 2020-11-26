using TheSingleResponsibilityPrinciple1.Interfaces;

namespace TheSingleResponsibilityPrinciple1
{
    class TradeMapper : ITradeMapper
    {
        public TradeRecord Map(string[] fields)
        {
            var sourceCurrencyCode = fields[0].Substring(0, 3);
            var destinationCurrencyCode = fields[0].Substring(3, 3);
            int tradeAmount = int.Parse(fields[1]);
            int tradePrice = int.Parse(fields[2]);
            //calculate values
            var trade = new TradeRecord
            {
                SourceCurrency = sourceCurrencyCode,
                DestinationCurrency = destinationCurrencyCode,
                Lots = tradeAmount / LotSize,
                Price = tradePrice
            };
            return trade;
        }
        private static float LotSize = 100000f;

    }
}
