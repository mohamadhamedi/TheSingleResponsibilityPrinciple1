using System.Collections.Generic;
using System.IO;
using TheSingleResponsibilityPrinciple1.Interfaces;

namespace TheSingleResponsibilityPrinciple1
{
    internal class StreamDataTradeProvider : ITradeDataProvider
    {
        public StreamDataTradeProvider(Stream stream)
        {
            _stream = stream;
        }
        public IEnumerable<string> GetTradeData()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(_stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }
        private readonly Stream _stream;
    }
}
