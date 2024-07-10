using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WinTracker.Models
{
    public class ApplicationInfo
    {
        public Guid Guid { get; set; }

        public Icon? Icon { get; set; }
        public ProcessInfo ProcessInfo { get; set; }
        
        public Category Category { get; set; }

        public State State { get; set; }
        public TimeSpan TimeElapsed { get; set; }

        private TimeSpan TimeCreated { get; }


        public ApplicationInfo(ProcessInfo processInfo, Category category)
        {
            this.Guid = Guid.NewGuid();
            this.ProcessInfo = processInfo;
            this.Category = category;

            this.TimeElapsed = TimeSpan.Zero;
            this.TimeCreated = DateTime.Now.TimeOfDay;
            Start();

        }

        public void Stop()
        {
           if(State is State.Stopped) return;
           State = State.Stopped;
           TimeElapsed = DateTime.Now.TimeOfDay - TimeCreated;
        }

        public void Start()
        {
            State = State.Running;
        }

        internal static ApplicationInfo ConvertFrom(Process process)
        {
            throw new NotImplementedException();
        }
    }
}
