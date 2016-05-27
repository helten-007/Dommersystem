using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NordicArenaTournament.Common
{
    /// <summary>
    /// Methods aiding in producing output for javascript
    /// </summary>
    public static class JavascriptHelper
    {
        /// <summary>
        /// Ensuring decimal points are formatted as periods, not commas
        /// </summary>
        public static String ForJavascript(this decimal val)
        {
            IFormatProvider fp = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            return val.ToString(fp); 
        }

        /// <summary>
        /// Convert bool to a javascript value which will evaluate to true/false with == operator.
        /// </summary>
        public static String ForJavascript(this bool val)
        {
            return val ? "true" : ""; // Any non-empty string will evaluate to true with == operator in javascript
        }


        /// <summary>
        /// Convert bool to a javascript value which will evaluate to true/false with == operator.
        /// </summary>
        public static String ForJavascript(this decimal? val)
        {
            return val != null ? ForJavascript(val.Value) : "";
        }
    }
}