using TheSingleResponsibilityPrinciple1.Interfaces;

namespace TheSingleResponsibilityPrinciple1
{
    public class TradeProcessor
    {
        private readonly ITradeDataProvider _tradeDataProvider;
        private readonly ITradeValidator _tradeValidator;
        private readonly ITradeParser _tradeParser;
        private readonly ITradeMapper _tradeMapper;
        private readonly ITradeStorage _tradeStorage;

        public TradeProcessor(ITradeDataProvider tradeDataProvider,
                              ITradeValidator tradeValidator,
                              ITradeParser tradeParser,
                              ITradeMapper tradeMapper,
                              ITradeStorage tradeStorage)
        {
            _tradeDataProvider = tradeDataProvider;
            _tradeValidator = tradeValidator;
            _tradeParser = tradeParser;
            _tradeMapper = tradeMapper;
            _tradeStorage = tradeStorage;
        }

        public void ProcessTrades()
        {
            var lines = _tradeDataProvider.GetTradeData();
            var _trades = _tradeParser.Parse(lines);
            _tradeStorage.Persist(_trades);
        }
    }
}
