using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_FirstApplication.Const
{
    public static class Extentions
    {
        public static string FixFormate(this string value)
        {
            return value.Split('.')[0].Replace("-", string.Empty);
        }
    }
}
