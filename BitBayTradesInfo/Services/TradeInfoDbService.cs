using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BitBayCommon.Settings;
using BitBayPublicApi.Models;
using Newtonsoft.Json;

namespace BitBayTradesInfo.Services
{
    public class TradeInfoDbService : ITradeInfoDbService
    {
        private readonly string _dataDir;

        private string _tradeTransactionsFileName = "tradeTransactions.bbdb";

        private string _tradeTransactionsFilePath;

        public TradeInfoDbService(ISettings settings)
        {
            _dataDir = settings.DataDirectoryPath;
            Initialize();
        }

        private void Initialize()
        {
            if (!Directory.Exists(_dataDir))
            {
                Directory.CreateDirectory(_dataDir);
            }

            _tradeTransactionsFilePath = Path.Combine(_dataDir, _tradeTransactionsFileName);

            if (!File.Exists(_tradeTransactionsFilePath))
            {
                File.Create(_tradeTransactionsFilePath);
            }
        }

        public async Task<IEnumerable<TradeTransaction>> GetStoredTransactions()
        {
            using (var fs = new FileStream(_tradeTransactionsFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                {
                    var transactions = await sr.ReadToEndAsync();

                    return JsonConvert.DeserializeObject<IEnumerable<TradeTransaction>>(transactions);
                }
            }
        }

        public async Task SaveTransactions(IEnumerable<TradeTransaction> transactions)
        {
            using (var fs = new FileStream(_tradeTransactionsFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var sr = new StreamWriter(fs))
                {
                    var transactionsString = JsonConvert.SerializeObject(transactions);

                    await sr.WriteAsync(transactionsString);
                }
            }
        }
    }
}
