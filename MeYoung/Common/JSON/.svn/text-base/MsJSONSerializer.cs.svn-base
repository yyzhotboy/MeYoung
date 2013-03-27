using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Common.JSON
{
    public class MsJSONSerializer : IJSONSerializer
    {
        public MsJSONSerializer()
        {
        }

        public string Serialize(object o)
        {
            return (new JavaScriptSerializer()).Serialize(o);
        }
    }
}
