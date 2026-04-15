using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core
{
    public class ApiRequest<T>
    {
        public T? Data { get; set; }
        public string? Requestedby  { get; set; }
        public string? RequestId { get; set; }
    }
}
