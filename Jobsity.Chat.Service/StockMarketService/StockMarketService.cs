using CsvHelper;
using Jobsity.Chat.Contracts.DTOs;
using Jobsity.Chat.Contracts.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Jobsity.Chat.Contracts.Interfaces;
using Newtonsoft.Json;

namespace Jobsity.Chat.Service.StockMarketService
{
    public class StockMarketService : IStockMarketService
    {
        private readonly HttpClient _httpClient;
        private readonly StockMarketSettings _settings;

        public StockMarketService(HttpClient httpClient, IOptions<StockMarketSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings?.Value;
        }

        public async Task<StockSymbolQuoteDto> GetQuoteAsync(string symbol)
        {
            StockSymbolQuoteDto dtoResponse = null;
            var stockDataFormat = GetStockMarketDataFormat();
            var urlApi = string.Format(_settings.ApiUrl, symbol, Enum.GetName(typeof(StockDataFormat), stockDataFormat));
            try
            {
                if (stockDataFormat == StockDataFormat.csv)
                {
                    dtoResponse = await GetQuoteFromCsvAsync(urlApi, symbol);
                }
                else if (stockDataFormat == StockDataFormat.json)
                {
                    dtoResponse = await GetQuoteFromJsonAsync(urlApi, symbol);
                }
            }
            catch (Exception ex)
            {
                // ToDo: Handling and logging exceptions
                // throw;
            }

            return dtoResponse;
        }

        private async Task<StockSymbolQuoteDto> GetQuoteFromCsvAsync(string urlApi, string symbol)
        {
            StockSymbolQuoteDto dtoResponse = null;
            using (var response = await _httpClient.GetAsync(urlApi, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    using var stream = await response.Content.ReadAsStreamAsync();
                    using (var reader = new StreamReader(stream))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = new List<StockSymbolQuoteDto>();
                        csv.Read();
                        csv.ReadHeader();
                        while (csv.Read())
                        {
                            var record = new StockSymbolQuoteDto
                            {
                                Symbol = csv.GetField<string>("Symbol"),
                                Date = csv.GetField<DateTime>("Date"),
                                Time = csv.GetField<TimeSpan>("Time"),
                                Open = csv.GetField<decimal>("Open"),
                                High = csv.GetField<decimal>("High"),
                                Low = csv.GetField<decimal>("Low"),
                                Close = csv.GetField<decimal>("Close"),
                                Volume = csv.GetField<double>("Volume"),
                            };                            
                            records.Add(record);
                        }
                        dtoResponse = records.FirstOrDefault();
                    }
                }
            }
            return dtoResponse;
        }

        private async Task<StockSymbolQuoteDto> GetQuoteFromJsonAsync(string urlApi, string symbol)
        {
            StockSymbolQuoteDto dtoResponse = null;
            using (var response = await _httpClient.GetAsync(urlApi))
            {
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<StockSymbolQuoteRootDto>(jsonString);
                    dtoResponse = data.Symbols.FirstOrDefault();

                }
            }
            return dtoResponse;
        }

        private StockDataFormat GetStockMarketDataFormat()
        {
            if (Enum.TryParse(_settings.Format.ToLowerInvariant().Trim(), out StockDataFormat format))
            {
                return format;
            }
            return StockDataFormat.csv;
        }
    }

    public enum StockDataFormat
    {
        csv,
        json
    }
}
