using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Racing.Util
{
    /// <summary>
    /// Group of conversion methods -- Add more here when necessary
    /// </summary>
    public static class Converter
    {
        public static float MillisecondsToSeconds(float milliseconds)
        {
            return milliseconds / 1000;
        }
    }
}
