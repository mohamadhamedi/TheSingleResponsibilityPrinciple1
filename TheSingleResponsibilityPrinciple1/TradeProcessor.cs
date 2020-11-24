using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace TheSingleResponsibilityPrinciple1
{
    public class TradeProcessor
    {
        public void ProcessTrades(Stream stream)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            var trades = new List<TradeRecord>();
            var lineCount = 1;
            foreach (var line in lines)
            {
                var fields = line.Split(',');
                if (fields.Length != 3)
                {
                    Console.WriteLine("Warn:Line {0} malformed.Only {1} founds", lineCount, fields);
                    continue;
                }

                if (fields[0].Length != 6)
                {
                    Console.WriteLine("Warn:Trade counrecies on line {0} malformed:'{1}'", lineCount, fields[0]);
                    continue;
                }

                int tradeAmount = 0;
                if (!int.TryParse(fields[1], out tradeAmount))
                {
                    Console.WriteLine("Warn:Trade amount on line {0} is not a valid integer:'{1}'", lineCount, fields[1]);
                }

                decimal tradePrice;
                if (!decimal.TryParse(fields[2], out tradePrice))
                {
                    Console.WriteLine("Warn:Trade price on line {0} is not a valid decimal:'{1}'", lineCount, fields[2]);
                }

                var sourceCurrencyCode = fields[0].Substring(0, 3);
                var destinationCurrencyCode = fields[0].Substring(3, 3);

                //calculate values
                var trade = new TradeRecord
                {
                    SourceCurrency = sourceCurrencyCode,
                    DestinationCurrency = destinationCurrencyCode,
                    Lots = tradeAmount / LotSize,
                    Price = tradePrice
                };

                trades.Add(trade);
                lineCount++;
            }

            using (var connection = new SqlConnection("Data Source=.;Initial Catalog=TradeDatabe;Integrated Security=True;"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var trade in trades)
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
            Console.WriteLine("INFO:{0} trades processed", trades.Count);
        }
        private static float LotSize = 100000f;

    }
}
