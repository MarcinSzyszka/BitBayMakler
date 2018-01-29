using System;
using System.Net.Http;
using System.Threading.Tasks;
using BitBayCommon.Settings;
using BitBayPublicApi.Models.Requests;
using Newtonsoft.Json;

namespace BitBayPublicApi.Services
{
    public class PublicApiClientService : IPublicApiClientService
    {
        private readonly HttpClient _client;

        public PublicApiClientService(ISettings settings)
        {
            _client = new HttpClient { BaseAddress = new Uri(settings.PublicApiUrl) };
        }
        public async Task<TResponseData> GetData<TResponseData, TRequestData>(TRequestData requestData) where TRequestData : RequestBase
        {
            var response = await _client.GetAsync(requestData.GetEndPointUrl());

            if (response.IsSuccessStatusCode)
            {
                var responseDataString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TResponseData>(responseDataString);
            }


            //TODO wrap response in ApiResponseModel with more information what went wrong
            return default(TResponseData);
        }
    }
}
