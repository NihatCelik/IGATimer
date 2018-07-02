using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IGATimer
{
    internal class ResourceAccessor
    {
        public static Uri Get(string respurcePath)
        {
            var uri = string.Format(
                "pack://application:,,,/{0};component/{1}"
                , Assembly.GetExecutingAssembly().GetName().Name
                , respurcePath
            );

            return new Uri(uri);
        }
    }
}
