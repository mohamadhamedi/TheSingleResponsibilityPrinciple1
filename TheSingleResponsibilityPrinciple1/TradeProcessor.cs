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
            var lines = ReadTradeData(stream);

            var trades = ParsTrades(lines);

            StoreTrades(trades);
        }

        private static void StoreTrades(IEnumerable<TradeRecord> trades)
        {
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

        private static IEnumerable<TradeRecord> ParsTrades(IEnumerable<string> lines)
        {
            var trades = new List<TradeRecord>();
            var lineCount = 1;
            foreach (var line in lines)
            {
                var fields = line.Split(',');
                if (!ValidateTradeData(fields, lineCount))
                {
                    continue;
                }

                var trade = MapTradeDataToTradeRecord(fields);


                trades.Add((TradeRecord)trade);
                lineCount++;
            }

            return trades;
        }

        private static object MapTradeDataToTradeRecord(string[] fields)
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

        private static bool ValidateTradeData(string[] fields, int lineCount)
        {
            if (fields.Length != 3)
            {
                Console.WriteLine("Warn:Line {0} malformed.Only {1} founds", lineCount, fields);
                return false;
            }

            if (fields[0].Length != 6)
            {
                Console.WriteLine("Warn:Trade counrecies on line {0} malformed:'{1}'", lineCount, fields[0]);
                return false;

            }

            int tradeAmount = 0;
            if (!int.TryParse(fields[1], out tradeAmount))
            {
                Console.WriteLine("Warn:Trade amount on line {0} is not a valid integer:'{1}'", lineCount, fields[1]);
                return false;
            }

            decimal tradePrice;
            if (!decimal.TryParse(fields[2], out tradePrice))
            {
                Console.WriteLine("Warn:Trade price on line {0} is not a valid decimal:'{1}'", lineCount, fields[2]);
                return false;
            }
            return true;

        }

        private static IEnumerable<string> ReadTradeData(Stream stream)
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

            return lines;
        }

        private static float LotSize = 100000f;

    }
}
