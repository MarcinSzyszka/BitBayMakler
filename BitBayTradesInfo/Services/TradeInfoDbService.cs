using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BitBayCommon.Settings;
using BitBayCurrencies.Enums;
using BitBayCurrencies.Extensions;
using BitBayPublicApi.Models;
using Newtonsoft.Json;

namespace BitBayTradesInfo.Services
{
    public class TradeInfoDbService : ITradeInfoDbService
    {
        private readonly string _dataDir;

        private string _tradeTransactionsFileExtension = ".bbdb";

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
        }

        public async Task<IEnumerable<TradeTransaction>> GetStoredTransactions(Currency currency)
        {
            var transactionsFilePath = GetTransactionsFilePath(currency);

            using (var fs = new FileStream(transactionsFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                {
                    var transactions = await sr.ReadToEndAsync();

                    if (!string.IsNullOrEmpty(transactions))
                    {
                        return JsonConvert.DeserializeObject<IEnumerable<TradeTransaction>>(transactions);
                    }

                    return new List<TradeTransaction>();
                }
            }
        }

        public async Task SaveTransactions(IEnumerable<TradeTransaction> transactions, Currency currency)
        {
            var transactionsFilePath = GetTransactionsFilePath(currency);

            using (var fs = new FileStream(transactionsFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var sr = new StreamWriter(fs))
                {
                    var transactionsString = JsonConvert.SerializeObject(transactions);

                    await sr.WriteAsync(transactionsString);
                }
            }
        }

        private string GetTransactionsFilePath(Currency currency)
        {
            var filePath = Path.Combine(_dataDir, currency.GetApiParameterName() + _tradeTransactionsFileExtension);

            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }

            return filePath;
        }
    }
}
