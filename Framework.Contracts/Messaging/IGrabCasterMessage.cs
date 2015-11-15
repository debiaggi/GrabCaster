using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IGrabCasterMessage
    {
        IDictionary<string, object> Properties { get; set; }

        byte[] Body { get; set; }
    }
}
