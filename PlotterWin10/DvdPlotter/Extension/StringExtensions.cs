using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DvdPlotter.Extension
{
    public static class StringExtensions
    {
        public static string Fmt(this string format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}
