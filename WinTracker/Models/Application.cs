using System.Security.Cryptography.X509Certificates;

namespace WinTracker.Models
{
    public class Application
    {
        public uint ProcessId { get; set; }
        public string ProcessName { get; set; }

        public Application(uint processId, string processName)
        {
            ProcessId = processId;
            ProcessName = processName;
        }         
    }
}