
namespace WinTracker.Models
{
    public class ProcessInfo
    {
        public uint ProcessId { get; private set; }
        public string ProcessName { get; private set; }

        public ProcessInfo(uint processId, string processName)
        {
            ProcessId = processId;
            ProcessName = processName;
        }
    }
}