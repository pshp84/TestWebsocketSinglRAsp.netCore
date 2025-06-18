using SignalRRealTimeApp.DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRRealTimeApp.DAL.Services
{
    public class LiveLogService : ILiveLogService
    {
        private readonly List<string> _logs = new();
        private readonly object _lock = new();
        public Task<bool> AddLogAsync(string log)
        {
            lock (_lock)
            {
                _logs.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {log}");

                // Limit stored logs to last 100
                if (_logs.Count > 100)
                    _logs.RemoveAt(0);
            }
            return Task.FromResult(true);
        }

        public async Task <List<string>> GetLogs()
        {
            lock (_lock)
            {
                return _logs.ToList();
            }
        }
    }
}
