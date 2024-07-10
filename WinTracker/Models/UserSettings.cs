using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace WinTracker.Models
{
    public class UserSettings
    {
        public bool RunAtStart { get; set; }
        public List<string> BlackList { get; set; }
    }
}
