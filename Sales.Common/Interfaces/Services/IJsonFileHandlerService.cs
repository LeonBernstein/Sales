using System.Threading.Tasks;

namespace Sales.Common.Interfaces.Services
{
    public interface IJsonFileHandlerService
    {
        Task<T> ReadFromFileAsync<T>(string fileName, string folderName);
        Task WriteToFileAsync<T>(string fileName, string folderName, T item);
    }
}
