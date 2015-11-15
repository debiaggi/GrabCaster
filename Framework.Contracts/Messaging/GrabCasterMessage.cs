using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    [Serializable]
    public class GrabCasterMessage: IGrabCasterMessage
    {
        public IDictionary<string, object> Properties { get; set; }

        public byte[] Body { get; set; }

        public GrabCasterMessage(byte[] body)
        {
            Properties = new Dictionary<string, object>();
            Body = body;
        }


    }
}
