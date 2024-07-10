
namespace WinTracker.Models
{
    public class ProcessInfo
    {
        public uint ProcessId { get; set; }
        public string ProcessName { get; set; }

        public ProcessInfo(uint processId, string processName)
        {
            ProcessId = processId;
            ProcessName = processName;
        }         
    }
}