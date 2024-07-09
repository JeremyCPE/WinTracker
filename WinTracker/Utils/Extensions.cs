using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTracker.Utils
{
    public static class Extensions
    {
        // https://stackoverflow.com/questions/11668965/add-to-collection-if-not-null
        public static void AddIfNotNull<T, U>(this IDictionary<T, U> dic, T? key, U value)
        {
            if (key != null) { dic.Add(key, value); }
        }
    }
}
