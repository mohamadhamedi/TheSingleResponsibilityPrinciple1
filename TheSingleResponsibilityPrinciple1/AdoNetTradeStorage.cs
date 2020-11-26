using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TheSingleResponsibilityPrinciple1.Interfaces;

namespace TheSingleResponsibilityPrinciple1
{
    public class AdoNetTradeStorage : ITradeStorage
    {
        private readonly ILogger logger;

        public AdoNetTradeStorage(ILogger logger)
        {
            this.logger = logger;
        }

        public void Persist(IEnumerable<TradeRecord> tradeRecords)
        {
            using (var connection = new SqlConnection("Data Source=.;Initial Catalog=TradeDatabe;Integrated Security=True;"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var trade in tradeRecords)
                    {
                        var command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "dbo_insert";
                        command.Parameters.AddWithValue("@sourceCurency", trade.SourceCurrency);
                        command.Parameters.AddWithValue("@desimationCurrency", trade.DestinationCurrency);
                        command.Parameters.AddWithValue("@lots", trade.Lots);
                        command.Parameters.AddWithValue("@price", trade.Price);
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                connection.Close();
            }
            logger.LogWarning("INFO:{0} trades processed", tradeRecords.Count());
        }
    }
}
