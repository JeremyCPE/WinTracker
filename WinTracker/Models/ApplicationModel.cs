using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTracker.Models
{
    public class ApplicationModel
    {
        public string ProcessId { get; set; }

        public string ProcessName { get; set; }
        public TimeSpan UsageTime { get; set; }
        public State State { get; set; }

        public TimeSpan TimeStart {get;set;}
        public ApplicationModel(string processId, string processName, State state = State.Running)
        {
            ProcessId = processId;
            ProcessName = processName;
            UsageTime = TimeSpan.Zero;
            State = state;
            TimeStart = DateTime.Now.TimeOfDay;
        }
    }

    public enum State
    {
        Running,
        Stopped,
        Unknown
    }
}
