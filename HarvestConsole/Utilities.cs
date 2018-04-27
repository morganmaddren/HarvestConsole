using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole
{
    static class Utilities
    {
        public static List<string> SplitAndTrim(this string me, char separator, StringSplitOptions options)
        {
            if (string.IsNullOrWhiteSpace(me))
            {
                return new List<string>();
            }

            return me.Split(new[] { separator }, options).Select(x => x.Trim()).ToList();
        }

    }
}
