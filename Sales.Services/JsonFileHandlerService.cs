using Sales.Common.Interfaces.Services;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sales.Services
{
    public class JsonFileHandlerService : IJsonFileHandlerService
    {
        private const string JSON_FILE_EXT = ".json";
        private const int FILE_ACCESS_LIMIT = 1;

        private readonly TimeSpan _semaphoreWaitTimeout = TimeSpan.FromSeconds(20);
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new();

        public async Task<T> ReadFromFileAsync<T>(string fileName, string folderName)
        {
            ValidateFileArguments(fileName, folderName);
            if (!fileName.EndsWith(JSON_FILE_EXT)) fileName += JSON_FILE_EXT;
            string filePath = Path.Combine(folderName, fileName);
            SemaphoreSlim semaphore = new(FILE_ACCESS_LIMIT);
            semaphore = _semaphores.GetOrAdd(filePath, semaphore);
            try
            {
                await semaphore.WaitAsync(_semaphoreWaitTimeout);
                string fileStrResult = await File.ReadAllTextAsync(filePath);
                T result = (T)JsonConvert.DeserializeObject(fileStrResult, typeof(T));
                return result;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public Task WriteToFileAsync(string fileName, string folderName, object data)
        {
            ValidateFileArguments(fileName, folderName, data);
            throw new NotImplementedException();
        }

        private static void ValidateFileArguments(string fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("\"fileName\" cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(folderName))
            {
                throw new ArgumentException("\"folderName\" cannot be null or empty.");
            }
        }

        private static void ValidateFileArguments(string fileName, string folderName, object data)
        {
            if (data == null)
            {
                throw new ArgumentException("\"data\" cannot.");
            }
            ValidateFileArguments(fileName, folderName);
        }
    }
}
