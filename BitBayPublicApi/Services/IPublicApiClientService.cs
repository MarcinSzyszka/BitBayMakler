using System.Threading.Tasks;
using BitBayPublicApi.Models.Requests;

namespace BitBayPublicApi.Services
{
    public interface IPublicApiClientService
    {
        Task<TResponseData> GetData<TResponseData, TRequestData>(TRequestData requestData) where TRequestData : RequestBase;
    }
}
