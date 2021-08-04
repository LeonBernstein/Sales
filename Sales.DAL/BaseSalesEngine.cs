
using Sales.Common.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sales.DAL
{
    public abstract class BaseSalesEngine
    {
        internal const string FOLDER_NAME = "JsonDb";
        internal const string FILE_EXT = ".json";

        internal readonly IJsonFileHandlerService _jsonFileHandler;

        protected BaseSalesEngine(IJsonFileHandlerService jsonFileHandler) => _jsonFileHandler = jsonFileHandler;

        protected async Task<List<T>> GetItemsAsync<T>(EntityTypes type)
        {
            return await _jsonFileHandler.ReadFromFileAsync<List<T>>(type.Value + FILE_EXT, FOLDER_NAME);
        }

        protected async Task InsertItemAsync<T>(EntityTypes type, T item)
        {
            List<T> items = await GetItemsAsync<T>(type);
            items.Add(item);
            await _jsonFileHandler.WriteToFileAsync(type.Value + FILE_EXT, FOLDER_NAME, items);
        }
    }
}
