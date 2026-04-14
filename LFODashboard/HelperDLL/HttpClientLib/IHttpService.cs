using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLib
{
 
        public interface IHttpService
        {
            Task<T> GetAsync<T>(string url, Dictionary<string, string>? headers = null);
            Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data, Dictionary<string, string>? headers = null);
        }
    
}
