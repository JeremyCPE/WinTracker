using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTracker.Models
{
    public class ApplicationInfo
    {
        public Guid Guid { get; set; }
        public Application Application { get; set; }
        
        public Category ApplicationType { get; set; }

        public State State { get; set; }
        public TimeSpan TimeElapsed { get; set; }
    }
}
