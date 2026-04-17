using LoggingInterface.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingInterface.Interface
{
    public interface IDBConnection
    {
        public Task SaveLogAsync(LogModel log);
    }
}
