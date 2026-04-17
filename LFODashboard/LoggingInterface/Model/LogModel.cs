using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingInterface.Model
{
    public class LogModel
    {
        public string Path { get; set; }
        public string Method { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public int StatusCode { get; set; }

        public string IPAddress { get; set; }      // ✅ new
        public string UserId { get; set; }         // ✅ new
        public long ExecutionTimeMs { get; set; }  // ✅ new

        public DateTime CreatedAt { get; set; }
    }
}
