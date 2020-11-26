
namespace TheSingleResponsibilityPrinciple1.Interfaces
{
    class SimpleTradeValidator : ITradeValidator
    {
     
        private readonly ILogger logger;

        public SimpleTradeValidator(ILogger logger)
        {
            this.logger = logger;
        }

        public bool Validate(string[] tradeDate)
        {
            if (tradeDate.Length != 3)
            {
                logger.LogWarning("Warn:Line malformed.Only {1} founds", tradeDate);
                return false;
            }

            if (tradeDate[0].Length != 6)
            {
                logger.LogWarning("Warn:Trade counrecies  malformed:'{1}'", tradeDate[0]);
                return false;

            }

            int tradeAmount = 0;
            if (!int.TryParse(tradeDate[1], out tradeAmount))
            {
                logger.LogWarning("Warn:Trade amount is not a valid integer:'{1}'", tradeDate[1]);
                return false;
            }

            decimal tradePrice;
            if (!decimal.TryParse(tradeDate[2], out tradePrice))
            {
                logger.LogWarning("Warn:Trade price  is not a valid decimal:'{1}'", tradeDate[2]);
                return false;
            }
            return true;

        }
    }
}
