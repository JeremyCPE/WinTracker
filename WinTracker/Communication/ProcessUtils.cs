using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTracker.Communication
{
    internal class ProcessUtils
    {
        public static Process? GetForegroundProcess()
        {
            try
            {
                IntPtr hWnd = NativeMethods.GetForegroundWindow();
                NativeMethods.GetWindowThreadProcessId(hWnd, out uint processID);
                return Process.GetProcessById(Convert.ToInt32(processID)); // Get it as a C# obj.;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
